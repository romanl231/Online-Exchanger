using Exchanger.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Exchanger.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<SessionToken> SessionTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SessionToken>()
                .HasOne(t => t.User)
                .WithMany(u => u.SessionTokens)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
