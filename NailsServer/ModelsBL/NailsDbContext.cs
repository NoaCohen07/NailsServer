using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace NailsServer.Models;

public partial class NailsDbContext : DbContext
{
    public User? GetUser(string email)// return user by email
    {
        return this.Users.Where(u => u.Email == email)
                            .FirstOrDefault();
    }

    public Models.User? GetUserById(int id)//returns user by user id
    {
        return this.Users.Where(u => u.UserId == id)
                            .FirstOrDefault();
    }


    public Post? GetPost(int id)//returns post by post id
    {
        return this.Posts.Where(p => p.PostId == id)
                            .FirstOrDefault();
    }

    public Like? GetLike(int userId, int postId)// return like by the post id (which post) and user id (which user liked)
    {
        return this.Likes.Where(p => p.PostId == postId && p.UserId==userId)
                            .FirstOrDefault();
    }
    public List<Post> GetPosts(string email)// returns all of the posts that the user posted and if user returns null will return empty list
    {
        User? u = GetUser(email);
        if (u != null) 
        {
            return this.Posts.Where(p => p.UserId == u.UserId).Include(p=>p.Likes).OrderByDescending(p => p.PostTime).ToList();
        }
        else
        {
            return new List<Post>();
        }                  
    }
    public List<Post> GetFavorites(string email)// returns all of the posts that the user favorite and if user returns null will return empty list
    {
        User? u = GetUser(email);
        if (u != null)
            return this.Posts.Where(p => p.Favorites.Where(ff => ff.UserId == u.UserId).Any()).OrderByDescending(p => p.PostTime).ToList();
        else
            return new List<Post>();
    }

    public List<Comment> GetComments(int postId)//returns a list with all the comments that the user has posted ordered by what he has posted last to first
    {
        return this.Comments.Where(c => c.PostId==postId).OrderByDescending(c => c.CommentTime).ToList();

    }

    public int GetNumLikes (int postId)//returns number of likes that a post has
    {
        return this.Likes.Where(c => c.PostId == postId).Count();

    }
    public bool GetFavorite(int userid, int postId)// returns the favorite post the a user favorite, if he hasn't favorite the post, fav will be null and return false
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
    public bool GetLiked(int userid, int postId)// returns the post the a user liked, if he hasn't liked the post, like will be null and return false
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

    public List<User> GetManicurists()// returns a list with all of the manicurists in the app
    {
        return this.Users.Where(u => u.IsManicurist == true).ToList();
    }

    public List<User> GetUsers()// returns a list with all of the users in the app
    {
        return this.Users.ToList();
    }

    public List<Post> GetAllPosts()// reutrns a list with all of the posts that have been posted
    {
        return this.Posts.Include(u=>u.Likes).ToList();
    }

    public List<string> GetAllEmails()//returns a list of emails of all of the users in the app
    {
        return this.Users.Select(u => u.Email).ToList();
    }
    public List<Treatment> GetTreatments(string email)//returns a list of treatments posted by a specific manicurist, if manicurist didn't post anything will return an empty list
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

    public void ChangeAllMessagesToSeen(int senderId, int recieverId)
    {
        List<ChatMessage> messages  = ChatMessages.Where(m => m.SenderId == senderId && m.ReceiverId== recieverId && !m.Seen).ToList();
        
        this.ChangeTracker.Clear();
        foreach (ChatMessage m in messages)
        {
            m.Seen = true;
            ChatMessages.Update(m);
        }

        this.SaveChanges();
    }

   
}

