using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MessagingWebApi.Models;
using MessagingWebApi.Services;

namespace MessagingWebApiTest.Services
{
    class FakeUserService : IUserService
    {

        private readonly List<User> _users;

        private readonly List<UserRelationship> _userRelationships;

        public FakeUserService()
        {
            _users = new List<User>()
            {
                new User() { Id = 1,
                    Username = "test1", Password="123",CreatedDate = DateTime.Now },
                new User() { Id = 2,
                    Username = "test2", Password="123", CreatedDate = DateTime.Now  },
                new User() { Id = 3,
                    Username = "test3", Password="123",CreatedDate = DateTime.Now  }
            };

            _userRelationships = new List<UserRelationship>()
            {
                new UserRelationship() { Id = 1,
                    UserId = 1, FriendId=2, IsBlocked =false },
            };
        }

        public UserResponseModel BlockFriend(User user, User friend)
        {
            throw new NotImplementedException();
        }

        public void CalculateFirstUser(User user, User friend, out User firstUser, out User secondUser)
        {
            throw new NotImplementedException();
        }

        public UserResponseModel CheckUsernamePassword(string username, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<List<User>> GetAllUsers()
        {
            return _users;
        }

        public Task<List<User>> GetFriends(int id)
        {
            throw new NotImplementedException();
        }

        public UserResponseModel GetUserById(int userId)
        {
            return new UserResponseModel() { user = _users.Find(x => x.Id == userId) };
        }

        public UserResponseModel GetUserByUsername(string username)
        {
            return new UserResponseModel() { user = _users.Find(x => x.Username == username) };
        }

        public UserResponseModel InsertFriend(User user, User friend)
        {
            throw new NotImplementedException();
        }

        public UserResponseModel InsertUser(User user)
        {
            _users.Add(user);
            return new UserResponseModel() { user = _users.Find(x => x.Username == user.Username) };
        }

        public Task<bool> IsBlocked(int userId, int friendId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsFriend(int userId, int friendId)
        {
            throw new NotImplementedException();
        }

        public UserResponseModel UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
