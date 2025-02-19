using System.Collections.ObjectModel;

namespace NailsServer.DTO
{
    public class UserChatMessages
    {
        public List<ChatMessage> Messages { get; set; }
        public virtual User User { get; set; }

        public UserChatMessages()
        {
            this.Messages = new List<ChatMessage>();
        }
    }
}
