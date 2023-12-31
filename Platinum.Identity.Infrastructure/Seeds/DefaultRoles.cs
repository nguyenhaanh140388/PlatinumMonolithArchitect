﻿using Microsoft.AspNetCore.Identity;
using Platinum.Core.Enums;
using Platinum.Identity.Core.Entities;

namespace Platinum.Identity.Infrastructure.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            await roleManager.CreateAsync(new ApplicationRole() { Name = Roles.SuperAdmin.ToString() });
            await roleManager.CreateAsync(new ApplicationRole() { Name = Roles.Admin.ToString() });
            await roleManager.CreateAsync(new ApplicationRole() { Name = Roles.Moderator.ToString() });
            await roleManager.CreateAsync(new ApplicationRole() { Name = Roles.User.ToString() });
        }
    }
}
