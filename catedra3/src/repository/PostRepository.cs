using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using catedra3.src.data;
using catedra3.src.interfaces;
using catedra3.src.models;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace catedra3.src.repository
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly Cloudinary _cloudinary;
        public PostRepository(ApplicationDBContext context, Cloudinary cloudinary) 
        {
            _context = context;
            _cloudinary = cloudinary;
        }

        public async Task<List<Post>> GetAllPosts()
        {
            return await _context.Posts.ToListAsync();
        }
        public async Task<Post?> GetById(string id) 
        {
            return await _context.Posts.FindAsync(id);
        }

        public async Task<Post> Post(Post post, IFormFile? image)
        {
            
            if (image == null || image.Length == 0)
            {
                throw new Exception("Image is required");
            }
            
            if (image.ContentType != "image/jpeg" && image.ContentType != "image/png" &&
                !image.FileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) &&
                !image.FileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                {
                    throw new Exception("Image must be a JPEG or PNG file");
                }
            
            if (image.Length > 5 * 1024 * 1024) 
            {
                throw new Exception("Image must be less than 5MB");
            }

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(image.FileName, image.OpenReadStream()),
                Folder = "products_images" 
            };
            
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
         
            if (uploadResult.Error != null)
            {
                throw new Exception(uploadResult.Error.Message);
            }

            
            post.ImageUrl = uploadResult.SecureUrl.ToString();

            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();

            return post; 
        }
    }
}