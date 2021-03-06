﻿using Microsoft.AspNetCore.Mvc;
using Tabloid.Data;
using Tabloid.Repositories;
using Tabloid.Models;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Tabloid.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly PostRepository _postRepository;
        private readonly UserProfileRepository _userProfileRepository;

        public PostController(ApplicationDbContext context)
        {
            _postRepository = new PostRepository(context);
            _userProfileRepository = new UserProfileRepository(context);
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_postRepository.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var post = _postRepository.GetById(id);
            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }

        [HttpGet("getbyuser/{id}")]
        public IActionResult GetByUser(int id)
        {
            return Ok(_postRepository.GetByUserProfileId(id));
        }

        [HttpPost]
        public IActionResult Post(Post post)
        {
            _postRepository.Add(post);
            return CreatedAtAction("Get", new { id = post.Id }, post);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, Post post)
        {
            if (id != post.Id)
            {
                return BadRequest();
            }

            _postRepository.Update(post);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var user = GetCurrentUserProfile();
            var post = _postRepository.GetById(id);
            if (user.Id != post.UserProfileId)
            {
                return Forbid();
            }
           
            _postRepository.Delete(id);
            return NoContent();
        }
        [HttpPost("addtag")]
        public IActionResult Post(PostTag postTag)
        {
            var currentUserProfile = GetCurrentUserProfile();
            var post = _postRepository.GetById(postTag.PostId);

            if (currentUserProfile.Id != post.UserProfileId)
            {
                return Unauthorized();
            }
            _postRepository.InsertTag(postTag);
            return CreatedAtAction("Get", new { id = postTag.Id }, postTag);
        }

        [HttpDelete("addtag/{id}")]
        public IActionResult DeletePostTag(int id)
        {
            var currentUserProfile = GetCurrentUserProfile();
            var postTag = _postRepository.GetPostTagById(id);

            var post = _postRepository.GetById(postTag.PostId);

            if (currentUserProfile.Id != post.UserProfileId)
            {
                return Unauthorized();
            }

            _postRepository.RemoveTag(id);
            return NoContent();
        }

        private UserProfile GetCurrentUserProfile()
        {
            var firebaseUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return _userProfileRepository.GetByFirebaseUserId(firebaseUserId);
        }
    }
}
