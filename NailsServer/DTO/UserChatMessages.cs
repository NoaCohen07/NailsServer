using System.Collections.ObjectModel;

namespace NailsServer.DTO
{
    public class UserChatMessages
    {
        public ObservableCollection<ChatMessage> Messages { get; set; }
        public virtual User User { get; set; }
    }
}
