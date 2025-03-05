using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using NailsServer.DTO;
using NailsServer.Models;
 

namespace NailsServer.Hubs
{
    public class ChatHub:Hub
    {
        private static Dictionary<string, string> connectedUsers = new Dictionary<string, string>();
        private readonly NailsDbContext dbContext;
        public ChatHub(NailsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public async Task SendMessage( DTO.ChatMessage message)
        {
            //Find all connections for the user id who need to recieve the message
            List<KeyValuePair<string, string>>? connections = connectedUsers.Where(x => x.Value == message.ReceiverId.ToString()).ToList();
            Models.ChatMessage m = message.GetModel();
            dbContext.ChatMessages.Add(m);
            dbContext.SaveChanges();
            
            //If all is good, loop through the connections and send them all the message
            if (connections != null)
            {
                foreach (KeyValuePair<string, string> connection in connections)
                {
                    await Clients.Client(connection.Key).SendAsync("ReceiveMessage", message);
                }
            }
        }

        public async Task<List<DTO.UserChatMessages>> OnConnect(string userId)
        {
            connectedUsers.Add(Context.ConnectionId, userId);
            await base.OnConnectedAsync();

            List<Models.ChatMessage> messages = dbContext.ChatMessages.Where(c => c.ReceiverId.ToString() == userId || c.SenderId.ToString() == userId).ToList();
            List<DTO.UserChatMessages> userMessages= new List<DTO.UserChatMessages>();
            foreach (Models.ChatMessage message in messages)
            {
               
                DTO.UserChatMessages? userChatMessages = userMessages.Where(um => um.User.UserId == message.SenderId || um.User.UserId == message.ReceiverId).FirstOrDefault();
                DTO.ChatMessage message1= new DTO.ChatMessage(message);
                
                
                if (userChatMessages == null) 
                {
                    int userid;
                    if(message.ReceiverId.ToString() == userId)
                    {
                         userid = message1.SenderId;
                    }
                    else
                    {
                        userid = message1.ReceiverId;
                    }
                    userChatMessages = new DTO.UserChatMessages();
                    Models.User u= dbContext.GetUserById(userid);
                    DTO.User user= new DTO.User(u);
                    userChatMessages.User = user;
                    userMessages.Add(userChatMessages);
                }
                userChatMessages.Messages.Add(message1);
            }
            
            return userMessages;
            
        }

        public async Task OnDisconnect()
        {
            connectedUsers.Remove(Context.ConnectionId);
            await base.OnDisconnectedAsync(null);
        }
    }
}
