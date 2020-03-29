using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MessagingWebApi.Data;
using MessagingWebApi.Models;
using MessagingWebApi.Data;
using MessagingWebApi.Models;
using MessagingWebApi.Services;

namespace MessagingWebApi.Controllers
{
    [Route("message")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IChatService _chatService;
        private readonly IMessageService _messageService;
        public MessageController(IChatService chatService, IUserService userService, IMessageService messageService)
        {
            _chatService = chatService;
            _userService = userService;
            _messageService = messageService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage([FromBody] MessageRequestDto request)
        {
            //TODO: Correct return types add logging
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userService.GetUserById(request.SenderId);
                    var friend = await _userService.GetUserById(request.RecieverId);
                    if (user == null) return BadRequest("User does not exist");
                    if (friend == null) return BadRequest("User does not exist");

                    var isFriend = await _userService.IsFriend(user.Id, friend.Id);
                    if (!isFriend) return BadRequest("You are no longer friends");

                    Chat chat = await _chatService.GetChat(user, friend);
                    
                    if (chat == null)
                    {
                        chat = await _chatService.CreateChat(user, friend);
                    }
             
                    var message = await _messageService.InsertMessage(user, friend, request.MessageBody);
                    chat.Messages.Add(message);

                    await _chatService.UpdateChat(chat);
                    return Ok(chat);
                }
                return BadRequest("Invalid Model");
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }

        }

    }
}