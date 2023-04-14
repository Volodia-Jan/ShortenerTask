using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Shortener.Core.DTO;
using Shortener.Core.Entities;
using Shortener.Core.RepositoryContracts;
using Shortener.Core.ServiceContracts;
using Shortener.Core.Services;

namespace Shortener.ServicesTests;

public class UrlsServiceTest
{
    private readonly Mock<IUrlsRepository> _mockUrlsRepo;
    private readonly Mock<IUsersService> _mockUsersService;
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;

    private readonly IUrlsRepository _urlsRepository;
    private readonly IUsersService _usersService;
    private readonly IHttpContextAccessor _httpContext;
    private readonly IUrlsService _urlsService;

    private readonly IFixture _fixture;

    public UrlsServiceTest()
    {
        _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        _mockUrlsRepo = new Mock<IUrlsRepository>();
        _mockUsersService = new Mock<IUsersService>();

        _urlsRepository = _mockUrlsRepo.Object;
        _usersService = _mockUsersService.Object;
        _httpContext = _mockHttpContextAccessor.Object;

        _urlsService = new UrlsService(_urlsRepository, _httpContext, _usersService);

        _fixture = new Fixture();
    }


    #region AddNewUrl
    [Fact]
    public async Task AddNewUrl_NullArguments_ThrowArgumentNullException()
    {
        Func<Task> throwArgumentNullException = async () =>
            await _urlsService.AddNewUrl(null, string.Empty);

        await throwArgumentNullException.Should().ThrowAsync<ArgumentNullException>();

    }

    [Fact]
    public async Task AddNewUrl_InvalidUrl_ThrowArgumentException()
    {
        string invalidUrl = "invalidUrl";

        Func<Task> throwArgumentException = async () =>
            await _urlsService.AddNewUrl(invalidUrl, invalidUrl);

        await throwArgumentException.Should()
            .ThrowAsync<ArgumentException>()
            .WithMessage($"{invalidUrl} is not valid URL");
    }

    [Fact]
    public async Task AddNewUrl_ExistedLongUrl_ThrowArgumentException()
    {
        var url = _fixture.Create<Url>();
        var urlPath = "https://www.google.com.ua/";

        _mockUrlsRepo.Setup(repo => repo.FindByBaseUrl(It.IsAny<string>()))
            .ReturnsAsync(url);

        Func<Task> throwArgumentException = async () =>
            await _urlsService.AddNewUrl(urlPath, urlPath);

        await throwArgumentException.Should()
            .ThrowAsync<ArgumentException>()
            .WithMessage($"{urlPath} is already exist");
    }

    [Fact]
    public async Task AddNewUrl_ProperValue_ReturnValidUrlResponse()
    {
        var urlPath = "https://www.google.com.ua/";
        var userResponse = _fixture.Create<UserResponse>();
        var urlEntity = _fixture.Build<Url>()
            .With(url => url.BaseUrl, urlPath)
            .With(url => url.UserId, userResponse.Id)
            .Create();

        var expectedUrlResponse = urlEntity.ToResponse();

        _mockUrlsRepo.Setup(repo => repo.FindByBaseUrl(It.IsAny<string>()))
            .ReturnsAsync(null as Url);

        _mockUrlsRepo.Setup(repo => repo.Save(It.IsAny<Url>()))
            .ReturnsAsync(urlEntity);

        _mockHttpContextAccessor.Setup(context => context.HttpContext.User.Identity.Name)
            .Returns("UserName");

        _mockUsersService.Setup(service => service.GetUserByEmail(It.IsAny<string>()))
            .ReturnsAsync(userResponse);

        UrlResponse actualUrlResponse = await _urlsService.AddNewUrl(urlPath, urlPath);

        actualUrlResponse.UrlId.Should().NotBeNull();
        actualUrlResponse.UserId.Should().Be(expectedUrlResponse.UserId);
    }
    #endregion

    #region GetAllUrls
    [Fact]
    public async Task GetAllUrls_ReuturnEmptyList()
    {
        _mockUrlsRepo.Setup(repo => repo.FindAll())
            .ReturnsAsync(new List<Url>());

        var urlList = await _urlsService.GetAllUrls();

        urlList.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllUrls_ReuturnNonEmptyList()
    {
        var urlList = _fixture.CreateMany<Url>(3).ToList();
        var expectedUrlList = urlList
            .Select(url => url.ToResponse())
            .ToList();

        _mockUrlsRepo.Setup(repo => repo.FindAll())
            .ReturnsAsync(urlList);

        var actualUrlList = await _urlsService.GetAllUrls();

        actualUrlList.Should().NotBeEmpty();
        actualUrlList.Should().BeEquivalentTo(expectedUrlList);
    }
    #endregion


    #region GetUrlById
    [Fact]
    public async Task GetUrlById_NullId_ThrowArgumentNullException()
    {
        Func<Task> throwArgumentNullException = async ()
            => await _urlsService.GetUrlById(null);

        await throwArgumentNullException.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task GetUrlById_UrlNotFound_ThrowArgumentException()
    {
        Func<Task> throwArgumentException = async ()
            => await _urlsService.GetUrlById(Guid.NewGuid());

        await throwArgumentException.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task GetUrlById_ReturnUrl()
    {
        var url = _fixture.Create<Url>();
        var expectedUrlResponse = url.ToResponse();

        _mockUrlsRepo.Setup(repo => repo.FindById(It.IsAny<Guid>()))
            .ReturnsAsync(url);

        var actualUrlResponse = await _urlsService.GetUrlById(Guid.NewGuid());

        actualUrlResponse.Should().Be(expectedUrlResponse);
    }
    #endregion

    #region GetBaseUrlByShortUrl
    [Fact]
    public async Task GetBaseUrlByShortUrl_UrlNotFound_ThrowArgumentException()
    {
        Func<Task> throwArgumentException = async ()
            => await _urlsService.GetBaseUrlByShortUrl("someUrl");

        await throwArgumentException.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task GetBaseUrlByShortUrl_ReturnUrl()
    {
        var url = _fixture.Create<Url>();
        var expectedUrlResponse = url.ToResponse();

        _mockUrlsRepo.Setup(repo => repo.FindByShortUrl(It.IsAny<string>()))
            .ReturnsAsync(url);

        var actualUrlResponse = await _urlsService.GetBaseUrlByShortUrl(url.ShortUrl);

        actualUrlResponse.Should().Be(expectedUrlResponse);
    }
    #endregion

    #region DeleteUrlById
    [Fact]
    public async Task DeleteUrlById_NullId_ThrowArgumentNullException()
    {
        Func<Task> throwArgumentNullException = async ()
            => await _urlsService.DeleteUrlById(null);

        await throwArgumentNullException.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task DeleteUrlById_UrlNull_ThrowArgumentException()
    {
        var userResponse = _fixture.Create<UserResponse>();
        var id = Guid.NewGuid();

        _mockHttpContextAccessor.Setup(context => context.HttpContext.User.Identity.Name)
            .Returns("UserName");

        _mockUsersService.Setup(service => service.GetUserByEmail(It.IsAny<string>()))
            .ReturnsAsync(userResponse);

        _mockUsersService.Setup(service => service.IsUserHasRole(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(true);

        _mockUrlsRepo.Setup(repo => repo.FindById(It.IsAny<Guid>()))
            .ReturnsAsync(null as Url);

        Func<Task> throwArgumentException = async ()
            => await _urlsService.DeleteUrlById(id);

        await throwArgumentException.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task DeleteUrlById_NoPermissions_ReturnFalse()
    {
        var userResponse = _fixture.Create<UserResponse>();
        var url = _fixture.Create<Url>();
        var id = Guid.NewGuid();

        _mockHttpContextAccessor.Setup(context => context.HttpContext.User.Identity.Name)
            .Returns("UserName");

        _mockUsersService.Setup(service => service.GetUserByEmail(It.IsAny<string>()))
            .ReturnsAsync(userResponse);

        _mockUsersService.Setup(service => service.IsUserHasRole(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(false);

        _mockUrlsRepo.Setup(repo => repo.FindById(It.IsAny<Guid>()))
            .ReturnsAsync(url);

        var isDeleted = await _urlsService.DeleteUrlById(id);

        isDeleted.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteUrlById_ReturnTrue()
    {
        var userResponse = _fixture.Create<UserResponse>();
        var url = _fixture.Create<Url>();
        var id = Guid.NewGuid();

        var urlResponse = url.ToResponse();

        _mockHttpContextAccessor.Setup(context => context.HttpContext.User.Identity.Name)
            .Returns("UserName");

        _mockUsersService.Setup(service => service.GetUserByEmail(It.IsAny<string>()))
            .ReturnsAsync(userResponse);

        _mockUsersService.Setup(service => service.IsUserHasRole(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(true);

        _mockUrlsRepo.Setup(repo => repo.FindById(It.IsAny<Guid>()))
            .ReturnsAsync(url);

        _mockUrlsRepo.Setup(repo => repo.DeleteById(It.IsAny<Guid>()))
            .ReturnsAsync(true);

        var isDeleted = await _urlsService.DeleteUrlById(id);

        isDeleted.Should().BeTrue();
    }
    #endregion
}