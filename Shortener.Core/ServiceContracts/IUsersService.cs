using Shortener.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shortener.Core.ServiceContracts;

public interface IUsersService
{

    Task<UserResponse> GetUserByEmail(string? email);

    Task<bool> IsUserHasRole(string? email, string? roleName);

    Task<bool> Register(RegisterDto registerDto);

    Task<bool> Login(LoginDTO loginDTO);

    Task SignOut();
}