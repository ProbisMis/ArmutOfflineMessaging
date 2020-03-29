using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MessagingWebApi.Models
{
    public class UserRelationship
    {
        [Key, Column(Order = 1)]
        public int UserId { get; set; }
        [Key, Column(Order = 2)]
        public int FriendId { get; set; }
        public User FirstUser { get; set; }
        public User SecondUser { get; set; }
        public bool IsBlocked { get; set; }
    }
}
