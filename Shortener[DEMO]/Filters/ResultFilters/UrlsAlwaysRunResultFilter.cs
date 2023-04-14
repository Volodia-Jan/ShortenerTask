using Microsoft.AspNetCore.Mvc.Filters;

namespace Shortener_DEMO_.Filters.ResultFilters;

public class UrlsAlwaysRunResultFilter : IAlwaysRunResultFilter
{
    private readonly ILogger<UrlsAlwaysRunResultFilter> _logger;

    public UrlsAlwaysRunResultFilter(ILogger<UrlsAlwaysRunResultFilter> logger)
    {
        _logger = logger;
    }

    public void OnResultExecuted(ResultExecutedContext context)
    {
        _logger.LogInformation("{FilterName}.{MethodName} method is executing",
            nameof(UrlsAlwaysRunResultFilter), nameof(OnResultExecuted));
    }

    public void OnResultExecuting(ResultExecutingContext context)
    {
        // If it find SkipFilter it will skip this filter
        if (context.Filters.OfType<SkipFilter>().Any())
            return;

        _logger.LogInformation("{FilterName}.{MethodName} method is executing",
            nameof(UrlsAlwaysRunResultFilter), nameof(OnResultExecuting));
    }
}
