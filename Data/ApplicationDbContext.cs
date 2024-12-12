using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TicketingSystem.Models;

namespace TicketingSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // جداول سفارشی
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketResponse> TicketResponses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // تعریف روابط و تنظیمات
            builder.Entity<TicketResponse>()
                .HasOne(tr => tr.Ticket)
                .WithMany(t => t.Responses)
                .HasForeignKey(tr => tr.TicketId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
