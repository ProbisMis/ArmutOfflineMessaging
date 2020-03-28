using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MessagingWebApi.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public string ChatGuid { get; set; }
        public int SenderId { get; set; }
        public int RecieverId { get; set; }
        public DateTime CreatedDate{ get; set; }
        public bool IsBlocked { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
    }
}