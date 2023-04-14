using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shortener.Core.DTO;
using Shortener.Core.IdentityEntities;
using Shortener.Core.ServiceContracts;
using Shortener_DEMO_.Filters.ActionFilters;

namespace Shortener_DEMO_.Controllers;

[Route("/Auth/[action]")]
[AllowAnonymous]
public class AuthorithationController : Controller
{

    private readonly IUsersService _usersService;
    private readonly ILogger<AuthorithationController> _logger;

    public AuthorithationController(IUsersService usersService, ILogger<AuthorithationController> logger)
    {
        _usersService = usersService;
        _logger = logger;
    }

    [HttpGet]
    [TypeFilter(typeof(ResponseHeaderActionFilter), Arguments = new object[] { "X-Custom-Key", "Custom-Value" })]
    public IActionResult Register()
    {
        _logger.LogInformation($"{nameof(AuthorithationController.Register)} action method(parametreless) of {nameof(AuthorithationController)}");
        return View();
    }

    [HttpPost]
    [TypeFilter(typeof(RegisterAndLoginActionFilter))]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        _logger.LogInformation($"{nameof(AuthorithationController.Register)} action method with parameters of {nameof(AuthorithationController)}");
        _logger.LogDebug($"{registerDto}");
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
    [TypeFilter(typeof(RegisterAndLoginActionFilter))]
    public async Task<IActionResult> Login(LoginDTO loginDTO, string? ReturnUrl)
    {
        var isLogedIn = await _usersService.Login(loginDTO);
        ViewBag.ErrorMessage = "Invalid username or/and password!";

        return isLogedIn ? RedirectToAction(nameof(UrlsController.Index), "Urls") : View();
    }

    public async Task<IActionResult> Logout()
    {
        await _usersService.SignOut();

        return RedirectToAction(nameof(UrlsController.Index), "Urls");
    }

    public async Task<IActionResult> IsEmailAreadyExist(string email)
    {
        var userResponse = await _usersService.GetUserByEmail(email);

        return userResponse is null ? Json(true) : Json(false);
    }
}