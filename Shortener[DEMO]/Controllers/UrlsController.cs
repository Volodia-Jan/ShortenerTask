﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Shortener.Core.DTO;
using Shortener.Core.ServiceContracts;
using Shortener_DEMO_.Filters;
using Shortener_DEMO_.Filters.ResultFilters;

namespace Shortener_DEMO_.Controllers;

// This filter applies for every method in controller
// and for example I wanna skip it in some method
[TypeFilter(typeof(UrlsAlwaysRunResultFilter))]
[Route("[controller]/[action]")]
public class UrlsController : Controller
{
    private readonly IUrlsService _urlsService;
    private readonly IPostsService _postsService;

    public UrlsController(IUrlsService urlsService, IPostsService postsService)
    {
        _urlsService = urlsService;
        _postsService = postsService;
    }

    [Route("/")]
    //[AllowAnonymous]
    [Authorize]
    [SkipFilter]
    public async Task<IActionResult> Index()
    {
        var urlResponses = await _urlsService.GetAllUrls();

        return View(urlResponses);
    }

    [Authorize]
    [TypeFilter(typeof(UrlsListResultFilter))]
    public async Task<IActionResult> Create(string? url)
    {
        if (User.Identity is null || !User.Identity.IsAuthenticated)
            return Unauthorized();

        string requestPath = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
        await _urlsService.AddNewUrl(url, requestPath);

        return Redirect("/");
    }

    [Authorize]
    public async Task<IActionResult> RedirectByShortUrl(string shortUrl)
    {
        var urlResponse = await _urlsService.GetBaseUrlByShortUrl(shortUrl);

        return Redirect(urlResponse?.Url ?? "/");
    }

    [Authorize]
    public async Task<IActionResult> Info(Guid urlId)
    {
        var urlResponse = await _urlsService.GetUrlById(Guid.NewGuid());

        return View(urlResponse);
    }

    [Authorize]
    public async Task<IActionResult> Delete(Guid urlId)
    {
        await _urlsService.DeleteUrlById(urlId);

        return Redirect("/");
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> About()
    {
        var posts = await _postsService.GetAllPosts();
        return View(posts);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> About(AddPostRequest addPostRequest)
    {
        addPostRequest.AuthorName = User.Identity?.Name;
        await _postsService.AddNewPost(addPostRequest);
        return Redirect("/Urls/About");
    }
}