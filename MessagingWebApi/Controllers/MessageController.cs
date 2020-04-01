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
using Microsoft.Extensions.Logging;

namespace MessagingWebApi.Controllers
{
    [Route("message")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IChatService _chatService;
        private readonly IMessageService _messageService;
        private readonly ILogger<MessageController> _logger;
        public MessageController(IChatService chatService, IUserService userService, IMessageService messageService, ILogger<MessageController> logger)
        {
            _chatService = chatService;
            _userService = userService;
            _messageService = messageService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage([FromBody] MessageRequestDto request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user =  _userService.GetUserById(request.user_id);
                    var friend =  _userService.GetUserById(request.friend_id);
                    var userModel = user.user;
                    var friendModel = friend.user;
                    if (user.UserFriendly) return BadRequest(user);
                    if (friend.UserFriendly) return BadRequest(friend);

                    var isFriend = await _userService.IsFriend(request.user_id, request.friend_id);
                    if (!isFriend) return BadRequest(SystemCustomerFriendlyMessages.FriendNotFound);

                    var isBlocked = await _userService.IsBlocked(request.user_id, request.friend_id);
                    if (isBlocked) return Ok(); 

                    ChatResponseModel chatResponse =  _chatService.GetChat(userModel, friendModel);
                    if (chatResponse.chat == null)
                        chatResponse = _chatService.CreateChat(userModel, friendModel);
                    var chat = chatResponse.chat;

                    if (chatResponse.UserFriendly) return BadRequest(chat);
                    
                    var message =  _messageService.InsertMessage(userModel, friendModel, request.message_body,  chat);
                    
                    //TODO: Uncomment if messages are not updated
                    //chat.Messages.Add(message);

                    //var chatResponse  =  _chatService.UpdateChat(chat);
                    return Ok(chat);
                }
                return BadRequest("Invalid Model");
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Exception: {0}", ex));
                return StatusCode(500);
            }

        }

    }
}