using KCR.Components;
using KCR.Components.Account;
using KCR.Data;
using KCR.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
}); // ❌ Se eliminó .AddIdentityCookies() para evitar el error "Scheme already exists".

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// El método AddIdentity ahora registra todo, incluyendo roles (AddRoles) y cookies (AddSignInManager)
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddRoles<IdentityRole>() // ✅ Mantiene el soporte de Roles
    .AddSignInManager()
    .AddDefaultTokenProviders();
// -------------------------------------------------------------------------------------------------

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

// Registro de tus servicios de negocio (Ya estaban correctos)
builder.Services.AddScoped<TurnoService>();
builder.Services.AddScoped<EmpleadoService>();
builder.Services.AddScoped<PreFacturaService>();
builder.Services.AddScoped<ClienteService>();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var configuration = services.GetRequiredService<IConfiguration>();

    // Contraseña del Admin (obtenida de la configuración o fallback)
    var adminPassword = configuration["AdminSettings:DefaultPassword"] ?? "PasswordSeguro123!";

    // ✅ Llamada al Seeder (asumiendo que IdentitySeeder está en KCR.Services)
    await KCR.IdentitySeeder.SeedRolesAndAdminAsync(services, adminPassword);
}
// -------------------------------------------------------------------------------------------------

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

// Los middlewares de Autenticación y Autorización deben ir aquí
app.UseAuthentication();
app.UseAuthorization(); // Se recomienda añadir UseAuthentication/UseAuthorization explícitamente

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();