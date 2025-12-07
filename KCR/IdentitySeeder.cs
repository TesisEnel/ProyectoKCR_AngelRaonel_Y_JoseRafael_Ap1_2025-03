namespace KCR;

using KCR.Data;
using KCR.Models; // Necesario para acceder a la tabla Empleados
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore; // Necesario para consultas de BD
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

public static class IdentitySeeder
{
    // Roles
    public static readonly string AdminRole = "Administrador";
    public static readonly string EmpleadoExpressRole = "EmpleadoExpress";
    public static readonly string EmpleadoDERole = "EmpleadoDE";

    public static async Task SeedRolesAndAdminAsync(
        IServiceProvider serviceProvider,
        string adminPassword)
    {
        // 1. Obtener los servicios necesarios (Identity + Base de Datos de Negocio)
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var context = serviceProvider.GetRequiredService<ApplicationDbContext>(); // <--- ¡Vital!

        // 2. Crear los roles si no existen
        await CreateRoleIfNotExists(roleManager, AdminRole);
        await CreateRoleIfNotExists(roleManager, EmpleadoExpressRole);
        await CreateRoleIfNotExists(roleManager, EmpleadoDERole);

        // 3. Crear el usuario Administrador
        await CreateAdminUser(userManager, context, AdminRole, adminPassword);

        // 4. Crear los usuarios empleados específicos
        string passwordEmpleados = "PasswordSeguro123!"; // Contraseña común para pruebas

        // Usuario Empleado Express
        await CreateUserIfNotExists(
            userManager,
            context, // Pasamos el contexto
            "EmpleadoExpress@kcr.com",
            passwordEmpleados,
            EmpleadoExpressRole,
            "Empleado Express",
            "000-0000000-1",
            "Operador Express"
        );

        // Usuario Empleado DE
        await CreateUserIfNotExists(
            userManager,
            context, // Pasamos el contexto
            "EmpleadoDE@kcr.com",
            passwordEmpleados,
            EmpleadoDERole,
            "Empleado DE",
            "000-0000000-2",
            "Operador DE"
        );
    }

    private static async Task CreateRoleIfNotExists(RoleManager<IdentityRole> roleManager, string roleName)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    private static async Task CreateAdminUser(
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext context,
        string adminRole,
        string adminPassword)
    {
        const string adminEmail = "admin@kcr.com";

        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                Nombre = "Administrador Principal",
                Cedula = "000-0000000-0",
                Cargo = "Administrador General"
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, adminRole);
                // Sincronizar con tabla Empleados
                await SincronizarEmpleado(context, adminUser);
            }
        }
        else
        {
            // Si el usuario ya existe en Login pero se borró la BD de empleados, lo recreamos
            var existingUser = await userManager.FindByEmailAsync(adminEmail);
            await SincronizarEmpleado(context, existingUser);
        }
    }

    private static async Task CreateUserIfNotExists(
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext context,
        string email,
        string password,
        string role,
        string nombre,
        string cedula,
        string cargo)
    {
        if (await userManager.FindByEmailAsync(email) == null)
        {
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                Nombre = nombre,
                Cedula = cedula,
                Cargo = cargo
            };

            var result = await userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, role);
                // Sincronizar con tabla Empleados
                await SincronizarEmpleado(context, user);
            }
        }
        else
        {
            // Si el usuario ya existe en Login pero se borró la BD de empleados, lo recreamos
            var existingUser = await userManager.FindByEmailAsync(email);
            await SincronizarEmpleado(context, existingUser);
        }
    }

    // Método auxiliar para evitar repetir código de guardado en la tabla Empleados
    private static async Task SincronizarEmpleado(ApplicationDbContext context, ApplicationUser userIdentity)
    {
        // Verificamos si ya existe en la tabla de negocio
        var existeEnNegocio = await context.empleados.AnyAsync(e => e.Usuario == userIdentity.Email);

        if (!existeEnNegocio)
        {
            var nuevoEmpleado = new Empleados
            {
                Nombre = userIdentity.Nombre,
                Usuario = userIdentity.Email, // Clave para vincular Login con Datos
                Clave = "******", // No guardamos la real aquí, Identity la maneja
                Cedula = userIdentity.Cedula,
                Cargo = userIdentity.Cargo
            };

            context.empleados.Add(nuevoEmpleado);
            await context.SaveChangesAsync();
        }
    }
}
