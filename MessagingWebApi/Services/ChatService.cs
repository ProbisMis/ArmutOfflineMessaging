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
            if (_context.Chats.Count() == 0) return null;
            var isChatInitiliazed =  _context.Chats.Where(
                        x =>
                        (
                            (x.RecieverId == reciever.Id && x.SenderId == sender.Id) ||
                            (x.RecieverId == sender.Id && x.SenderId == reciever.Id))
                        ).First();
            _context.Entry(isChatInitiliazed).Collection(s => s.Messages).Load();
            await _context.SaveChangesAsync();

            isChatInitiliazed.Messages = _context.Messages.Where(x => x.ChatId == isChatInitiliazed.Id).ToList();

            return isChatInitiliazed;
        }

        public async Task<List<Chat>> GetAllChats(User user)
        {
            var result = _context.Chats.Where(
                         x =>
                         (
                             (x.RecieverId == user.Id || x.SenderId == user.Id))
                         ).ToList();
            //await _context.Entry(user).Collection(s => s.Chats).LoadAsync();
            //var updatedUser = _context.Users.Where(x => x.Username == user.Username).First();
            //return updatedUser.Chats.ToList();

            return result;
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
