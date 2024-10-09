using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NailsServer.Models;

public partial class Favorite
{
    [Column("UserID")]
    public int? UserId { get; set; }

    [Key]
    [Column("PostID")]
    public int PostId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime SavedTime { get; set; }

    [ForeignKey("PostId")]
    [InverseProperty("Favorite")]
    public virtual Post Post { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Favorites")]
    public virtual User? User { get; set; }
}
