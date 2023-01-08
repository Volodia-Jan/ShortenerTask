﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shortener.Core.DTO;

public class RegisterDto
{
    [Required(ErrorMessage = "{0} can not be blank")]
    [DisplayName("Full name")]
    public string? UserName { get; set; }

    [Required(ErrorMessage = "{0} can not be blank")]
    [EmailAddress(ErrorMessage = "{0} is not valid email address")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "{0} can not be blank")]
    [Phone(ErrorMessage = "{0} must contain only numerical characters")]
    public string? Phone { get; set; }

    [Required(ErrorMessage = "{0} can not be blank")]
    public string? Password { get; set; }

    [Required(ErrorMessage = "{0} can not be blank")]
    [Compare("Password", ErrorMessage = "{1} and {0} must be equal")]
    public string? ConfirmPassword { get; set; }
}
