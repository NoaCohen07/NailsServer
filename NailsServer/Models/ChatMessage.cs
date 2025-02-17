using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NailsServer.Models;

public partial class ChatMessage
{
    [Column("SenderID")]
    public int? SenderId { get; set; }

    [Column("ReceiverID")]
    public int? ReceiverId { get; set; }

    [Key]
    [Column("MessageID")]
    public int MessageId { get; set; }

    [StringLength(4000)]
    public string? MessageText { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime MessageTime { get; set; }

    public bool Seen { get; set; }

    [StringLength(5)]
    public string? Pic { get; set; }

    [StringLength(5)]
    public string? Video { get; set; }

    [ForeignKey("ReceiverId")]
    [InverseProperty("ChatMessageReceivers")]
    public virtual User? Receiver { get; set; }

    [ForeignKey("SenderId")]
    [InverseProperty("ChatMessageSenders")]
    public virtual User? Sender { get; set; }
}
