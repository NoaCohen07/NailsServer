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

    public Models.User? GetUser1(int id)
    {
        return this.Users.Where(u => u.UserId == id)
                            .FirstOrDefault();
    }

    public Models.User? GetUserEmail(string email)
    {
        return this.Users.Where(u => u.Email == email)
                            .FirstOrDefault();
    }

    public Post? GetPost(int id)
    {
        return this.Posts.Where(p => p.PostId == id)
                            .FirstOrDefault();
    }

    public Like? GetLike(int userId, int postId)
    {
        return this.Likes.Where(p => p.PostId == postId && p.UserId==userId)
                            .FirstOrDefault();
    }
    public List<Post> GetPosts(string email)
    {
        User? u = GetUser(email);
        if (u != null) 
        {
            return this.Posts.Where(p => p.UserId == u.UserId).OrderByDescending(p => p.PostTime).ToList();
        }
        else
        {
            return new List<Post>();
        }                  
    }
    public List<Post> GetFavorites(string email)
    {
        User? u = GetUser(email);
        if (u != null)
            return this.Posts.Where(p => p.Favorites.Where(ff => ff.UserId == u.UserId).Any()).OrderByDescending(p => p.PostTime).ToList();
        else
            return new List<Post>();
    }

    public List<Comment> GetComments(int postId)
    {
        return this.Comments.Where(c => c.PostId==postId).OrderByDescending(c => c.CommentTime).ToList();

    }

    public int GetNumLikes (int postId)
    {
        return this.Likes.Where(c => c.PostId == postId).Count();

    }
    public bool GetFavorite(int userid, int postId)
    {
        Favorite? fav= this.Favorites.Where(p => p.PostId == postId && p.UserId == userid)
                            .FirstOrDefault();
        if (fav == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public bool GetLiked(int userid, int postId)
    {
        Like? like = this.Likes.Where(p => p.PostId == postId && p.UserId == userid)
                            .FirstOrDefault();
        if (like == null)
        {
            return false;
        }
        else
        {
            return true;
        }
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

    public List<string> GetAllEmails()
    {
        return this.Users.Select(u => u.Email).ToList();
    }
    public List<Treatment> GetTreatments(string email)
    {
        User? u = GetUser(email);
        if (u != null)
        {
            return this.Treatments.Where(p => p.UserId == u.UserId).ToList();
        }
        else
        {
            return new List<Treatment>();
        }


    }
}

