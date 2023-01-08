using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shortener.Core.IdentityEntities;
using Shortener.Core.RepositoryContracts;
using Shortener.Core.ServiceContracts;
using Shortener.Core.Services;
using Shortener.Infrastructure.DbContext;
using Shortener.Infrastructure.Repositories;
using Shortener_DEMO_.StartupExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
