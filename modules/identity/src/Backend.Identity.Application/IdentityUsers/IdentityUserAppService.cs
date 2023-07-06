using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Identity.IdentityRoles;
using Backend.Identity.IdentityRoles.Dto;
using Backend.Identity.IdentityUsers.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.ObjectExtending;
using IdentityRole = Backend.Identity.IdentityRoles.IdentityRole;

namespace Backend.Identity.IdentityUsers;

public class IdentityUserAppService: IdentityAppServiceBase, IIdentityUserAppService
{
    private readonly IdentityUserManager _userManager;
    private readonly IIdentityUserRepository _userRepository;
    private readonly IIdentityRoleRepository _roleRepository;
    private readonly IOptions<IdentityOptions> _identityOptions;
    private readonly IUserProfileRepository _profileRepository;

    public IdentityUserAppService(
        IdentityUserManager userManager,
        IIdentityUserRepository userRepository,
        IIdentityRoleRepository roleRepository,
        IOptions<IdentityOptions> identityOptions, 
        IUserProfileRepository profileRepository)
    {
        _userManager = userManager;
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _identityOptions = identityOptions;
        _profileRepository = profileRepository;
    }

    //[Authorize(IdentityPermissions.Users.Default)]
    public virtual async Task<IdentityUserDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<IdentityUser, IdentityUserDto>(
            await _userManager.GetByIdAsync(id)
        );
    }

    //[Authorize(IdentityPermissions.Users.Default)]
    public virtual async Task<PagedResultDto<IdentityUserDto>> GetListAsync(GetIdentityUsersInput input)
    {
        var count = await _userRepository.GetCountAsync(input.Filter);
        var list = await _userRepository.GetListAsync(input.Sorting, input.MaxResultCount, input.SkipCount, input.Filter);

        return new PagedResultDto<IdentityUserDto>(
            count,
            ObjectMapper.Map<List<IdentityUser>, List<IdentityUserDto>>(list)
        );
    }

    //[Authorize(IdentityPermissions.Users.Default)]
    public virtual async Task<ListResultDto<IdentityRoleDto>> GetRolesAsync(Guid id)
    {
        //Should also include roles of the related OUs.

        var roles = await _userRepository.GetRolesAsync(id);

        return new ListResultDto<IdentityRoleDto>(
            ObjectMapper.Map<List<IdentityRole>, List<IdentityRoleDto>>(roles)
        );
    }

    //[Authorize(IdentityPermissions.Users.Default)]
    public virtual async Task<ListResultDto<IdentityRoleDto>> GetAssignableRolesAsync()
    {
        var list = await _roleRepository.GetListAsync();
        return new ListResultDto<IdentityRoleDto>(
            ObjectMapper.Map<List<IdentityRole>, List<IdentityRoleDto>>(list));
    }

    //[Authorize(IdentityPermissions.Users.Create)]
    public virtual async Task<IdentityUserDto> CreateAsync(IdentityUserCreateDto input)
    {
        await _identityOptions.SetAsync();

        var user = new IdentityUser(
            GuidGenerator.Create(),
            input.UserName,
            input.Email,
            CurrentTenant.Id
        );

        input.MapExtraPropertiesTo(user);

        (await _userManager.CreateAsync(user, input.Password)).CheckErrors();

        await UpdateUserByInput(user, input);
        (await _userManager.UpdateAsync(user)).CheckErrors();

        await CurrentUnitOfWork.SaveChangesAsync();

        return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
    }

    //[Authorize(IdentityPermissions.Users.Update)]
    public virtual async Task<IdentityUserDto> UpdateAsync(Guid id, IdentityUserUpdateDto input)
    {
        await _identityOptions.SetAsync();

        var user = await _userManager.GetByIdAsync(id);

        user.SetConcurrencyStampIfNotNull(input.ConcurrencyStamp);

        (await _userManager.SetUserNameAsync(user, input.UserName)).CheckErrors();

        await UpdateUserByInput(user, input);
        input.MapExtraPropertiesTo(user);

        (await _userManager.UpdateAsync(user)).CheckErrors();

        if (!input.Password.IsNullOrEmpty())
        {
            (await _userManager.RemovePasswordAsync(user)).CheckErrors();
            (await _userManager.AddPasswordAsync(user, input.Password)).CheckErrors();
        }

        if (input.Profile != null)
        {
            var profile = ObjectMapper.Map<ProfileDto, UserProfile>(input.Profile);

            var oldProfile = await _profileRepository.FindAsync(input.Profile.Id);

            if (oldProfile == null)
            {
                await _profileRepository.InsertAsync(profile);
            }
            else
            {
                await _profileRepository.UpdateAsync(profile);
            }
        }

        await CurrentUnitOfWork.SaveChangesAsync();

        return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
    }

    //[Authorize(IdentityPermissions.Users.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        if (CurrentUser.Id == id)
        {
            throw new UserFriendlyException("Không thể tự xóa bản thân!");
        }

        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            return;
        }

        (await _userManager.DeleteAsync(user)).CheckErrors();
    }

    //[Authorize(IdentityPermissions.Users.Update)]
    public virtual async Task UpdateRolesAsync(Guid id, IdentityUserUpdateRolesDto input)
    {
        var user = await _userManager.GetByIdAsync(id);
        await _userManager.SetRolesAsync(user, input.RoleNames);
        await _userRepository.UpdateAsync(user);
    }

    //[Authorize(IdentityPermissions.Users.Default)]
    public async Task<IdentityUserDto> FindByUsernameAsync(string userName)
    {
        return ObjectMapper.Map<IdentityUser, IdentityUserDto>(
            await _userManager.FindByNameAsync(userName)
        );
    }

    //[Authorize(IdentityPermissions.Users.Default)]
    public async Task<IdentityUserDto> FindByEmailAsync(string email)
    {
        return ObjectMapper.Map<IdentityUser, IdentityUserDto>(
            await _userManager.FindByEmailAsync(email)
        );
    }

    protected virtual async Task UpdateUserByInput(IdentityUser user, IdentityUserCreateOrUpdateDtoBase input)
    {
        if (!string.Equals(user.Email, input.Email, StringComparison.InvariantCultureIgnoreCase))
        {
            (await _userManager.SetEmailAsync(user, input.Email)).CheckErrors();
        }

        if (!string.Equals(user.PhoneNumber, input.PhoneNumber, StringComparison.InvariantCultureIgnoreCase))
        {
            (await _userManager.SetPhoneNumberAsync(user, input.PhoneNumber)).CheckErrors();
        }

        (await _userManager.SetLockoutEnabledAsync(user, input.LockoutEnabled)).CheckErrors();

        user.Name = input.Name;
        user.Surname = input.Surname;
        (await _userManager.UpdateAsync(user)).CheckErrors();
        user.SetIsActive(input.IsActive);
        if (input.RoleNames != null)
        {
            (await _userManager.SetRolesAsync(user, input.RoleNames)).CheckErrors();
        }
    }
}