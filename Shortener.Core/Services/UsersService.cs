using Shortener.Core.DTO;
using Shortener.Core.Enums;
using Shortener.Core.Helper;
using Shortener.Core.IdentityEntities;
using Shortener.Core.RepositoryContracts;
using Shortener.Core.ServiceContracts;

namespace Shortener.Core.Services;

public class UsersService : IUsersService
{
    private readonly IUsersRepository _userRepository;

    public UsersService(IUsersRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<UserResponse> GetUserByEmail(string? email)
    {
        if (string.IsNullOrEmpty(email))
            throw new ArgumentNullException(nameof(email));
        
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
        if (string.IsNullOrEmpty(email))
            throw new ArgumentNullException(nameof(email));

        if (string.IsNullOrEmpty(roleName))
            throw new ArgumentNullException(nameof(roleName));

        return IsUserHasRoleAsync(email, roleName);

        async Task<bool> IsUserHasRoleAsync(string emailToSearch, string roleToSearch)
            => await _userRepository.IsUserHasRole(emailToSearch, roleToSearch);
    }

    public async Task<bool> Login(LoginDTO loginDTO)
    {
        ValidationHelper.ModelValidation(loginDTO);

        return await _userRepository.SignIn(loginDTO.Email, loginDTO.Password);
    }

    public async Task<bool> Register(RegisterDto registerDto)
    {
        ValidationHelper.ModelValidation(registerDto);

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

        var isRegister = await _userRepository.SaveUser(user, registerDto.Password, Role.Ordinal.ToString());
        if (isRegister)
        {
            await _userRepository.SignIn(user);
            return true;
        }
        return false;
    }

    public async Task SignOut()
    {
        await _userRepository.SignOut();
    }
}
