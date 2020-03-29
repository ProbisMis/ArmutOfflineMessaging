using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MessagingWebApi.Data;
using MessagingWebApi.Models;

namespace MessagingWebApi.Controllers
{
    [ApiController]
    [Route("user")]
    public class UsersController : Controller
    {
        private readonly MessagingWebApiContext _context;

        public UsersController(MessagingWebApiContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return Ok(_context.Users.ToList());
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
                    var result = _context.Users.Where(x => x.Username == user.Username).ToList();

                    if (result != null && result.Count > 0) return BadRequest("User already exist");

                    //hash password 
                    user.CreatedDate = DateTime.Now;
                    user.LastLoginDate = DateTime.Now;
                    _context.Add(user);
                    await _context.SaveChangesAsync();
                    return Ok(user);
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
                    var result = _context.Users.Any(o => o.Username == user.Username);
                 
                    if (!result) return BadRequest("User does not exist");

                    var foundUser = _context.Users.Where(x => x.Password == user.Password && x.Username == user.Username).ToList();

                    if (foundUser == null && foundUser.Count == 0) return BadRequest("Password does not match");

                    //hash password 
                    foundUser.FirstOrDefault().LastLoginDate = DateTime.Now;
                    _context.Update(foundUser.FirstOrDefault());
                    await _context.SaveChangesAsync();
                    return Ok(foundUser);
                }
                return BadRequest("Invalid Model");
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }

        }

        // GET: Users/Friends
        [HttpGet("friends/{user_id}")]
        public async Task<IActionResult> Friends(int user_id)
        {
            //TODO: Correct return types add logging
            try
            {
                if (ModelState.IsValid)
                {
                    var user = _context.Users.Find(user_id);
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

        // POST: Users/Friends
        [HttpPost("friends")]
        public async Task<IActionResult> AddFriend([FromBody] FriendRequestDto request)
        {   

            //TODO: Correct return types add logging
            try
            {
                if (ModelState.IsValid)
                {
                    int user_id = request.user_id;
                    int friend_id = request.friend_id;

                    var user = _context.Users.Find(user_id);
                    if (user == null) return BadRequest("User does not exist");

                    var friend = _context.Users.Find(friend_id);
                    if (friend == null) return BadRequest("User does not exist");

                    var isFriend = _context.Users.Any(x => x.Friends.Any(z => z.Id == friend_id));

                    //Not a good place
                    if (user.Friends == null)
                        user.Friends = new List<User>();

                    user.Friends.Add(friend);
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                    return Ok(user);
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
