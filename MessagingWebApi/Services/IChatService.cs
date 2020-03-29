using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessagingWebApi.Models;

namespace MessagingWebApi.Services
{
    public interface IChatService
    {
        public Task<Chat> GetChat(User sender, User reciever);
        public Task<Chat> CreateChat(User sender, User reciever);
        public Task<Chat> UpdateChat(Chat chat);
    }
}
