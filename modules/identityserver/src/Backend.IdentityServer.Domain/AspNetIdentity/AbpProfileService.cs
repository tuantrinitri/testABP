﻿using System.Security.Principal;
using System.Threading.Tasks;
using Backend.Identity.IdentityUsers;
using IdentityServer4.AspNetIdentity;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;
using IdentityUser = Backend.Identity.IdentityUsers.IdentityUser;

namespace Backend.IdentityServer.AspNetIdentity;

public class AbpProfileService : ProfileService<IdentityUser>
{
    protected ICurrentTenant CurrentTenant { get; }

    public AbpProfileService(
        IdentityUserManager userManager,
        IUserClaimsPrincipalFactory<IdentityUser> claimsFactory,
        ICurrentTenant currentTenant)
        : base(userManager, claimsFactory)
    {
        CurrentTenant = currentTenant;
    }

    [UnitOfWork]
    public override async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        using (CurrentTenant.Change(context.Subject.FindTenantId()))
        {
            await base.GetProfileDataAsync(context);
        }
    }

    [UnitOfWork]
    public override async Task IsActiveAsync(IsActiveContext context)
    {
        using (CurrentTenant.Change(context.Subject.FindTenantId()))
        {
            await base.IsActiveAsync(context);
        }
    }

    [UnitOfWork]
    public override Task<bool> IsUserActiveAsync(IdentityUser user)
    {
        return Task.FromResult(user.IsActive);
    }
}
