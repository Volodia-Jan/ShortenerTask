using Microsoft.AspNetCore.Mvc.Filters;
using Shortener.Core.ServiceContracts;
using Shortener_DEMO_.Controllers;

namespace Shortener_DEMO_.Filters.ActionFilters;

public class RegisterAndLoginActionFilter : IAsyncActionFilter
{
    private readonly IUsersService _usersService;

    public RegisterAndLoginActionFilter(IUsersService usersService)
    {
        _usersService = usersService;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.Controller is AuthorithationController authController && !authController.ModelState.IsValid)
        {
            List<string> errors = authController.ModelState.Values
                .SelectMany(error => error.Errors)
                .Select(error => error.ErrorMessage)
                .ToList();
            authController.ViewBag.ErrorMessage = string.Join("\n", errors);

            // Short-cicuiting(коротка схема) of Action filter
            // or skiping the subsequent filters
            context.Result = authController.View();
            // in Result we can supply any type of IActionResult
            // and await next(); should not be invoked so you're skipping all next filters in chain
        }
        else
            await next();
    }
}