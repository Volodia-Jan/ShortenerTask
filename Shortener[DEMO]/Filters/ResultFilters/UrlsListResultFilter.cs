using Microsoft.AspNetCore.Mvc.Filters;

namespace Shortener_DEMO_.Filters.ResultFilters;

public class UrlsListResultFilter : IAsyncResultFilter
{
    private readonly ILogger<UrlsListResultFilter> _logger;

    public UrlsListResultFilter(ILogger<UrlsListResultFilter> logger)
    {
        _logger = logger;
    }

    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        _logger.LogInformation("{FilterName}.{MethodName} before logic is executing",
            nameof(UrlsListResultFilter), nameof(OnResultExecutionAsync));
        await next();
        _logger.LogInformation("{FilterName}.{MethodName} after logic is executing",
            nameof(UrlsListResultFilter), nameof(OnResultExecutionAsync));

        context.HttpContext.Response.Headers["Last-Modified"] =
            DateTime.Now.ToString("dd/MM/yyyy HH:mm");
    }
}