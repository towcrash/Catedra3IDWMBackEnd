using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace catedra3.src.models
{
    public class AppUser : IdentityUser
    {
        public List<Post> Posts { get; set; } = new List<Post>();
    }
}