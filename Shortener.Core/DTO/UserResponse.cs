using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shortener.Core.DTO;

public class UserResponse
{
    public Guid Id { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;
        if (obj.GetType() != typeof(UserResponse))
            return false;
        var userResponse = (UserResponse)obj;

        return Id.Equals(userResponse.Id) &&
            UserName == userResponse.UserName &&
            Email == userResponse.Email &&
            Phone == userResponse.Phone;
    }

    public override int GetHashCode()
    {
        int result = Id.GetHashCode();
        result = 31 * result + (UserName?.GetHashCode() ?? 0);
        result = 31 * result + (Email?.GetHashCode() ?? 0);
        result = 31 * result + (Phone?.GetHashCode() ?? 0);
        return result;
    }
}
