using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace NailsServer.Models;

public partial class NailsDbContext : DbContext
{
    public User? GetUser(string email)
    {
        return this.Users.Where(u => u.Email == email)
                            .FirstOrDefault();
    }

    public User? GetUser(int id)
    {
        return this.Users.Where(u => u.UserId == id)
                            .FirstOrDefault();
    }

    public Post? GetPost(int id)
    {
        return this.Posts.Where(p => p.PostId == id)
                            .FirstOrDefault();
    }
    public List<Post> GetPosts(string email)
    {
        User? u = GetUser(email);
        return this.Posts.Where(p => p.UserId == u.UserId).OrderByDescending(p => p.PostTime).ToList();
                            
    }

    public List<Comment> GetComments(int postId)
    {
        return this.Comments.Where(c => c.PostId==postId).OrderByDescending(c => c.CommentTime).ToList();

    }

    public int GetNumLikes (int postId)
    {
        return this.Likes.Where(c => c.PostId == postId).Count();

    }

    public List<User> GetManicurists()
    {
        return this.Users.Where(u => u.IsManicurist == true).ToList();
    }

    public List<User> GetUsers()
    {
        return this.Users.ToList();
    }

    public List<Post> GetAllPosts()
    {
        return this.Posts.ToList();
    }
}

