using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
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
    private readonly ILogger<UsersRepository> _logger;

    public UsersRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<UsersRepository> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

    public async Task<ApplicationUser?> FindUserByEmail(string email)
    {
        _logger.LogInformation($"{nameof(UsersRepository.FindUserByEmail)} method is executing");
        _logger.LogDebug($"Parameters:{nameof(email)}:{email}");
        return await _userManager.FindByNameAsync(email);
    }

    public async Task<bool> IsUserHasRole(string email, string roleName)
    {
        _logger.LogInformation($"{nameof(UsersRepository.IsUserHasRole)} method is executing");
        _logger.LogDebug($"Parameters:{nameof(email)}:{email}, {nameof(roleName)}:{roleName}");
        var user = await FindUserByEmail(email);
        if (user == null)
            return false;

        return await _userManager.IsInRoleAsync(user, roleName);
    }

    public async Task<bool> SaveUser(ApplicationUser applicationUser, string password, string roleName)
    {
        _logger.LogInformation($"{nameof(UsersRepository.SaveUser)} method is executing");
        _logger.LogDebug($"Parameters:{nameof(applicationUser)}:{applicationUser}, {nameof(password)}:{password}, {nameof(roleName)}:{roleName}");
        var result = await _userManager.CreateAsync(applicationUser, password);
        await _userManager.AddToRoleAsync(applicationUser, roleName);
        return result.Succeeded;
    }

    public async Task<bool> SignIn(string email, string password)
    {
        _logger.LogInformation($"{nameof(UsersRepository.SignIn)} method is executing");
        _logger.LogDebug($"Parameters:{nameof(email)}:{email}, {nameof(password)}:{password}");
        var result = await _signInManager.PasswordSignInAsync(email, password, isPersistent: false, lockoutOnFailure: false);
        return result.Succeeded;
    }

    public async Task SignIn(ApplicationUser user)
    {
        _logger.LogInformation($"{nameof(UsersRepository.SignIn)} method is executing");
        _logger.LogDebug($"Parameters:{nameof(user)}:{user}");
        await _signInManager.SignInAsync(user, false);
    }

    public async Task SignOut()
    {
        _logger.LogInformation($"{nameof(UsersRepository.SignOut)} method is executing");
        await _signInManager.SignOutAsync();
    }
}