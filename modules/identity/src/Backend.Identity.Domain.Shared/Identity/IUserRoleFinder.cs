using System;
using System.Threading.Tasks;

namespace Backend.Identity.Identity;

public interface IUserRoleFinder
{
    Task<string[]> GetRolesAsync(Guid userId);
}