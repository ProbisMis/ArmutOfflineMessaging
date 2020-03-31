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
    [Route("chat")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly IUserService _userService;
        private readonly IMessageService _messageService;
        public ChatController(IChatService chatService, IUserService userService, IMessageService messageService)
        {
            _chatService = chatService;
            _userService = userService;
            _messageService = messageService;
        }
        public async Task<IActionResult> CreateChat([FromBody] FriendRequestDto request)
        {
            //TODO: Correct return types add logging
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

                    var chatResponse =  _chatService.GetChat(userModel, friendModel);
                    ChatResponseModel chat;
                    if (chatResponse.chat == null)
                    {
                        chat = _chatService.CreateChat(userModel, friendModel);
                        if (chat.UserFriendly)
                        {
                            return BadRequest(chat);
                        }
                        else
                        {
                            return Ok();
                        }
                    }

                    return Ok(user);
                }
                return BadRequest(SystemCustomerFriendlyMessages.InvalidModel);
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }

        }

        [HttpGet("{user_id}/{friend_id}")]
        public async Task<IActionResult> GetChat(int user_id, int friend_id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user =  _userService.GetUserById(user_id);
                    var friend =  _userService.GetUserById(friend_id);
                    if (user.UserFriendly) return BadRequest(user);
                    if (friend.UserFriendly) return BadRequest(friend);

                    var isFriend = await _userService.IsFriend(user.user.Id, friend.user.Id);
                    if (!isFriend) return BadRequest(SystemCustomerFriendlyMessages.FriendNotFound);

                    ChatResponseModel chat =  _chatService.GetChat(user.user, friend.user);
                    await _messageService.ReadMessage(user.user, friend.user);
                    return Ok(chat.chat);
                }
                return BadRequest("Invalid Model");
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }

        }

        [HttpGet("{user_id}")]
        public async Task<IActionResult> GetAllChats(int user_id)
        {
            //TODO: Correct return types add logging
            try
            {
                if (ModelState.IsValid)
                {
                    var user =  _userService.GetUserById(user_id);
                    if (user.UserFriendly) return BadRequest(user);

                    List<Chat> chat = await _chatService.GetAllChats(user.user);
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