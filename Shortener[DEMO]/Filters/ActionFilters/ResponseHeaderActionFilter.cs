using Microsoft.AspNetCore.Mvc.Filters;

namespace Shortener_DEMO_.Filters.ActionFilters;

/// <summary>
/// It add some header to response.
/// Main goal of that filter is
/// showing how filters work with additional parameters
/// </summary>
public class ResponseHeaderActionFilter : IAsyncActionFilter, IOrderedFilter
{
    private readonly ILogger<ResponseHeaderActionFilter> _logger;
    public int Order { get; set; }
    private readonly string _key;
    private readonly string _value;

    public ResponseHeaderActionFilter(ILogger<ResponseHeaderActionFilter> logger, string key, string value, int order = 0)
    {
        _logger = logger;
        _key = key;
        _value = value;
        Order = order;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // before logic here
        _logger.LogInformation("{FilterName}.{MethodName} before logic executing",
            nameof(ResponseHeaderActionFilter), nameof(OnActionExecutionAsync));
        await next();
        // after logic here
        _logger.LogInformation("{FilterName}.{MethodName} after logic executing",
            nameof(ResponseHeaderActionFilter), nameof(OnActionExecutionAsync));
        context.HttpContext.Response.Headers[_key] = _value;
    }
}