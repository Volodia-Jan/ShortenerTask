using Microsoft.Extensions.Logging;using Serilog;using SerilogTimings;using Shortener.Core.DTO;using Shortener.Core.Enums;using Shortener.Core.Helper;using Shortener.Core.IdentityEntities;using Shortener.Core.RepositoryContracts;using Shortener.Core.ServiceContracts;namespace Shortener.Core.Services;

public class UsersService : IUsersService
{
    private readonly IUsersRepository _userRepository;
    private readonly ILogger<UsersService> _logger;
    // IDiagnosticContext adds some additioanal info in Seq loggs
    // To use it we need to install Serilog.Extensions.Hosting NuGetPackage
    private readonly IDiagnosticContext _diagnosticContext;

    public UsersService(IUsersRepository userRepository, ILogger<UsersService> logger, IDiagnosticContext diagnosticContext)
    {
        _userRepository = userRepository;
        _logger = logger;
        _diagnosticContext = diagnosticContext;
    }

    public Task<UserResponse> GetUserByEmail(string? email)
    {
        _logger.LogInformation($"{nameof(IUsersService.GetUserByEmail)} method is executing");
        if (string.IsNullOrEmpty(email))
            throw new ArgumentNullException(nameof(email));

        _logger.LogDebug($"{nameof(email)}:{email}");
        return GetUserByEmailAsync(email);

        async Task<UserResponse> GetUserByEmailAsync(string emailToSearch)
        {
            var user = await _userRepository.FindUserByEmail(emailToSearch);
            return user is null
                ? throw new ArgumentException($"User was not found by email:{emailToSearch}")
                : user.ToResponse();
        }
    }

    public Task<bool> IsUserHasRole(string? email, string? roleName)
    {
        _logger.LogInformation($"{nameof(IUsersService.IsUserHasRole)} method is executing");
        if (string.IsNullOrEmpty(email))
            throw new ArgumentNullException(nameof(email));

        if (string.IsNullOrEmpty(roleName))
            throw new ArgumentNullException(nameof(roleName));

        _logger.LogDebug($"{nameof(email)}:{email}, {nameof(roleName)}:{roleName}");


        return IsUserHasRoleAsync(email, roleName);

        async Task<bool> IsUserHasRoleAsync(string emailToSearch, string roleToSearch)
            => await _userRepository.IsUserHasRole(emailToSearch, roleToSearch);
    }

    public async Task<bool> Login(LoginDTO loginDTO)
    {
        _logger.LogInformation($"{nameof(IUsersService.Login)} method is executing");
        ValidationHelper.ModelValidation(loginDTO);

        _logger.LogDebug($"{nameof(loginDTO)}:{loginDTO}");
        _diagnosticContext.Set("Login details", loginDTO);

        return await _userRepository.SignIn(loginDTO.Email, loginDTO.Password);
    }

    public async Task<bool> Register(RegisterDto registerDto)
    {
        _logger.LogInformation($"{nameof(IUsersService.Register)} method is executing");
        ValidationHelper.ModelValidation(registerDto);

        _logger.LogDebug($"{nameof(registerDto)}:{registerDto}");
        ApplicationUser user = new()
        {
            Id = Guid.NewGuid(),
            PersonName = registerDto.UserName,
            UserName = registerDto.Email,
            Email = registerDto.Email,
            PhoneNumber = registerDto.Phone
        };

        if (await _userRepository.FindUserByEmail(user.Email) != null)
            return false;

        // If some opertaion takes some time and you wanna emphasize(підкреслити) it by adding additional log
        // you can install SerilogTimings NuGet package and use next statments
        using (Operation.Time($"Time for user registration: UserId:{user.Id}, Username:{user.UserName}"))
        {
            var isRegister = await _userRepository.SaveUser(user, registerDto.Password, Role.Ordinal.ToString());
            if (isRegister)
            {
                await _userRepository.SignIn(user);
                return true;
            }
        }
        return false;
    }

    public async Task SignOut()
    {
        _logger.LogInformation($"{nameof(IUsersService.SignOut)} method is executing");
        await _userRepository.SignOut();
    }
}