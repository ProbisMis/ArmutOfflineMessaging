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

            if (result != null && result.Count > 0) return null;

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
            _context.Entry(user).Collection(s => s.Chats).Load();
            _context.Update(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User> GetUserById(int userId)
        {
            var result = _context.Users.Where(x => x.Id == userId).FirstOrDefault();
            return result;
        }

        public async Task<User> GetUserByUsername(string username)
        {
            var result = _context.Users.Where(x => x.Username == username).FirstOrDefault();
            return result;
        }

        public async Task<List<User>> GetAllUsers()
        {
            var result = _context.Users.ToList();
            return result;
        }

        public async Task<List<User>> GetFriends(int id)
        {
            var relations = _context.UserRelationships.Where(x => (x.FriendId == id || x.UserId == id)).ToList(); //TODO: Çok kötü..
            var distinctUsers = new List<User>();
            foreach (var relation in relations)
            {
                User tempUser;
                if (relation.UserId == id)
                {
                    tempUser = await GetUserById(relation.FriendId);
                    distinctUsers.Add(tempUser);
                }
                else
                {
                    tempUser = await GetUserById(relation.UserId);
                    distinctUsers.Add(tempUser);
                }
            }

            return distinctUsers;
        }
        public async Task<User> CheckUsernamePassword(string username, string password)
        {
            var result = _context.Users.Where(x => x.Username == username && x.Password == password).FirstOrDefault();
            return result;
        }

        public async Task<User> InsertFriend(User user, User friend)
        {
            try
            {
                var result = _context.Users.Where(x => x.Username == user.Username).ToList();

                if (result != null && result.Count == 0) return null;

                User firstUser = user, secondUser = friend;
                CalculateFirstUser(user, friend, out firstUser, out secondUser);
              
                var relation = new UserRelationship()
                {
                    UserId = firstUser.Id,
                    FriendId = secondUser.Id
                };

                _context.Add(relation);
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
                var isFriend = false;
                if (IsFirstUser(userId,friendId))
                     isFriend = _context.UserRelationships.Select(z => z.UserId == userId).First();
                else
                     isFriend = _context.UserRelationships.Select(z => z.UserId == friendId).First();
                
                return isFriend;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> IsBlocked(int userId, int friendId)
        {
            try
            {
                UserRelationship relation;
                if (IsFirstUser(userId, friendId))
                    relation = _context.UserRelationships.Where(z => z.UserId == userId).First();
                else
                    relation = _context.UserRelationships.Where(z => z.UserId == friendId).First();
                
                if (relation.IsBlocked)
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<string> BlockFriend(User user, User friend)
        {

            try
            {
                User user1, user2;
                CalculateFirstUser(user, friend, out user1, out user2);
                var relation = _context.UserRelationships.Where(x => x.UserId == user1.Id).FirstOrDefault();
                relation.IsBlocked = true;
                _context.Update(relation);
                await _context.SaveChangesAsync();
                return "success";
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
          
        }

        public bool IsFirstUser(int userId , int friendId)
            {

                if (userId < friendId)
                    return true;
                else
                    return false;
            }

        public void CalculateFirstUser(User user, User friend, out User firstUser, out User secondUser)
        {

            if (user.Id < friend.Id)
            {
                firstUser = user;
                secondUser = friend;
            }
            else
            {
                firstUser = friend;
                secondUser = user;
            }
        }


    }
}
