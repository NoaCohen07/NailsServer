using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NailsServer.DTO
{
    public class Post
    {
   
        public int? UserId { get; set; }

        public DateTime PostTime { get; set; }

        public string? PostText { get; set; }

      
        public string Pic { get; set; } = null!;

    
        public int PostId { get; set; }

       
        public virtual Favorite? Favorite { get; set; }

       
        public virtual User? User { get; set; }
    }
}
