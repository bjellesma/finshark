using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Account
{
    public class RegisterUserDto
    {
        [Required]
        public string? Username {get;set;}
        [Required]
        // email is a built annotation that .Net provides for validating email strings
        [EmailAddress]
        public string? Email {get; set;}
        [Required]
        public string? Password { get; set; }
    }
}