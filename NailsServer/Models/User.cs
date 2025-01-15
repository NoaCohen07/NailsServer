using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NailsServer.Models;

[Index("Email", Name = "UQ__Users__A9D10534925A8033", IsUnique = true)]
public partial class User
{
    [Key]
    [Column("UserID")]
    public int UserId { get; set; }

    [StringLength(25)]
    public string FirstName { get; set; } = null!;

    [StringLength(50)]
    public string LastName { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    [StringLength(70)]
    public string Email { get; set; } = null!;

    [StringLength(10)]
    public string PhoneNumber { get; set; } = null!;

    [StringLength(100)]
    public string UserAddress { get; set; } = null!;

    [StringLength(1)]
    [Unicode(false)]
    public string Gender { get; set; } = null!;

    [StringLength(20)]
    public string Pass { get; set; } = null!;

    public bool IsManicurist { get; set; }

    public bool IsBlocked { get; set; }

    [StringLength(50)]
    public string ProfilePic { get; set; } = null!;

    public bool IsManager { get; set; }

    [InverseProperty("Receiver")]
    public virtual ICollection<ChatMessage> ChatMessageReceivers { get; set; } = new List<ChatMessage>();

    [InverseProperty("Sender")]
    public virtual ICollection<ChatMessage> ChatMessageSenders { get; set; } = new List<ChatMessage>();

    [InverseProperty("Post")]
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    [InverseProperty("User")]
    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    [InverseProperty("Post")]
    public virtual ICollection<Like> Likes { get; set; } = new List<Like>();

    [InverseProperty("User")]
    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    [InverseProperty("User")]
    public virtual ICollection<Treatment> Treatments { get; set; } = new List<Treatment>();
}
