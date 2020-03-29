using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessagingWebApi.Models;
using MessagingWebApi.Data;

namespace MessagingWebApi.Services
{
    public interface IUserService
    {
        public  Task<User> InsertUser(User user);
        public  Task<User> UpdateUser(User user);
        public  Task<User> GetUserById(int userId);
        public  Task<User> GetUserByUsername(string username);
        public  Task<List<User>> GetAllUsers();
        public  Task<User> CheckUsernamePassword(string username, string password);
        public  Task<User> InsertFriend(User user, User friend);
        public  Task<bool> IsFriend(int userId, int friendId);
    }
}
