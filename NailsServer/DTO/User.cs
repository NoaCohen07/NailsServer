using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NailsServer.DTO
{
    public class User
    {
        public int UserId { get; set; }

       
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public DateOnly DateOfBirth { get; set; }

   
        public string Email { get; set; } = null!;

    
        public string PhoneNumber { get; set; } = null!;

    
        public string UserAddress { get; set; } = null!;

        public string Gender { get; set; } = null!;


        public string Pass { get; set; } = null!;

        public bool IsManicurist { get; set; }

        public bool IsBlocked { get; set; }

 
        public string? ProfilePic { get; set; }

        public bool IsManager { get; set; }


     
        public virtual ICollection<ChatMessage> ChatMessageReceivers { get; set; } = new List<ChatMessage>();

       
        public virtual ICollection<ChatMessage> ChatMessageSenders { get; set; } = new List<ChatMessage>();

       
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

     
        public virtual ICollection<Like> Likes { get; set; } = new List<Like>();

     
        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();


        public virtual ICollection<Treatment> Treatments { get; set; } = new List<Treatment>();
    }
}
