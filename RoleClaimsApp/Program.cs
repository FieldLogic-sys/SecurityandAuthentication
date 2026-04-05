using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RoleClaimsApp.Data; // Ensure this matches your folder structure
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// --- 1. SERVICE REGISTRATION (The Toolbox) ---
builder.Services.AddControllers();
builder.Services.AddOpenApi(); // .NET 9 OpenAPI support

// In-memory DB for Identity
builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseInMemoryDatabase("RoleClaimsAppDB"));

// Identity Services: Registering both User and Role managers
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Add Authorization Policies (Claims-Based)
builder.Services.AddAuthorization(options => 
{
    // High-Level Requirement: User must have a 'Department' claim with value 'IT'
    options.AddPolicy("RequireITDepartment", policy => policy.RequireClaim("Department", "IT"));
});

var app = builder.Build();

// --- 2. DATABASE SEEDING (The Commissioning) ---
// We create a scope to access the scoped Identity services before the app starts
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedIdentityAsync(services);
}

// --- 3. MIDDLEWARE PIPELINE (The Order of Operations) ---
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// CRITICAL: Routing must come before Authentication
app.UseRouting(); 

// CRITICAL: Check the "ID Badge" before checking "Permissions"
app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

app.Run();

// --- 4. SEED METHOD (The Root User Logic) ---
static async Task SeedIdentityAsync(IServiceProvider services)
{
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

    // Create Roles if they don't exist
    string[] roleNames = { "Admin", "User" };
    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    // Create the Global Elite Admin User
    var adminEmail = "tony@globalelite.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    if (adminUser == null)
    {
        adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail };
        var result = await userManager.CreateAsync(adminUser, "P@ssword123!");

        if (result.Succeeded)
        {
            // Assign Role (RBAC)
            await userManager.AddToRoleAsync(adminUser, "Admin");

            // Assign Claim (ABAC/Claims-Based)
            await userManager.AddClaimAsync(adminUser, new Claim("Department", "IT"));
        }
    }
}