using Microsoft.AspNetCore.HttpLogging;
using Serilog;
using Shortener_DEMO_.Middleware;
using Shortener_DEMO_.StartupExtensions;

var builder = WebApplication.CreateBuilder(args);

// Srilog framewokr for logging (install NuGet packages Serilog & Serilog.AspNetCore)
// Adding Serilog as default Logger
builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services, LoggerConfiguration loggerConfig) =>
{
    // Serilog confuguring
    loggerConfig
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services);
});

// That is adding and configuring build-in Logger
builder.Logging.ClearProviders()
               .AddConsole()
               .AddDebug();

builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

/*if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
    app.UseExceptionHandlerMiddleware();*/

app.UseExceptionHandler("/Error");
app.UseExceptionHandlerMiddleware();

app.UseSerilogRequestLogging();
app.UseHttpLogging();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
