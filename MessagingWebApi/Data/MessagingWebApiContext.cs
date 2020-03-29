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

        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<UserRelationship> UserRelationships { get; set; }
    }
}
