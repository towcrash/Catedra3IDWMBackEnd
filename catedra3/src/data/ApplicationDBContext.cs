using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using catedra3.src.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace catedra3.src.data
{
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions): base(dbContextOptions) 
        {

        }

        public DbSet<Post> Posts { get; set; } = null!;
        
    }
}