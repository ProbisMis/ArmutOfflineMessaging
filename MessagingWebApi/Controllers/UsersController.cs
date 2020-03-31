using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MessagingWebApi.Data;
using MessagingWebApi.Models;
using MessagingWebApi.Services;
using NLog.Extensions.Logging;
using Microsoft.Extensions.Logging;
using NLog;

namespace MessagingWebApi.Controllers
{
    [ApiController]
    [Route("user")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return Ok("Application Started!");
        }

        // POST: Users/Create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result =  _userService.InsertUser(user);

                    if (result.UserFriendly)
                        return BadRequest(result);
                    else
                        return Ok(result.user);
                }
                return BadRequest("Invalid Model");
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Exception: {0}", ex));
                return StatusCode(500);
            }

        }

        // POST: Users/Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = _userService.GetUserByUsername(user.Username);
                    if (response.UserFriendly) return NotFound(response);
                    var response1 = _userService.CheckUsernamePassword(user.Username, user.Password);
                    if (response1.UserFriendly) return NotFound(response1);

                    response.user.LastLoginDate = DateTime.Now;
                    var updatedUser = _userService.UpdateUser(response.user);
                    
                    return Ok(updatedUser.user);
                }
                return BadRequest(SystemCustomerFriendlyMessages.InvalidModel);
            }
            catch (Exception ex)
            {

                _logger.LogError(string.Format("Exception: {0}", ex));
                return StatusCode(500);
            }

        }

        // GET: Users/Friends
        [HttpGet("friends/{userId}")]
        public async Task<IActionResult> Friends(int userId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = _userService.GetFriends(userId).Result;
                    if (user == null) return BadRequest(SystemCustomerFriendlyMessages.UserNotFound);

                    return Ok(user);
                }
                return BadRequest(SystemCustomerFriendlyMessages.InvalidModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Exception: {0}", ex));
                return StatusCode(500);
            }

        }

        // GET: Users/Friends
        [HttpPost("block")]
        public async Task<IActionResult> BlockFriend([FromBody] FriendRequestDto friendRequestDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user =   _userService.GetUserById(friendRequestDto.user_id);
                    var friend =  _userService.GetUserById(friendRequestDto.friend_id);
                    var blocked=  _userService.BlockFriend(user.user,friend.user);

                    if(blocked.UserFriendly)
                    {
                        return Ok(blocked);
                    }
                    return Ok(blocked.user);
                }
                return BadRequest(SystemCustomerFriendlyMessages.InvalidModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Exception: {0}", ex));
                return StatusCode(500);
            }

        }

        // POST: Users/Friends
        [HttpPost("friends")]
        public async Task<IActionResult> AddFriend([FromBody] FriendRequestDto request)
        {   
            try
            {
                if (ModelState.IsValid)
                {
                    var user =  _userService.GetUserById(request.user_id);
                    var friend =  _userService.GetUserById(request.friend_id);
                    if (user.UserFriendly) return BadRequest(user);
                    if (friend.UserFriendly) return BadRequest(friend);
                    
                    var isFriend = await _userService.IsFriend(request.user_id, request.friend_id);
                    if (isFriend) return BadRequest(SystemCustomerFriendlyMessages.AlreadyFriend);
                   
                    var result =  _userService.InsertFriend(user.user, friend.user);
                    return Ok();

                }
                return BadRequest(SystemCustomerFriendlyMessages.InvalidModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Exception: {0}", ex));
                return StatusCode(500);
            }
        }
    }
}
