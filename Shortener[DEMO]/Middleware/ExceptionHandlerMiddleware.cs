using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog;
using System.Threading.Tasks;

namespace Shortener_DEMO_.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;
        private readonly IDiagnosticContext _diagnosticContext;

        public ExceptionHandlerMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlerMiddleware> logger,
            IDiagnosticContext diagnosticContext)
        {
            _next = next;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                if (ex.InnerException is not null)
                {
                    _logger.LogError("{ExceptionType}\n{ExceptionMessage}",
                        ex.InnerException.GetType().ToString(),
                        ex.InnerException.Message);
                }
                else
                {
                    _logger.LogError("{ExceptionType}\n{ExceptionMessage}",
                        ex.GetType(), ex.Message);
                }

                //await httpContext.Response.WriteAsync("Error handled");
                throw;
            }
        }
    }

    public static class ExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
