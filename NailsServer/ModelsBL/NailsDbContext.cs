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

    public List<Post> GetPosts(string email)
    {
        User? u = GetUser(email);
        return this.Posts.Where(p => p.UserId == u.UserId).OrderByDescending(p => p.PostTime).ToList();
                            
    }
}

