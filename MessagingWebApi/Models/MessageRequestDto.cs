using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessagingWebApi.Models
{
    public class MessageRequestDto
    {
        public string MessageBody { get; set; }
        public int SenderId { get; set; }
        public int RecieverId { get; set; }
    }
}
