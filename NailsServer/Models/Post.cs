﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NailsServer.Models;

[Table("Post")]
public partial class Post
{
    [Column("UserID")]
    public int? UserId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime PostTime { get; set; }

    [StringLength(300)]
    public string? PostText { get; set; }

    [StringLength(50)]
    public string Pic { get; set; } = null!;

    [Key]
    [Column("PostID")]
    public int PostId { get; set; }

    [InverseProperty("Post")]
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    [InverseProperty("Post")]
    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    [ForeignKey("UserId")]
    [InverseProperty("PostsNavigation")]
    public virtual User? User { get; set; }

    [ForeignKey("PostId")]
    [InverseProperty("Posts")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
