namespace NailsServer.DTO
{
    public class PostData
    {
        public virtual ICollection<Comment> PostComments { get; set; } = new List<Comment>();
        public int NumLikes { get; set; }
    }
}
