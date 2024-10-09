using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NailsServer.DTO
{
    public class Favorite
    {
       
        public int? UserId { get; set; }

    
        public int PostId { get; set; }

 
        public DateTime SavedTime { get; set; }

      
        public virtual Post Post { get; set; } = null!;

    
        public virtual User? User { get; set; }
    }

}
