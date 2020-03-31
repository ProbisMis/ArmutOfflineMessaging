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
        public UserResponseModel InsertUser(User user);
        public UserResponseModel UpdateUser(User user);
        public UserResponseModel GetUserById(int userId);
        public UserResponseModel GetUserByUsername(string username);
        public  Task<List<User>> GetAllUsers();
        public UserResponseModel CheckUsernamePassword(string username, string password);
        public UserResponseModel InsertFriend(User user, User friend);
        public  Task<bool> IsFriend(int userId, int friendId);
        public Task<List<User>> GetFriends(int id);
        public UserResponseModel BlockFriend(User user, User friend);
        public Task<bool> IsBlocked(int userId, int friendId);
        public void CalculateFirstUser(User user, User friend, out User firstUser, out User secondUser);
    }
}
