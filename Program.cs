using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.MVC;
using Project.MVC.Areas.Data;
using Project.MVC.Data;
using Project.MVC.Repositories;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
        builder.Services.AddControllersWithViews();
        builder.Services.AddTransient<IHomeRepository, HomeRepository>();
        builder.Services.AddTransient<ICartRepository, CartRepository>();
        builder.Services.AddTransient<IUserOrderRepository, UserOrderRepository>();
        builder.Services.AddTransient<IActiveOrdersRepository, ActiveOrdersRepository>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();

        using (var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var roles = new[] { "Admin", "Member" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
        using (var scope = app.Services.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            string email = "admin@admin.com";
            string password = "AdminAdmin1!";
            if (await userManager.FindByEmailAsync(email) == null)
            {
                var user = new AppUser();
                user.UserName = email;
                user.Email = email;
                user.EmailConfirmed = true;
                user.Address = "Rajko Zinzifov";
                user.City = "Skopje";
                user.FirstName = "Admin";
                user.LastName = "Admin";
                await userManager.CreateAsync(user, password);
                await userManager.AddToRoleAsync(user, "Admin");

            }
        }

        app.Run();
    }
}