using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MessagingWebApi.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public int SenderId { get; set; }
        public int RecieverId { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsRead { get; set; }
        public DateTime? ReadDate { get; set; }
        public bool IsDeleted { get; set; }
        public int ChatId { get; set; }


    }
}