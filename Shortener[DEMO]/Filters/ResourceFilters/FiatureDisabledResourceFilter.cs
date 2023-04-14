using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Shortener_DEMO_.Filters.ResourceFilters;

public class FiatureDisabledResourceFilter : IAsyncResourceFilter
{
    private readonly ILogger<FiatureDisabledResourceFilter> _logger;
    private readonly bool _isDisabled;

    public FiatureDisabledResourceFilter(ILogger<FiatureDisabledResourceFilter> logger, bool isDisabled)
    {
        _logger = logger;
        _isDisabled = isDisabled;
    }

    public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
    {
        // before logic
        _logger.LogInformation("{FilterName}.{MethodName} before logic executing",
            nameof(FiatureDisabledResourceFilter), nameof(OnResourceExecutionAsync));
        if (_isDisabled)
            context.Result = new StatusCodeResult(501); // 501 - Not implemented
        else
            await next();
        // after logic
        _logger.LogInformation("{FilterName}.{MethodName} after logic executing",
            nameof(FiatureDisabledResourceFilter), nameof(OnResourceExecutionAsync));
    }
}