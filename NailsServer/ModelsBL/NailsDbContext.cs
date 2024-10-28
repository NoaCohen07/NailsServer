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
}

