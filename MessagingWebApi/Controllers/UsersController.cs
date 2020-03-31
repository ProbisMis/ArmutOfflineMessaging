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
    

            _logger.LogInformation("UsersController.Index method called!!!");
            return Ok(await _userService.GetAllUsers());
        }

        // POST: Users/Create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] User user)
        {
            //TODO: Correct return types add logging
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

                return BadRequest(ex);
            }

        }

        // POST: Users/Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            //TODO: Correct return types add logging
            try
            {
                if (ModelState.IsValid)
                {
                    //var result = _context.Users.Any(o => o.Username == user.Username);
                    //if (!result) return BadRequest("User does not exist");
                    var response = _userService.GetUserByUsername(user.Username);
                    if (response.UserFriendly) return NotFound(response);
                    var response1 = _userService.CheckUsernamePassword(user.Username, user.Password);
                    if (response1.UserFriendly) return NotFound(response1);

                    //var foundUser = _context.Users.Where(x => x.Password == user.Password && x.Username == user.Username).First();
                    //if (foundUser == null) return BadRequest("user Does not exist");

                    response.user.LastLoginDate = DateTime.Now;
                    var updatedUser = _userService.UpdateUser(response.user);
                    
                    return Ok(updatedUser.user);
                }
                return BadRequest("Invalid Model");
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }

        }

        // GET: Users/Friends
        [HttpGet("friends/{userId}")]
        public async Task<IActionResult> Friends(int userId)
        {
            //TODO: Correct return types add logging
            try
            {
                if (ModelState.IsValid)
                {
                    var user = _userService.GetFriends(userId).Result;
                    if (user == null) return BadRequest("User does not exist");

                    return Ok(user);
                }
                return BadRequest("Invalid Model");
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }

        }

        // GET: Users/Friends
        [HttpPost("block")]
        public async Task<IActionResult> BlockFriend([FromBody] FriendRequestDto friendRequestDto)
        {
            //TODO: Correct return types add logging
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

                return BadRequest(ex);
            }

        }

        // POST: Users/Friends
        [HttpPost("friends")]
        public async Task<IActionResult> AddFriend([FromBody] FriendRequestDto request)
        {   

            //TODO: Correct return types add logging
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

                    if (result.UserFriendly)
                    {
                        return Ok(result);
                    }
                    else
                    {
                        //TODO: Should not be returned to user, already logged to DB, just send internal server error
                        return StatusCode(500);
                    }
                }
                return BadRequest(SystemCustomerFriendlyMessages.InvalidModel);
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }

        }

        //// GET: Users/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var user = await _context.Users.FindAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(user);
        //}

        //// POST: Users/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Username,Password,CreatedDate,LastLoginDate")] User user)
        //{
        //    if (id != user.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(user);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!UserExists(user.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(user);
        //}

        //// GET: Users/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var user = await _context.Users
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(user);
        //}

        //// POST: Users/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var user = await _context.Users.FindAsync(id);
        //    _context.Users.Remove(user);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool UserExists(int id)
        //{
        //    return _context.Users.Any(e => e.Id == id);
        //}
    }
}
