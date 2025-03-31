using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NailsServer.Models;

[PrimaryKey("PostId", "UserId")]
public partial class Like
{
    [Key]
    [Column("PostID")]
    public int PostId { get; set; }

    [Key]
    [Column("UserID")]
    public int UserId { get; set; }

    [ForeignKey("PostId")]
    [InverseProperty("Likes")]
    public virtual Post Post { get; set; } = null!;
}
