using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Shortener.Core.DTO;
using Shortener.Core.Entities;
using Shortener.Core.Helper;
using Shortener.Core.Helpers;
using Shortener.Core.IdentityEntities;
using Shortener.Core.RepositoryContracts;
using Shortener.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Shortener.Core.Services;

public class UrlsService : IUrlsService
{
    private readonly IUrlsRepository _urlsRepository;
    private readonly IUsersService _usersService;
    private readonly IHttpContextAccessor _httpContext;

    public UrlsService(IUrlsRepository urlsRepository, IHttpContextAccessor httpContextAccessor, IUsersService usersService)
    {
        _urlsRepository = urlsRepository;
        _httpContext = httpContextAccessor;
        _usersService = usersService;
    }

    public Task<UrlResponse> AddNewUrl(string? detinationUrl, string requestPath)
    {
        if (string.IsNullOrEmpty(detinationUrl))
            throw new ArgumentNullException(nameof(detinationUrl));

        Uri uriResult;
        if (!Uri.TryCreate(detinationUrl, UriKind.Absolute, out uriResult)
            && !(uriResult?.Scheme == Uri.UriSchemeHttp || uriResult?.Scheme == Uri.UriSchemeHttps))
            throw new ArgumentException($"{detinationUrl} is not valid URL");

        Url url = new(){ BaseUrl = detinationUrl };

        return AddNewUrlAsync(url);

        async Task<UrlResponse> AddNewUrlAsync(Url urlToAdd)
        {
            if (urlToAdd.BaseUrl is not null &&
                await _urlsRepository.FindByBaseUrl(urlToAdd.BaseUrl) is not null)
                throw new ArgumentException($"{urlToAdd.BaseUrl} is already exist");

            var user = await GetCurrentUser();

            urlToAdd.Id = Guid.NewGuid();
            urlToAdd.UserId = user.Id;
            urlToAdd.ShortUrl = requestPath + "/" + ShortUrlHelper.Encode(urlToAdd.Id.GetHashCode());
            urlToAdd.CreatedDate = DateTime.Now;

            await _urlsRepository.Save(url);
            return urlToAdd.ToResponse();
        }
    }

    public Task<bool> DeleteUrlById(Guid? urlId)
    {
        if (urlId is null)
            throw new ArgumentNullException(nameof(urlId));

        return DeleteUrlByIdAsync(urlId.Value);

        async Task<bool> DeleteUrlByIdAsync(Guid id)
        {
            var user = await GetCurrentUser();
            var isAdmin = await _usersService.IsUserHasRole(user?.Email ?? string.Empty, "Admin");
            var url = (await _urlsRepository.FindById(id))?.ToResponse();
            if (url is null)
                throw new ArgumentException($"Url was not found by id:{id}");

            if (isAdmin || url.Username == user.Email)
                return await _urlsRepository.DeleteById(id);

            return false;
        }
    }

    public async Task<List<UrlResponse>> GetAllUrls()
    {
        var urls = await _urlsRepository.FindAll();

        return urls
            .Select(url => url.ToResponse())
            .ToList();
    }

    public async Task<UrlResponse> GetUrlById(Guid? urlId)
    {
        if (urlId is null)
            throw new ArgumentNullException(nameof(urlId));

        var url = await _urlsRepository.FindById(urlId.Value);
        return url is null
            ? throw new ArgumentException($"Url was not found by Id:{urlId.Value}")
            : url.ToResponse();
    }

    public async Task<UrlResponse> GetBaseUrlByShortUrl(string shortUrl)
    {
        Url? url = await _urlsRepository.FindByShortUrl(shortUrl);

        return url?.ToResponse() ?? throw new ArgumentException($"Url was not found by shortUlr:{shortUrl}");
    }

    private async Task<UserResponse> GetCurrentUser()
    {
        string? userName = _httpContext.HttpContext.User.Identity?.Name;
        if (string.IsNullOrEmpty(userName))
            throw new ArgumentException($"{nameof(userName)} is null or empty");

        var user = await _usersService.GetUserByEmail(userName);
        if (user is null)
            throw new ArgumentException($"{nameof(user)} is null");

        return user;
    }
}
