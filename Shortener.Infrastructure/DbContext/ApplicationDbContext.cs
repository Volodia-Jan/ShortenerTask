using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shortener.Core.Entities;
using Shortener.Core.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Shortener.Infrastructure.DbContext;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{

    public ApplicationDbContext(DbContextOptions options) : base(options) { }

    public DbSet<Url> URLs { get; set; }
    public DbSet<Post> Posts { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        var roles = GetRolesData("roles.json");
        var user = GetUserData("admin.json");

        builder.Entity<Url>().ToTable("Urls");
        builder.Entity<Post>().ToTable("Posts");

        builder.Entity<ApplicationUser>().HasData(user);
        builder.Entity<ApplicationRole>().HasData(roles);
        var adminRole = roles.SingleOrDefault(role => role.Name == "Admin");
        var usersRole = new IdentityUserRole<Guid>() { UserId = user.Id, RoleId = adminRole.Id };
        builder.Entity<IdentityUserRole<Guid>>().HasData(usersRole);
    }

    private ApplicationUser GetUserData(string fileName)
    {
        string usersAsJson = File.ReadAllText(fileName);
        var user = JsonSerializer.Deserialize<ApplicationUser>(usersAsJson);
        var passwordHasher = new PasswordHasher<ApplicationUser>();
        user.NormalizedUserName = user.UserName.ToUpper();
        user.NormalizedEmail = user.Email.ToUpper();
        user.SecurityStamp = Guid.NewGuid().ToString("D");
        user.PasswordHash = passwordHasher.HashPassword(user, user.PasswordHash);

        return user;
    }

    private List<ApplicationRole> GetRolesData(string fileName)
    {
        string usersAsJson = File.ReadAllText(fileName);
        var roles = JsonSerializer.Deserialize<List<ApplicationRole>>(usersAsJson);
        foreach (var role in roles)
            role.NormalizedName = role.Name.ToUpper();

        return roles;
    }
}
