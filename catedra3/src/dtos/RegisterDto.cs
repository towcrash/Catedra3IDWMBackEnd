using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using catedra3.src.Validators;

namespace catedra3.src.dtos
{
    public class RegisterDto
    {
        [EmailAddress]
        [Required]
        public string Email {get; set;} = null!;

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        [ContainsNumber]
        public string Password {get; set;} = null!;
    }
}