using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessagingWebApi.Data;
using MessagingWebApi.Models;

namespace MessagingWebApi.Services
{
    public class ChatService : IChatService
    {
        private readonly MessagingWebApiContext _context;
        public ChatService(MessagingWebApiContext context)
        {
            _context = context;
        }

        public async Task<Chat> GetChat(User sender, User reciever)
        {
            var isChatInitiliazed =  _context.Chat.Where(
                        x =>
                        (
                            x.RecieverId == reciever.Id && x.SenderId == sender.Id) ||
                            (x.RecieverId == sender.Id && x.SenderId == reciever.Id)
                        ).First();
            return isChatInitiliazed;
        }

        public async Task<Chat> CreateChat(User sender, User reciever)
        {
            var chat = _context.Add(new Chat()
            {
                ChatGuid = sender.Username + reciever.Username,
                SenderId = sender.Id,
                RecieverId = reciever.Id,
                CreatedDate = DateTime.Now,
                IsBlocked = false
            }).Entity;
            chat.Messages = new List<Message>();

            if (sender.Chats == null)
                sender.Chats = new List<Chat>();
            if (reciever.Chats == null)
                reciever.Chats = new List<Chat>();

            sender.Chats.Add(chat);
            reciever.Chats.Add(chat);
            _context.Update(sender);
            _context.Update(reciever);
            await _context.SaveChangesAsync();

            return chat;
        }

        public async Task<Chat> UpdateChat(Chat chat)
        {
            var updatedChat = _context.Update(chat);
            await _context.SaveChangesAsync();

            return updatedChat.Entity;
        }


    }
}
