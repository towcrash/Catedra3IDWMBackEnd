using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using catedra3.src.dtos;
using catedra3.src.interfaces;
using catedra3.src.models;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace catedra3.src.controllers
{   
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        public PostController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll() 
        {
            var posts = await _postRepository.GetAllPosts();
            return Ok(posts);
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var post = await _postRepository.GetById(id);

            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [Authorize]
        public async Task<IActionResult> PostAsync([FromForm] PostDto postDto)
        {
            DateTime time = DateTime.Now;
            string formattedTime = time.ToString("yyyy-MM-dd HH:mm:ss");
            var postModel = new Post
            {
                Title = postDto.Title,
                PublishDate = time
            };
            await _postRepository.Post(postModel, postDto.Image); 

            return CreatedAtAction(nameof(GetById), new { id = postModel.Id }, postModel);
        }
    }
}