using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessagingWebApi.Models;

namespace MessagingWebApi.Services
{
    public interface IMessageService
    {
        public Task<Message> InsertMessage(User sender, User reciever, string body, Chat chat);
        public Task ReadMessage(User sender, User reciever);
    }
}
