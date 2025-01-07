using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace catedra3.src.dtos
{
    public class NewUserDto
    {
        [EmailAddress]
        [Required]
        public string Email {get; set;} = null!;

        [Required]
        public string Token {get; set;} = null!;
    }
}