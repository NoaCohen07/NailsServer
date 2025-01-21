using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NailsServer.Models;

[PrimaryKey("UserId", "PostId")]
public partial class Favorite
{
    [Key]
    [Column("UserID")]
    public int UserId { get; set; }

    [Key]
    [Column("PostID")]
    public int PostId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime SavedTime { get; set; }

    [ForeignKey("PostId")]
    [InverseProperty("Favorites")]
    public virtual Post Post { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Favorites")]
    public virtual User User { get; set; } = null!;
}
