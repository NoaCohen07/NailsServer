using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NailsServer.DTO;

public class ChatMessage
{
   
    public int? SenderId { get; set; }

   
    public int? ReceiverId { get; set; }

    
    public int MessageId { get; set; }

   
    public string? MessageText { get; set; }

    
    public DateTime MessageTime { get; set; }


    public string? Pic { get; set; }

  
    public string? Video { get; set; }


    public virtual User? Receiver { get; set; }


    public virtual User? Sender { get; set; }
}