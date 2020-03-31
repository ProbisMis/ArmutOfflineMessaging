using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessagingWebApi.Data;
using MessagingWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MessagingWebApi.Services
{
    public class UserService : ControllerBase, IUserService
    {
        private readonly MessagingWebApiContext _context;
        private readonly ILogger<UserService> _logger;
        public UserService(MessagingWebApiContext context, ILogger<UserService> logger)
        {
            _context = context;
            _logger = logger;

            _context.Users
                   .Include(b => b.Chats)
                   .ToList();
        }

        public UserResponseModel InsertUser(User user)
        {
            try
            {
                var result = _context.Users.Where(x => x.Username == user.Username).ToList();

                if (result != null && result.Count > 0)
                {
                    _logger.LogWarning(string.Format(SystemCustomerFriendlyMessages.UserFound, user.Username));
                    return new UserResponseModel()
                    {
                        UserFriendly = true,
                        Message = string.Format(SystemCustomerFriendlyMessages.UserFound, user.Username),
                        ErrorCode = "400",
                    };
                }
                //hash password 
                _logger.LogInformation(string.Format(SystemCustomerFriendlyMessages.UserRegister, user.Id));
                user.CreatedDate = DateTime.Now;
                user.LastLoginDate = DateTime.Now;
                _ = _context.Add(user);
                _ = _context.SaveChangesAsync();
                var newUser = _context.Users.Where(x => x.Username == user.Username).First();
                return new UserResponseModel() { user = newUser };
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Exception:  {0}", ex));
                return new UserResponseModel()
                {
                    UserFriendly = false,
                    Message = string.Format("Exception:  {0}", ex),
                    ErrorCode = "500",
                };
            }
        }

        public  UserResponseModel UpdateUser(User user)
        {
            try
            {
                var result = _context.Users.Where(x => x.Username == user.Username).First();

                if (result == null)
                {
                    _logger.LogWarning(string.Format(SystemCustomerFriendlyMessages.UserNotFound, user.Id));
                    return new UserResponseModel()
                    {
                        UserFriendly = false,
                        Message = string.Format(SystemCustomerFriendlyMessages.UserNotFound, user.Id),
                        ErrorCode = "400",
                    };
                }

                _logger.LogInformation(string.Format(SystemCustomerFriendlyMessages.UserLogin, user.Id));
                _context.Entry(user).Collection(s => s.Chats).Load();
                _context.Update(user); 
                _ = _context.SaveChangesAsync();

                return new UserResponseModel() { user = user };
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Exception:  {0}", ex));
                return new UserResponseModel()
                {
                    UserFriendly = false,
                    Message = string.Format("Exception:  {0}", ex),
                    ErrorCode = "500",
                };
            }
            
        }

        public UserResponseModel GetUserById(int userId)
        {
            try
            {
                var result = _context.Users.Where(x => x.Id == userId).First();

                if (result == null)
                {
                    _logger.LogWarning(string.Format(SystemCustomerFriendlyMessages.UserNotFound, userId));
                    return new UserResponseModel()
                    {
                        UserFriendly = false,
                        Message = string.Format(SystemCustomerFriendlyMessages.UserNotFound, userId),
                        ErrorCode = "400",
                    };
                }
                return new UserResponseModel() { user = result };
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Exception:  {0}", ex));
                return new UserResponseModel()
                {
                    UserFriendly = false,
                    Message = string.Format("Exception:  {0}", ex),
                    ErrorCode = "500",
                };
            }

        }

        public UserResponseModel GetUserByUsername(string username)
        {
            try
            {
                var result = _context.Users.Where(x => x.Username == username).FirstOrDefault();
                if (result == null)
                {
                    _logger.LogWarning(string.Format(SystemCustomerFriendlyMessages.UserNotFound, username));
                    return new UserResponseModel()
                    {
                        UserFriendly = false,
                        Message = string.Format(SystemCustomerFriendlyMessages.UserNotFound, username),
                        ErrorCode = "400",
                    };
                }

                return new UserResponseModel() { user = result };
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Exception:  {0}", ex));
                return new UserResponseModel()
                {
                    UserFriendly = false,
                    Message = string.Format("Exception:  {0}", ex),
                    ErrorCode = "500",
                };
            }
           
        }

        public async Task<List<User>> GetAllUsers()
        {
            var result = _context.Users.ToList();
            return result;
        }

        public async Task<List<User>> GetFriends(int id)
        {
            //TODO: Check GetUserById return 
            var relations = _context.UserRelationships.Where(x => (x.FriendId == id || x.UserId == id)).ToList(); //TODO: Çok kötü..
            var distinctUsers = new List<User>();
            foreach (var relation in relations)
            {
                UserResponseModel tempUser;
                if (relation.UserId == id)
                {
                    tempUser =  GetUserById(relation.FriendId);
                    distinctUsers.Add(tempUser.user);
                }
                else
                {
                    tempUser =  GetUserById(relation.UserId);
                    distinctUsers.Add(tempUser.user);
                }
            }

            return distinctUsers;
        }
        public UserResponseModel CheckUsernamePassword(string username, string password)
        {
            try
            {
                var result = _context.Users.Where(x => x.Username == username && x.Password == password).FirstOrDefault();
               
                if (result == null)
                {
                    _logger.LogWarning(SystemCustomerFriendlyMessages.UsernamePasswordMatchError);
                    return new UserResponseModel()
                    {
                        UserFriendly = true,
                        Message = SystemCustomerFriendlyMessages.UsernamePasswordMatchError,
                        ErrorCode = "400",
                    };
                }

                return new UserResponseModel() { user = result };
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Exception:  {0}", ex));
                return new UserResponseModel()
                {
                    UserFriendly = false,
                    Message = string.Format("Exception:  {0}", ex),
                    ErrorCode = "500",
                };
            }
            
        }

        public UserResponseModel InsertFriend(User user, User friend)
        {
            try
            {
                User firstUser = user, secondUser = friend;
                CalculateFirstUser(user, friend, out firstUser, out secondUser);
              
                var relation = new UserRelationship()
                {
                    UserId = firstUser.Id,
                    FriendId = secondUser.Id,
                    IsBlocked = false
                };
                _logger.LogInformation(string.Format(SystemCustomerFriendlyMessages.UserRelationshipAdd, user.Username, friend.Username));
                _context.Add(relation);
                _ = _context.SaveChangesAsync();
                return new UserResponseModel() { user = friend};
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Exception:  {0}", ex));
                return new UserResponseModel()
                {
                    UserFriendly = false,
                    Message = string.Format("Exception:  {0}", ex),
                    ErrorCode = "500",
                };
            }
        }

        public async Task<bool> IsFriend(int userId,int friendId)
        {
            try
            {
                var isFriend = false;
                if (IsFirstUser(userId,friendId))
                     isFriend = _context.UserRelationships.Any(z => z.UserId == userId && z.FriendId == friendId);
                else
                     isFriend = _context.UserRelationships.Any(z => z.UserId == friendId && z.FriendId == userId);
                
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
                    relation = _context.UserRelationships.Where(z => z.UserId == userId && z.FriendId == friendId)?.First();
                else
                    relation = _context.UserRelationships.Where(z => z.UserId == friendId && z.FriendId == userId)?.First();

                if (relation.IsBlocked)
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public UserResponseModel BlockFriend(User user, User friend)
        {
            try
            {
                User user1, user2;
                CalculateFirstUser(user, friend, out user1, out user2);
                var relation = _context.UserRelationships.Where(x => x.UserId == user1.Id && x.FriendId == user2.Id).First();

                if (relation == null)
                {
                    _logger.LogWarning(SystemCustomerFriendlyMessages.FriendNotFound);
                    return new UserResponseModel()
                    {
                        UserFriendly = true,
                        Message = SystemCustomerFriendlyMessages.FriendNotFound,
                        ErrorCode = "400",
                    };
                }

                _logger.LogInformation(string.Format(SystemCustomerFriendlyMessages.UserRelationshipBlock, user.Username, friend.Username));
                relation.IsBlocked = true;
                _context.Update(relation);
                 _ = _context.SaveChangesAsync();
                return new UserResponseModel() { user = friend };
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Exception:  {0}", ex));
                return new UserResponseModel()
                {
                    UserFriendly = false,
                    Message = string.Format("Exception:  {0}", ex),
                    ErrorCode = "500",
                };
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
