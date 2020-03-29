using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MessagingWebApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastLoginDate { get; set; }

        //public virtual ICollection<UserRelationship> Friends { get; set; }
        public virtual ICollection<Chat> Chats { get; set; }

        public User()
        {
            this.Chats = new HashSet<Chat>();
        }
    }
}