using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace catedra3.src.dtos
{
    public class PostDto
    {
        [Required]
        [MinLength(5, ErrorMessage = "Title must contain at least 5 characters")]
        public required string Title {get; set;}
        

        [Required]
        public required IFormFile? Image { get; set; }
        [Required]
        public required string AppUserId { get; set; }
        
    }
}