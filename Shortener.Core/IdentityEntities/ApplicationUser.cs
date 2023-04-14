using Microsoft.AspNetCore.Identity;
using Shortener.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shortener.Core.IdentityEntities;

public class ApplicationUser : IdentityUser<Guid>
{
    public string? PersonName { get; set; }
}

public static class ApplicationUserExtensions
{
    public static UserResponse ToResponse(this ApplicationUser user) => new()
    {
        Id = user.Id,
        UserName = user.PersonName,
        Email = user.Email,
        Phone = user.PhoneNumber
    };
}