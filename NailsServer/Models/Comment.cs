using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NailsServer.Models;

[Table("Comment")]
public partial class Comment
{
    [Column("PostID")]
    public int? PostId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CommentTime { get; set; }

    [StringLength(300)]
    public string? CommentText { get; set; }

    [Column("UserID")]
    public int UserId { get; set; }

    [Key]
    [Column("CommentID")]
    public int CommentId { get; set; }

    [ForeignKey("PostId")]
    [InverseProperty("Comments")]
    public virtual User? Post { get; set; }
}
