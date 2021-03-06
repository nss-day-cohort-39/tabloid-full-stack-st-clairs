﻿using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Tabloid.Data;
using Tabloid.Models;
using System;

namespace Tabloid.Repositories
{
    public class PostRepository
    {
        private readonly ApplicationDbContext _context;

        public PostRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Post> GetAll()
        {
            return _context.Post
                .Include(p => p.UserProfile)
                .Include(p => p.Comments)
                .Include(p => p.Category)
                .OrderByDescending(p => p.CreateDateTime)
                .Where(p => p.IsApproved == true && p.PublishDateTime < DateTime.Now)
                .ToList();

        }

        public Post GetById(int id)
        {
            return _context.Post.Include(p => p.UserProfile)
                                .Include(p => p.Comments)
                                .Include(p => p.Category)
                                .Include(p => p.PostTags)
                                .ThenInclude(pt => pt.Tag)
                                .OrderByDescending(p => p.CreateDateTime)
                                .FirstOrDefault(p => p.Id == id);
        }

        public List<Post> GetByUserProfileId(int id)
        {
            return _context.Post.Include(p => p.UserProfile)
                            .Include(p => p.Comments)
                            .Include(p => p.Category)
                            .Include(p => p.PostTags)
                            .ThenInclude(pt => pt.Tag)
                            .Where(p => p.UserProfileId == id)
                            .OrderByDescending(p => p.CreateDateTime)
                            .ToList();
        }

        public void Add(Post post)
        {
            _context.Add(post);
            _context.SaveChanges();
        }

        public void Update(Post post)
        {
            _context.Entry(post).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var post = GetById(id);
            _context.Post.Remove(post);
            _context.SaveChanges();
        }
        public PostTag GetPostTagById(int id)
        {
            return _context.PostTag
                           .FirstOrDefault(pt => pt.Id == id);
        }
        public void InsertTag(PostTag postTag)
        {
            _context.PostTag.Add(postTag);
            _context.SaveChanges();
        }

        public void RemoveTag(int id)
        {
            var postTag = GetPostTagById(id);
            _context.PostTag.Remove(postTag);
            _context.SaveChanges();
        }
    }
}
