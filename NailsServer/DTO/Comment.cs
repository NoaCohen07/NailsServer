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

        //public virtual User? Post { get; set; }

        public Comment() { }
        public Comment(Models.Comment c)
        {
            this.PostId = c.PostId;
            this.CommentTime = c.CommentTime;
            this.CommentText = c.CommentText;
            this.UserId = c.UserId;
            this.CommentId = c.CommentId;
            
        }
        public Models.Comment GetModel()
        {
            Models.Comment c= new Models.Comment();
            c.PostId = this.PostId;
            c.CommentTime = this.CommentTime;
            c.CommentText = this.CommentText;
            c.UserId = this.UserId;
            c.CommentId = this.CommentId;
            return c;
        }
    }
}
