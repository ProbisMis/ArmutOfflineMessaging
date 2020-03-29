using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MessagingWebApi.Models;

namespace MessagingWebApi.Data
{
    public class MessagingWebApiContext : DbContext
    {
        public MessagingWebApiContext (DbContextOptions<MessagingWebApiContext> options)
            : base(options)
        {
        }

        public DbSet<MessagingWebApi.Models.User> Users { get; set; }
        public DbSet<MessagingWebApi.Models.Message> Messages { get; set; }
        public DbSet<MessagingWebApi.Models.Chat> Chats { get; set; }
    }
}
