using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessagingWebApi.Models;

namespace MessagingWebApi.Services
{
    public interface IChatService
    {
        public ChatResponseModel GetChat(User sender, User reciever);
        public ChatResponseModel CreateChat(User sender, User reciever);
        public ChatResponseModel UpdateChat(Chat chat);
        public Task<List<Chat>> GetAllChats(User user);
     }
}
