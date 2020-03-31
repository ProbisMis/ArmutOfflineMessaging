using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessagingWebApi.Data;
using MessagingWebApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MessagingWebApi.Services
{
    public class ChatService : IChatService
    {
        private readonly MessagingWebApiContext _context;
        private readonly ILogger<ChatService> _logger;
        public ChatService(MessagingWebApiContext context, ILogger<ChatService> logger)
        {
            _context = context;
            _logger = logger;

            //Lazy load
            _context.Chats
                        .Include(b => b.Messages)
                        .ToList();
        }

        public ChatResponseModel GetChat(User sender, User reciever)
        {
            if (_context.Chats.Count() == 0) return null;

            try
            {
                var isChatInitiliazed = _context.Chats.Where(
                       x =>
                       (
                           (x.RecieverId == reciever.Id && x.SenderId == sender.Id) ||
                           (x.RecieverId == sender.Id && x.SenderId == reciever.Id))
                       )?.First();

                if (isChatInitiliazed == null)
                {
                    _logger.LogWarning(SystemCustomerFriendlyMessages.ChatNotFound);
                    return new ChatResponseModel()
                    {
                        UserFriendly = false,
                        Message = SystemCustomerFriendlyMessages.ChatNotFound,
                        ErrorCode = "400",
                    };
                }
                _context.Entry(isChatInitiliazed)
                    .Collection(s => s.Messages)
                    .Load(); //TODO: check if needed

                return new ChatResponseModel() { chat = isChatInitiliazed };
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

        public ChatResponseModel CreateChat(User sender, User reciever)
        {
            try
            {
                //TODO: Burda oluştururken kıyasla ID'leri
                User firstUser; User secondUser;
                if (sender.Id < reciever.Id)
                {
                    firstUser = sender;
                    secondUser = reciever;
                }
                else
                {
                    firstUser = reciever;
                    secondUser = sender;
                }

                var chat = _context.Add(new Chat()
                {
                    ChatGuid = firstUser.Username + secondUser.Username,
                    SenderId = firstUser.Id,
                    RecieverId = secondUser.Id,
                    CreatedDate = DateTime.Now,
                    IsBlocked = false
                }).Entity;

                _logger.LogInformation(string.Format(SystemCustomerFriendlyMessages.ChatCreate, chat.Id));
                sender.Chats.Add(chat);
                reciever.Chats.Add(chat);
                _context.Update(firstUser);
                _context.Update(secondUser);
                _ = _context.SaveChangesAsync();

                return new ChatResponseModel() { chat = chat };
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

        public ChatResponseModel UpdateChat(Chat chat)
        {
            try
            {
                var updatedChat = _context.Update(chat).Entity;
                _ = _context.SaveChangesAsync();

                return new ChatResponseModel() { chat = updatedChat};
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


    }
}
