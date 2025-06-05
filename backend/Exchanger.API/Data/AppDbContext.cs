using Exchanger.API.Entities;
using Exchanger.API.Enums.Category;
using Microsoft.EntityFrameworkCore;

namespace Exchanger.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<SessionToken> SessionTokens { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Listing> Listing { get; set; }
        public DbSet<ListingCategory> ListingCategories { get; set; }
        public DbSet<ListingImages> ListingImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SessionToken>()
                .HasOne(t => t.User)
                .WithMany(u => u.SessionTokens)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Name)
                .IsUnique();
            modelBuilder.Entity<Category>().HasData(
                Enum.GetValues(typeof(CategorySeed))
                    .Cast<CategorySeed>()
                    .Select(e => new Category
                    {
                        Id = (int)e,
                        Name = e.ToString()
                    })
            );

            modelBuilder.Entity<Listing>()
                .HasOne(l => l.User)
                .WithMany(u => u.Listings)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Listing>()
                .HasIndex(l => l.Title);

            modelBuilder.Entity<ListingCategory>()
                .HasKey(lc => new { lc.ListingId, lc.CategoryId });
            modelBuilder.Entity<ListingCategory>()
                .HasOne(lc => lc.Listing)
                .WithMany(l => l.Categories)
                .HasForeignKey(lc => lc.ListingId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ListingCategory>()
                .HasOne(lc => lc.Category)
                .WithMany()
                .HasForeignKey(lc => lc.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ListingImages>()
                .HasOne(li => li.Listing)
                .WithMany(l => l.Images)
                .HasForeignKey(li => li.ListingId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
