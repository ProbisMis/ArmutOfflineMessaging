﻿using System;
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
                    var user = await _userService.GetUserById(request.user_id);
                    var friend = await _userService.GetUserById(request.friend_id);
                    if (user == null) return BadRequest("User does not exist");
                    if (friend == null) return BadRequest("User does not exist");

                    var isFriend = await _userService.IsFriend(user.Id, friend.Id);
                    if (!isFriend) return BadRequest("Be friend first");

                    var foundChat = await _chatService.GetChat(user, friend);

                    /* Read status update */

                    if (foundChat == null)
                    {
                        //FirstId
                        if (user.Id < friend.Id)
                            await _chatService.CreateChat(user, friend);
                        else
                            await _chatService.CreateChat(friend, user);
                    }

                    return Ok(user);
                }
                return BadRequest("Invalid Model");
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }

        }

        [HttpGet("{user_id}/{friend_id}")]
        public async Task<IActionResult> GetChat(int user_id, int friend_id)
        {
            //TODO: Correct return types add logging
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userService.GetUserById(user_id);
                    var friend = await _userService.GetUserById(friend_id);
                    if (user == null) return BadRequest("User does not exist");
                    if (friend == null) return BadRequest("User does not exist");

                    var isFriend = await _userService.IsFriend(user.Id, friend.Id);
                    if (!isFriend) return BadRequest("You are no longer friend! ");

                    Chat chat = await _chatService.GetChat(user, friend);
                    await _messageService.ReadMessage(user, friend);
                    return Ok(chat);
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
                    var user = await _userService.GetUserById(user_id);
                    if (user == null) return BadRequest("User does not exist");

                    List<Chat> chat = await _chatService.GetAllChats(user);
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