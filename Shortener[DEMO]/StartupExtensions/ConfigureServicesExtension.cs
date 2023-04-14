using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shortener.Core.IdentityEntities;
using Shortener.Core.RepositoryContracts;
using Shortener.Core.ServiceContracts;
using Shortener.Core.Services;
using Shortener.Infrastructure.DbContext;
using Shortener.Infrastructure.Repositories;
using Shortener_DEMO_.Filters.ActionFilters;
using static System.Net.WebRequestMethods;

namespace Shortener_DEMO_.StartupExtensions;

public static class ConfigureServicesExtension
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllersWithViews(options =>
        {
            // Returns needed service from Service collection
            var logger = services.BuildServiceProvider().GetRequiredService<ILogger<ResponseHeaderActionFilter>>();

            // Adding Global filter as generic type
            // options.Filters.Add<ResponseHeaderActionFilter>()
            // But that Filter has parameters so we have to use second way
            // options.Filters.Add(new ResponseHeaderActionFilter(logger, "My-Global-Key", "My-Global-Value"))

            // But now we can not set the order number for global filter
            // To solve this problem our Filter class have to implement IOrderedFilter
            options.Filters.Add(new ResponseHeaderActionFilter(logger, "My-Global-Key", "My-Global-Value", 1));

            // Therefore if filter doesn't have additonal parameters and we wanna
            // initialize order number we don't need to implement IOrderedFilter, just use next statments:
            // options.Filters.Add<ResponseHeaderActionFilter>(3)
        });

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
            options.LoginPath = "/Auth/Login";
        });

        // Adding indentity
        services
            .AddIdentity<ApplicationUser, ApplicationRole>(options =>
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

        // Http logging is helping us to log
        // Request and response info.
        // It good idea to use it
        // only in Debug reason
        services.AddHttpLogging(
            // Configure what we need to Log
            // By default it logs everything
            options =>
            {
                options.LoggingFields = HttpLoggingFields.RequestProperties | HttpLoggingFields.ResponsePropertiesAndHeaders;
            });

        return services;
    }
}