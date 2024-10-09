using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NailsServer.DTO
{
    public class Like
    {
        
        public int PostId { get; set; }

  
        public int UserId { get; set; }

       
        public virtual User Post { get; set; } = null!;
    }
}
