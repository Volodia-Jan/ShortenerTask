using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shortener.Core.DTO;
using Shortener.Core.IdentityEntities;
using Shortener.Core.ServiceContracts;

namespace Shortener_DEMO_.Controllers;

[Route("/auth/[action]")]
[AllowAnonymous]
public class AuthorithationController : Controller
{

    private readonly IUsersService _usersService;

    public AuthorithationController(IUsersService usersService)
    {
        _usersService = usersService;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        if (!ModelState.IsValid)
        {
            List<string> errors = ModelState.Values
                .SelectMany(error => error.Errors)
                .Select(error => error.ErrorMessage)
                .ToList();
            ViewBag.ErrorMessage = string.Join("\n", errors);
            return View();
        }

        var isRegistered = await _usersService.Register(registerDto);
        ViewBag.ErrorMessage = "Something goes wrong!";
        return isRegistered ? RedirectToAction(nameof(UrlsController.Index), "Urls") : View();
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginDTO loginDTO)
    {
        if (!ModelState.IsValid)
        {
            List<string> errors = ModelState.Values
                .SelectMany(error => error.Errors)
                .Select(error => error.ErrorMessage)
                .ToList();
            ViewBag.ErrorMessage = string.Join("\n", errors);
            return View(loginDTO);
        }

        var isLogedIn = await _usersService.Login(loginDTO);
        ViewBag.ErrorMessage = "Invalid username or/and password!";
        return isLogedIn ? RedirectToAction(nameof(UrlsController.Index), "Urls") : View();
    }

    public async Task<IActionResult> Logout()
    {
        await _usersService.SignOut();
        return RedirectToAction(nameof(UrlsController.Index), "Urls");
    }
}
