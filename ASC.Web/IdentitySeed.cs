using ASC.Model.BaseEntity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using ASC.Web.Configuration;

namespace ASC.Web.Data
{
    public class IdentitySeed : IIdentitySeed
    {
        public async Task Seed(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<ApplicationSettings> options)
        {
            // 1. Tạo roles
            var roles = options.Value.Roles.Split(',');

            foreach (var role in roles)
            {
                try
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        var identityRole = new IdentityRole
                        {
                            Name = role
                        };

                        await roleManager.CreateAsync(identityRole);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            // 2. Tạo Admin
            var admin = await userManager.FindByEmailAsync(options.Value.AdminEmail);

            if (admin == null)
            {
                var user = new IdentityUser
                {
                    UserName = options.Value.AdminName,
                    Email = options.Value.AdminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, options.Value.AdminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddClaimAsync(user,
                        new Claim(ClaimTypes.Email, options.Value.AdminEmail));

                    await userManager.AddClaimAsync(user,
                        new Claim("IsActive", "True"));

                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }

            // 3. Tạo Engineer
            var engineer = await userManager.FindByEmailAsync(options.Value.EngineerEmail);

            if (engineer == null)
            {
                var user = new IdentityUser
                {
                    UserName = options.Value.EngineerName,
                    Email = options.Value.EngineerEmail,
                    EmailConfirmed = true,
                    LockoutEnabled = false
                };

                var result = await userManager.CreateAsync(user, options.Value.EngineerPassword);

                if (result.Succeeded)
                {
                    await userManager.AddClaimAsync(user,
                        new Claim(ClaimTypes.Email, options.Value.EngineerEmail));

                    await userManager.AddClaimAsync(user,
                        new Claim("IsActive", "True"));

                    await userManager.AddToRoleAsync(user, "Engineer");
                }
            }
        }
    }
}