using System;
using System.Collections.Generic;
using System.Text;

namespace ForumSystem.Web.Infrastructure.Extensions
{
    using System.Threading.Tasks;
    using Common;
    using Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public static class UserManagerExtension
    {
        public static async Task<List<TUser>> GetUsersAsync<TUser>(this UserManager<TUser> manager)
            where TUser : class
        {
            return await manager.Users.ToListAsync();
        }

        public static async Task<bool> IsUserAdmin<TUser>(this UserManager<TUser> manager, TUser user)
            where TUser : class
        {
            if (user == null)
            {
                return false;
            }

            var userRoles = await manager.GetRolesAsync(user);
            return userRoles.Contains(GlobalConstants.AdministratorRoleName);
        }
    }
}
