using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Backend.Identity.Identity;
using Backend.Identity.Localization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Services;
using Volo.Abp.Threading;

namespace Backend.Identity.IdentityRoles;

public class IdentityRoleManager : RoleManager<IdentityRole>, IDomainService
{
    protected override CancellationToken CancellationToken => CancellationTokenProvider.Token;

    protected IStringLocalizer<IdentityResource> Localizer { get; }
    protected ICancellationTokenProvider CancellationTokenProvider { get; }

    public IdentityRoleManager(
        IdentityRoleStore store,
        IEnumerable<IRoleValidator<IdentityRole>> roleValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        ILogger<IdentityRoleManager> logger,
        IStringLocalizer<IdentityResource> localizer,
        ICancellationTokenProvider cancellationTokenProvider)
        : base(
            store,
            roleValidators,
            keyNormalizer,
            errors,
            logger)
    {
        Localizer = localizer;
        CancellationTokenProvider = cancellationTokenProvider;
    }

    public virtual async Task<IdentityRole> GetByIdAsync(Guid id)
    {
        var role = await Store.FindByIdAsync(id.ToString(), CancellationToken);
        if (role == null)
        {
            throw new EntityNotFoundException(typeof(IdentityRole), id);
        }

        return role;
    }

    public override async Task<IdentityResult> SetRoleNameAsync(IdentityRole role, string name)
    {
        if (role.IsStatic && role.Name != name)
        {
            throw new UserFriendlyException("Role tĩnh không thể đổi tên");
        }

        return await base.SetRoleNameAsync(role, name);
    }

    public override async Task<IdentityResult> DeleteAsync(IdentityRole role)
    {
        if (role.IsStatic)
        {
            throw new UserFriendlyException("Role tĩnh không thể đổi xóa");
        }

        return await base.DeleteAsync(role);
    }
}