using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OfflineMessaging.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastLoginDate { get; set; }


        public ICollection<User> Friends { get; set; }
        public ICollection<Chat> Converstaions { get; set; }
    }
}