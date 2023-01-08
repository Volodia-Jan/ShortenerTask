using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shortener.Core.DTO;

public class LoginDTO
{
    [Required(ErrorMessage = "{0} can not be blank")]
    [EmailAddress(ErrorMessage = "{0} is not valid email address")]
    public string? Email { get; set; }
    [Required(ErrorMessage = "{0} can not be blank")]

    [MinLength(4, ErrorMessage = "{0} must have at least {1} characters")]
    public string? Password { get; set; }
}
