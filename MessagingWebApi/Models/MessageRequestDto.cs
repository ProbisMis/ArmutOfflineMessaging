using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessagingWebApi.Models
{
    public class MessageRequestDto
    {
        public string message_body { get; set; }
        public int user_id { get; set; }
        public int friend_id { get; set; }
    }
}
