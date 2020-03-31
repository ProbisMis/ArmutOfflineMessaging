using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessagingWebApi.Data;
using MessagingWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MessagingWebApi.Services
{
    public class MessageService : IMessageService
    {
        private readonly MessagingWebApiContext _context;
        private readonly ILogger<MessageService> _logger;
        public MessageService(MessagingWebApiContext context, ILogger<MessageService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public  ChatResponseModel InsertMessage(User sender, User reciever, string body, Chat chat)
        {
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

                _logger.LogInformation(string.Format(SystemCustomerFriendlyMessages.UserMessageSend, sender.Id, reciever.Id));
                _ = _context.SaveChangesAsync();

                return new ChatResponseModel() { chat = chat};
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Exception:  {0}", ex));
                return new ChatResponseModel()
                {
                    UserFriendly = false,
                    Message = string.Format("Exception:  {0}", ex),
                    ErrorCode = "500",
                };
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
