using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NailsServer.DTO
{
    public class Like
    {
        
        public int PostId { get; set; }

  
        public int UserId { get; set; }

       
        //public virtual User Post { get; set; } = null!;
        public Like() { }
        public Like(Models.Like l)
        {
            this.PostId=l.PostId;
            this.UserId=l.UserId;
        }
        public Models.Like GetModel()
        {
            Models.Like l = new Models.Like();
            l.PostId=this.PostId;
            l.UserId=this.UserId;
            return l;
        }
    }
}
