using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NailsServer.DTO;

public class ChatMessage
{
   
    public int SenderId { get; set; }

   
    public int ReceiverId { get; set; }

    
    public int MessageId { get; set; }

   
    public string? MessageText { get; set; }

    
    public DateTime MessageTime { get; set; }

    public bool Seen { get; set; }

    public string? Pic { get; set; }

   

   
  
    public string? Video { get; set; }

    //public virtual User? Receiver { get; set; }


    //public virtual User? Sender { get; set; }
    public ChatMessage() { }
    public ChatMessage(Models.ChatMessage message)
    {
        this.SenderId = message.SenderId.Value;
        this.ReceiverId = message.ReceiverId.Value;
        this.MessageId = message.MessageId;
        this.MessageText = message.MessageText;
        this.MessageTime = message.MessageTime;
        this.Pic = message.Pic;
        this.Video = message.Video;
        this.Seen= message.Seen;
    }
    public Models.ChatMessage GetModel()
    {
        Models.ChatMessage message=new Models.ChatMessage();
        message.SenderId = this.SenderId;
        message.ReceiverId = this.ReceiverId;
        message.MessageId = this.MessageId;
        message.MessageText = this.MessageText;
        message.MessageTime = this.MessageTime;
        message.Pic = this.Pic;
        message.Video = this.Video;
        message.Seen=this.Seen;
        return message;
    }
}