namespace KCR;

using KCR.Data; // Asegúrate de que este sea el namespace de tu ApplicationUser
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

    public static class IdentitySeeder
    {
        // Define los roles que necesitarás
        public static readonly string AdminRole = "Administrador";
        public static readonly string EmployeeRole = "Empleado";

        public static async Task SeedRolesAndAdminAsync(
            IServiceProvider serviceProvider,
            string adminPassword)
        {
            // 1. Obtener los servicios necesarios
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // 2. Crear los roles si no existen
            await CreateRoleIfNotExists(roleManager, AdminRole);
            await CreateRoleIfNotExists(roleManager, EmployeeRole);

            // 3. Crear el usuario Administrador principal
            await CreateAdminUser(userManager, AdminRole, adminPassword);
        }

        // Método auxiliar para crear roles
        private static async Task CreateRoleIfNotExists(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // Método auxiliar para crear el usuario Administrador
        private static async Task CreateAdminUser(UserManager<ApplicationUser> userManager, string adminRole, string adminPassword)
        {
            const string adminEmail = "admin@kcr.com"; // Email único para el Administrador

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true, // Confirmar automáticamente el email
                    // Puedes añadir más propiedades aquí (ej. Nombre = "Admin Principal")
                    Nombre = "Administrador Principal", // Asignar un valor
                    Cedula = "000-0000000-0",           // Asignar un valor o placeholder
                    Cargo = "Administrador General"     // Asignar un valor
                };

                // Crear el usuario con la contraseña
                var result = await userManager.CreateAsync(adminUser, adminPassword);

                if (result.Succeeded)
                {
                    // Asignar el rol de Administrador
                    await userManager.AddToRoleAsync(adminUser, adminRole);
                }
                // Manejar errores de creación si es necesario
            }
        }
    }
