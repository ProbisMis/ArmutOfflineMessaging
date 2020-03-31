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
                    var user =  _userService.GetUserById(request.SenderId);
                    var friend =  _userService.GetUserById(request.RecieverId);
                    var userModel = user.user;
                    var friendModel = friend.user;
                    if (user.UserFriendly) return BadRequest(user);
                    if (friend.UserFriendly) return BadRequest(friend);

                    var isFriend = await _userService.IsFriend(request.SenderId, request.RecieverId);
                    if (!isFriend) return BadRequest(SystemCustomerFriendlyMessages.FriendNotFound);

                    var isBlocked = await _userService.IsBlocked(request.SenderId, request.RecieverId);
                    if (isBlocked) return Ok(); //TODO: User should not see this!

                    ChatResponseModel chatResponse =  _chatService.GetChat(userModel, friendModel);
                    if (chatResponse.chat == null)
                        chatResponse = _chatService.CreateChat(userModel, friendModel);
                    var chat = chatResponse.chat;

                    if (chatResponse.UserFriendly) return BadRequest(chat);
                    
                    var message =  _messageService.InsertMessage(userModel, friendModel, request.MessageBody,  chat);
                    
                    //TODO: Uncomment if messages are not updated
                    //chat.Messages.Add(message);

                    //var chatResponse  =  _chatService.UpdateChat(chat);
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