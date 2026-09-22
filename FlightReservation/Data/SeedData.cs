using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using FlightReservation.Models;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FlightReservation.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            string roleName = "Admin";
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            string adminEmail = "admin@flight.com";
            string adminPassword = "AdminPassword123!";

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                IdentityResult result = await userManager.CreateAsync(user, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, roleName);
                    
                    context.UserDetails.Add(new UserDetail
                    {
                        FirstName = "System",
                        LastName = "Admin",
                        EmailID = adminEmail,
                        Gender = "Other",
                        DOB = DateTime.Now.Date,
                        Age = 30,
                        PhoneNo = 123456789,
                        Address = "Admin Headquarters",
                        Wallet = 0
                    });
                    
                    await context.SaveChangesAsync();
                }
            }

            string userEmail = "passenger@flight.com";
            string userPassword = "UserPassword123!";

            if (await userManager.FindByEmailAsync(userEmail) == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = userEmail,
                    Email = userEmail,
                    EmailConfirmed = true
                };

                IdentityResult result = await userManager.CreateAsync(user, userPassword);

                if (result.Succeeded)
                {
                    context.UserDetails.Add(new UserDetail
                    {
                        FirstName = "Alex",
                        LastName = "Smith",
                        EmailID = userEmail,
                        Gender = "Male",
                        DOB = new DateTime(1995, 5, 15),
                        Age = 31,
                        PhoneNo = 123456789,
                        Address = "221B Baker Street, London",
                        Wallet = 25000
                    });

                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
