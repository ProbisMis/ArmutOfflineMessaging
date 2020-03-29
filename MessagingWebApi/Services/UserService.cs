using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessagingWebApi.Data;
using MessagingWebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace MessagingWebApi.Services
{
    public class UserService : ControllerBase, IUserService
    {
        private readonly MessagingWebApiContext _context;
        public UserService(MessagingWebApiContext context)
        {
            _context = context;
        }

        public  async Task<User> InsertUser(User user)
        {
            var result = _context.Users.Where(x => x.Username == user.Username).ToList();

            if (result != null && result.Count == 0) return null;

            //hash password 
            user.CreatedDate = DateTime.Now;
            user.LastLoginDate = DateTime.Now;
            _context.Add(user);
            await _context.SaveChangesAsync();

            return user;
          
        }

        public async Task<User> UpdateUser(User user)
        {
            var result = _context.Users.Where(x => x.Username == user.Username).ToList();

            if (result != null && result.Count == 0) return null;

            _context.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User> GetUserById(int userId)
        {
            var result = _context.Users.Where(x => x.Id == userId).FirstOrDefault();

            if (result != null) return null;

            return result;
        }

        public async Task<User> GetUserByUsername(string username)
        {
            var result = _context.Users.Where(x => x.Username == username).FirstOrDefault();

            if (result == null) return null;

            return result;
        }

        public async Task<List<User>> GetAllUsers()
        {
            var result = _context.Users.ToList();

            if (result == null) return null;

            return result;
        }

        public async Task<User> CheckUsernamePassword(string username, string password)
        {
            var result = _context.Users.Where(x => x.Username == username && x.Password == password).FirstOrDefault();

            if (result == null) return null;

            return result;
        }

        public async Task<User> InsertFriend(User user, User friend)
        {
            try
            {
                var result = _context.Users.Where(x => x.Username == user.Username).ToList();

                if (result != null && result.Count == 0) return null;

                if (user.Friends == null)
                    user.Friends = new List<User>();

                user.Friends.Add(friend);
                _context.Update(user);
                await _context.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {

                throw ex;
            }
           

        }

        public async Task<bool> IsFriend(int userId,int friendId)
        {
            try
            {
                var currentUser = _context.Users.Where(x => x.Id == userId).First();
                var isFriend = currentUser.Friends.Select(z => z.Id == friendId).First();

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }


    }
}
