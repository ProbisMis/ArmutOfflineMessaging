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

        public async Task<Message> InsertMessage(User sender, User reciever, string body)
        {
            //TODO: Correct return types add logging
            try
            {
                var chat  = _context.Messages.Add(new Message()
                {
                    Body = body,
                    SenderId = sender.Id,
                    RecieverId = reciever.Id,
                    CreatedDate = DateTime.Now,
                    IsRead = false,
                    ReadDate = null,
                    IsDeleted =false
                }).Entity;
                await _context.SaveChangesAsync();
                return chat;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


    }
}
