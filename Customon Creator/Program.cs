using Customon_Creator.Areas.Identity.Pages.Account;
using Customon_Creator.Models.Entities;
using Customon_Creator.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Change ApplicationUser to default user manager
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddUserManager<UserManager<ApplicationUser>>()
    .AddSignInManager<MySignInManager>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.Configure<IdentityOptions>(options => {
    options.User.RequireUniqueEmail = true;
});

// Allow postman to send requests
builder.Services.AddCors(options => {
    options.AddDefaultPolicy(
    builder => {
        builder.WithOrigins("https://web.postman.co")
    .AllowAnyHeader()
    .AllowAnyMethod();
    });
});

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IUserRepository, DbUserRepository>();
builder.Services.AddScoped<IPokemonRepository, DbPokemonRepository>();
builder.Services.AddScoped<IMoveRepository, DbMoveRepository>();
builder.Services.AddScoped<Initializer>();


var app = builder.Build();
await SeedDataAsync(app);

static async Task SeedDataAsync(WebApplication app) {
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    try {
        var initializer = services.GetRequiredService<Initializer>();
        await initializer.SeedDatabaseAsync();
    }
    catch (Exception ex) {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError("An error occurred while seeding the database: {Message} ", ex.Message);
    }
}



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseMigrationsEndPoint();
}
else {
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
