using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Shortener_DEMO_.Controllers;

public class ExceptionHandlerController : Controller
{
    [Route("/[action]")]
    public IActionResult Error()
    {
        IExceptionHandlerPathFeature? exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        if (exceptionHandlerPathFeature != null &&
            exceptionHandlerPathFeature.Error != null)
        {
            ViewBag.ErrorMessage = exceptionHandlerPathFeature.Error.Message;
        }
        return View();
    }
}
