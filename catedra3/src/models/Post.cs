using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace catedra3.src.models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string PublishDate { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;

    }
}