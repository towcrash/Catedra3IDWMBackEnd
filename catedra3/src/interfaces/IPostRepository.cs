using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using catedra3.src.models;

namespace catedra3.src.interfaces
{
    public interface IPostRepository
    {
        Task<List<Post>> GetAllPosts();
        Task<Post?> GetById(string id);
        Task<Post> Post(Post post, IFormFile? image);

    }
}