﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MessagingWebApi.Data;
using MessagingWebApi.Models;
using MessagingWebApi.Services;

namespace MessagingWebApi.Controllers
{
    [ApiController]
    [Route("user")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;

       
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
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
                    var result = await _userService.InsertUser(user);

                    if (result != null)
                        return Ok(result);
                    else
                    {
                        return NotFound(typeof(User));
                    }
                    //Assert

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
                    var foundUser = _userService.GetUserByUsername(user.Username).Result;
                    if (foundUser == null) return NotFound("user not found");
                    var foundUser1 = _userService.CheckUsernamePassword(user.Username, user.Password).Result;
                    if (foundUser1 == null) return NotFound("username and Password does not match");

                    //var foundUser = _context.Users.Where(x => x.Password == user.Password && x.Username == user.Username).First();
                    //if (foundUser == null) return BadRequest("user Does not exist");

                    foundUser.LastLoginDate = DateTime.Now;
                    var updatedUser = await _userService.UpdateUser(foundUser);
                    return Ok(updatedUser);
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
                    var user =  await _userService.GetUserById(friendRequestDto.user_id);
                    var friend = await _userService.GetUserById(friendRequestDto.friend_id);
                    await _userService.BlockFriend(user,friend);
                    return Ok(user);
                }
                return BadRequest("Invalid Model");
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
                    var user = await _userService.GetUserById(request.user_id);
                    var friend = await _userService.GetUserById(request.friend_id);
                    if (user == null) return BadRequest("User does not exist");
                    if (friend == null) return BadRequest("User does not exist");
                    
                    var isFriend = await _userService.IsFriend(user.Id, friend.Id);
                    if (isFriend) return BadRequest("Already friends");
                   
                    var result = await _userService.InsertFriend(user, friend);

                    if (result != null)
                    {
                        return Ok(result);
                    }
                    else
                    {
                        return BadRequest("TODO: OPERATION FAILED");
                    }
                }
                return BadRequest("Invalid Model");
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
