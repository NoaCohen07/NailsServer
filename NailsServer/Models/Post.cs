using System;
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

    [StringLength(5)]
    public string Pic { get; set; } = null!;

    [Key]
    [Column("PostID")]
    public int PostId { get; set; }

    [InverseProperty("Post")]
    public virtual Favorite? Favorite { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Posts")]
    public virtual User? User { get; set; }
}
