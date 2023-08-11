using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VacationsManagement.Data;
using VacationsManagement.Data.Models;

namespace VacationsManagement.Infrastructure
{
    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder PrepareDatabase(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var services = serviceScope.ServiceProvider;
            var data = services.GetRequiredService<VacationManagementDbContext>();
            var userManager = services.GetRequiredService<UserManager<Employee>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            MigrateDatabase(data);
            SeedManagerRole(roleManager);

            if(!data.Employees.Any())
            {
                SeedUsersAsync(userManager, roleManager);
            }

            if(!data.VacationStatuses.Any())
            {
                SeedVacationStatuses(data);
            }

            return app;
        }

        private static void MigrateDatabase(VacationManagementDbContext data)
        {
            data.Database.Migrate();
        }

        private static void SeedVacationStatuses(VacationManagementDbContext data)
        {
            data.VacationStatuses.AddRange(new[]
            {
                new VacationStatus {Status = "Pending"},
                new VacationStatus {Status = "Approved"},
                new VacationStatus {Status = "Rejected"},
            });

            data.SaveChanges();
        }

        private static void SeedUsersAsync(UserManager<Employee> userManager, RoleManager<IdentityRole> roleManager)
        {
            Task.Run(async () =>
            {
                var manager = new Employee
                {
                    UserName = "manager@vacationsManagement.com",
                    Email = "manager@vacationsManagement.com",
                    FirstName = "Manager",
                    LastName = "Manager",
                    VacationDays = 20,
                };

                await userManager.CreateAsync(manager, "Parola123");

                if (await roleManager.RoleExistsAsync(WebConstants.managerRoleName))
                {
                    await userManager.AddToRoleAsync(manager, WebConstants.managerRoleName);           
                }

                var employee = new Employee
                {
                    UserName = "employee@vacationsManagement.com",
                    Email = "employee@vacationsManagement.com",
                    FirstName = "Employee",
                    LastName = "Employee",
                    VacationDays = 20,
                    ManagerId = manager.Id
                };

                await userManager.CreateAsync(employee, "Parola123");
            })
                .GetAwaiter()
                .GetResult();
        }

        private static void SeedManagerRole(RoleManager<IdentityRole> roleManager)
        {
            Task.Run(async () =>
            {
                if (await roleManager.RoleExistsAsync(WebConstants.managerRoleName))
                {
                    return;
                }
                var role = new IdentityRole { Name = WebConstants.managerRoleName };

                await roleManager.CreateAsync(role);
            })
                .GetAwaiter()
                .GetResult();
        }
    }
}
