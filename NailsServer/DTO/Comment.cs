using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NailsServer.DTO
{
    public class Comment
    {
      
        public int? PostId { get; set; }

     
        public DateTime CommentTime { get; set; }

     
        public string? CommentText { get; set; }

  
        public int UserId { get; set; }

       
        public int CommentId { get; set; }

  
        public virtual User? Post { get; set; }
    }
}
