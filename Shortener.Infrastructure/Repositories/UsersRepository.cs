using Microsoft.AspNetCore.Identity;
using Shortener.Core.IdentityEntities;
using Shortener.Core.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shortener.Infrastructure.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public UsersRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<ApplicationUser?> FindUserByEmail(string email)
    {
        return await _userManager.FindByNameAsync(email);
    }

    public async Task<bool> IsUserHasRole(string email, string roleName)
    {
        var user = await FindUserByEmail(email);
        if (user == null)
            return false;

        return await _userManager.IsInRoleAsync(user, roleName);
    }

    public async Task<bool> SaveUser(ApplicationUser applicationUser, string password, string roleName)
    {
        var result = await _userManager.CreateAsync(applicationUser, password);
        await _userManager.AddToRoleAsync(applicationUser, roleName);
        return result.Succeeded;
    }

    public async Task<bool> SignIn(string email, string password)
    {
        var result = await _signInManager.PasswordSignInAsync(email, password, isPersistent: false, lockoutOnFailure: false);
        return result.Succeeded;
    }

    public async Task SignIn(ApplicationUser user)
    {
        await _signInManager.SignInAsync(user, false);
    }

    public async Task SignOut()
    {
        await _signInManager.SignOutAsync();
    }
}
