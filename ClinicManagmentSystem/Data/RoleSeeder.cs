using Microsoft.AspNetCore.Identity;
using ClinicManagementSystem.Constants;

namespace ClinicManagementSystem.Data
{
    public static class RoleSeeder
    {
        public static async Task SeedRolesAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var roles = new[] { Roles.Admin, Roles.Doctor, Roles.Patient };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}

