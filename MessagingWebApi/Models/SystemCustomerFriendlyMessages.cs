using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessagingWebApi.Models
{
    public static partial class SystemCustomerFriendlyMessages
    {
        public static string UsernamePasswordMatchError { get { return "Username and password does not match"; } }

        public static string FriendNotFound { get { return "You are no longer friends"; } }
        public static string UserFound { get { return "User: {0} already exist"; } }
        public static string UserLogin { get { return "User: {0} logged in"; } }
        public static string UserRegister { get { return "User: {0} registered"; } }
        public static string UserMessageSend { get { return "User: {0} sent message to User: {1}"; } }
        public static string ChatCreate { get { return "Chat: {0} created"; } }
        public static string UserNotFound { get { return "User: {0} does not exist"; } }
        public static string UserRelationshipAdd{ get { return "User: {0} and User: {1} became friends"; } }
        public static string UserRelationshipBlock { get { return "User: {0} blocked  User: {1} "; } }
        public static string UserBlocked { get { return "Blocked"; } }
        public static string InvalidModel { get { return "Invalid Model"; } }
        public static string AlreadyFriend { get { return "You are already friend"; } }
        public static string ChatNotFound { get { return "No associated chat found"; } }



    }
}
