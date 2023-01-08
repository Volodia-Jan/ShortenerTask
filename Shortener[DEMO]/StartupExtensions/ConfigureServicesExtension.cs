using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shortener.Core.IdentityEntities;
using Shortener.Core.RepositoryContracts;
using Shortener.Core.ServiceContracts;
using Shortener.Core.Services;
using Shortener.Infrastructure.DbContext;
using Shortener.Infrastructure.Repositories;

namespace Shortener_DEMO_.StartupExtensions;

public static class ConfigureServicesExtension
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllersWithViews();

        // Adding DbContext
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultString"));
        });

        // Adding services/repo-s to IoC container
        services.AddScoped<IPostsRepository, PostsRepository>();
        services.AddScoped<IUrlsService, UrlsService>();
        services.AddScoped<IUsersService, UsersService>();
        services.AddScoped<IPostsService, PostsService>();
        services.AddScoped<IUrlsRepository, UrlsRepository>();
        services.AddScoped<IUsersRepository, UsersRepository>();

        // Adding Authorization
        services.AddAuthorization(options =>
        {
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();
        });
        services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/auth/Login";
        });

        // Adding indentity
        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
        {
            options.Password.RequiredLength = 4;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireDigit = false;
        })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders()
            .AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>()
            .AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>();
        return services;
    }
}
