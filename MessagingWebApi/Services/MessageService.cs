using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessagingWebApi.Data;
using MessagingWebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace MessagingWebApi.Services
{
    public class MessageService : IMessageService
    {
        private readonly MessagingWebApiContext _context;
        public MessageService(MessagingWebApiContext context)
        {
            _context = context;
        }

        public async Task<Message> InsertMessage(User sender, User reciever, string body, Chat chat)
        {
            //TODO: Correct return types add logging
            try
            {
                var messages  = _context.Messages.Add(new Message()
                {
                    Body = body,
                    SenderId = sender.Id,
                    RecieverId = reciever.Id,
                    CreatedDate = DateTime.Now,
                    IsRead = false,
                    ReadDate = null,
                    IsDeleted =false,
                    ChatId = chat.Id,
                }).Entity;
                await _context.SaveChangesAsync();
                return messages;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task ReadMessage(User sender, User reciever)
        {
            try
            {
                var messages = _context.Messages.Where(x => x.SenderId == reciever.Id && x.RecieverId == sender.Id && x.IsRead == false).OrderBy(x => x.CreatedDate);
                if (messages.Count() == 0) return;
                foreach (var message in messages.ToList())
                {
                    message.IsRead = true;
                    message.ReadDate = DateTime.Now;
                    _context.Entry(message).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


    }
}
