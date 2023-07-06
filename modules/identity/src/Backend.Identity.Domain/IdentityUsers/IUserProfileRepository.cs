using System;
using Volo.Abp.Domain.Repositories;

namespace Backend.Identity.IdentityUsers;

public interface IUserProfileRepository : IBasicRepository<UserProfile, Guid>
{
    
}