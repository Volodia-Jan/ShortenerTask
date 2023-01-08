using AutoFixture;
using FluentAssertions;
using Moq;
using Shortener.Core.DTO;
using Shortener.Core.IdentityEntities;
using Shortener.Core.RepositoryContracts;
using Shortener.Core.ServiceContracts;
using Shortener.Core.Services;

namespace Shortener.ServicesTests;

public class UsersServiceTest
{
    private readonly IUsersService _usersService;
    private readonly Mock<IUsersRepository> _mockUsersRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly IFixture _fixture;

    public UsersServiceTest()
    {
        _fixture = new Fixture();
        _mockUsersRepository = new Mock<IUsersRepository>();
        _usersRepository = _mockUsersRepository.Object;
        _usersService = new UsersService(_usersRepository);
    }

    #region RegisterUser

    [Fact]
    public async Task RegisterUser_ValidationError_ThrowArgumentException()
    {
        // Arrange
        var registerDto = _fixture.Create<RegisterDto>();

        // Act
        Func<Task> thowsArgumentException = async ()
            => await _usersService.Register(registerDto);

        // Assert
        await thowsArgumentException.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task RegisterUser_DuplicateEmail_ReturnFalse()
    {
        // Arrange 
        var registerDto = _fixture.Build<RegisterDto>()
            .With(dto => dto.Email, "testEmail@example.com")
            .With(dto => dto.Phone, "1234567890")
            .With(dto => dto.Password, "pass")
            .With(dto => dto.ConfirmPassword, "pass")
            .Create();

        var appUser = _fixture.Create<ApplicationUser>();

        _mockUsersRepository
            .Setup(repo => repo.FindUserByEmail(It.IsAny<string>()))
            .ReturnsAsync(appUser);

        // Act
        bool isRegistered = await _usersService.Register(registerDto);

        // Assert
        isRegistered.Should().BeFalse();
    }

    [Fact]
    public async Task RegisterUser_ProperData_ReturnTrue()
    {
        // Arrange 
        var registerDto = _fixture.Build<RegisterDto>()
            .With(dto => dto.Email, "testEmail@example.com")
            .With(dto => dto.Phone, "1234567890")
            .With(dto => dto.Password, "pass")
            .With(dto => dto.ConfirmPassword, "pass")
            .Create();

        _mockUsersRepository
            .Setup(repo => repo.FindUserByEmail(registerDto.Email))
            .ReturnsAsync(null as ApplicationUser);

        _mockUsersRepository
            .Setup(repo => repo.SaveUser(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(true);

        // Act
        bool isRegistered = await _usersService.Register(registerDto);

        // Assert
        isRegistered.Should().BeTrue();
    }
    #endregion

    #region Login
    [Fact]
    public async Task Login_ValidationError_ThrowArgumentException()
    {
        // Arrange
        var loginDto = _fixture.Create<LoginDTO>();

        // Act
        Func<Task> thowsArgumentException = async ()
            => await _usersService.Login(loginDto);

        // Assert
        await thowsArgumentException.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task Login_ReturnFalse()
    {
        // Arrange
        var loginDto = _fixture.Build<LoginDTO>()
            .With(login => login.Email, "testEmail@example.come")
            .Create();

        _mockUsersRepository
            .Setup(repo => repo.SignIn(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(false);

        // Act
        bool isHasRole = await _usersService.Login(loginDto);

        // Assert
        isHasRole.Should().BeFalse();
    }

    [Fact]
    public async Task Login_ReturnTrue()
    {
        // Arrange
        var loginDto = _fixture.Build<LoginDTO>()
            .With(login => login.Email, "testEmail@example.come")
            .Create();

        _mockUsersRepository
            .Setup(repo => repo.SignIn(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(true);

        // Act
        bool isHasRole = await _usersService.Login(loginDto);

        // Assert
        isHasRole.Should().BeTrue();
    }
    #endregion

    #region IsUserHasRole
    [Fact]
    public async Task IsUserHasRole_ArgumentsNull_ThrowArgumentNullException()
    {
        // Arrange

        // Act
        Func<Task> thowsArgumentNullException = async ()
            => await _usersService.IsUserHasRole(null, null);

        // Assert
        await thowsArgumentNullException.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task IsUserHasRole_ReturnFalse()
    {
        // Arrange
        string email = "testEmail@example.com";
        string roleName = "someRole";

        _mockUsersRepository
            .Setup(repo => repo.IsUserHasRole(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(false);

        // Act
        bool isHasRole = await _usersService.IsUserHasRole(email, roleName);

        // Assert
        isHasRole.Should().BeFalse();
    }

    [Fact]
    public async Task IsUserHasRole_ReturnTrue()
    {
        // Arrange
        string email = "testEmail@example.com";
        string roleName = "someRole";

        _mockUsersRepository
            .Setup(repo => repo.IsUserHasRole(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(true);

        // Act
        bool isHasRole = await _usersService.IsUserHasRole(email, roleName);

        // Assert
        isHasRole.Should().BeTrue();
    }
    #endregion

    #region GetUserByEmail
    [Fact]
    public async Task GetUserByEmail_ArgumentNull_ThrowArgumentNullException()
    {
        // Act
        Func<Task> thowsArgumentNullException = async ()
            => await _usersService.GetUserByEmail(null);

        // Assert
        await thowsArgumentNullException.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task GetUserByEmail_UserNotFound_ThrowArgumentException()
    {
        //Arrange
        string email = "example@ex.com";

        _mockUsersRepository.Setup(repo => repo.FindUserByEmail(It.IsAny<string>()))
            .ReturnsAsync(null as ApplicationUser);
        // Act
        Func<Task> thowsArgumentException = async ()
             => await _usersService.GetUserByEmail(email);

        // Assert
        await thowsArgumentException.Should().ThrowAsync<ArgumentException>();
    }


    [Fact]
    public async Task GetUserByEmail_ReturnUser()
    {
        //Arrange
        var appUser = _fixture.Build<ApplicationUser>()
            .With(user => user.Email, "example@ex.com")
            .With(user => user.PhoneNumber, "0639876543")
            .With(user => user.PersonName, "Some Name")
            .Create();
        var expectedUserRepsonse = appUser.ToResponse();
        _mockUsersRepository.Setup(repo => repo.FindUserByEmail(It.IsAny<string>()))
            .ReturnsAsync(appUser);
        // Act
        UserResponse? actualUserResponse = await _usersService.GetUserByEmail(appUser.Email);

        // Assert
        actualUserResponse.Should().NotBeNull();
        actualUserResponse.Should().Be(expectedUserRepsonse);
    }
    #endregion

    #region SignOut

    [Fact]
    public async Task SignOut_Verefy()
    {
        _mockUsersRepository.Setup(repo => repo.SignOut());
        // Act
        await _usersService.SignOut();

        // Assert
        _mockUsersRepository.Verify(repo => repo.SignOut(), Times.Once());
    }
    #endregion
}