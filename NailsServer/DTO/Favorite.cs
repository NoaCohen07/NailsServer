using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NailsServer.DTO
{
    public class Favorite
    {
       
        public int? UserId { get; set; }

    
        public int PostId { get; set; }

 
        public DateTime SavedTime { get; set; }

        //public virtual Post Post { get; set; } = null!;

       
        //public virtual User? User { get; set; }

        public Favorite() { }
        public Favorite(Models.Favorite f)
        {
            this.UserId= f.UserId;
            this.PostId= f.PostId;
            this.SavedTime= f.SavedTime;
        }
        public Models.Favorite GetModel()
        {
            Models.Favorite f=new Models.Favorite();
            f.UserId= this.UserId;  
            f.PostId= this.PostId;
            f.SavedTime= this.SavedTime;
            return f;
        }
    }

}
