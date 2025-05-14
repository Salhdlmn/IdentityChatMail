using IdentityChatMailDay.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityChatMailDay.Context
{
    public class MailContext:IdentityDbContext<AppUser>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server = DESKTOP-M9MA5UL; initial Catalog=EmailChatDbDay; integrated Security= true; trust server certificate= true");
        }

        public DbSet<Message> Messages { get; set; }
    }
}
