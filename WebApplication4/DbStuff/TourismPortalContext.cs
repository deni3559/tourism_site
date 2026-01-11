using System.Reflection;
using Microsoft.EntityFrameworkCore;
using WebPortal.DbStuff.Models;
using WebPortal.DbStuff.Models.Tourism;

namespace WebPortal.DbStuff
{
    public class TourismPortalContext : DbContext
    {
        public TourismPortalContext(DbContextOptions<TourismPortalContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Tours> Tours { get; set; }
        public DbSet<NextArticlePreview> NextArticlePreview { get; set; }
        public DbSet<ShopCart> ShopCart {  get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            modelBuilder
                .Entity<Tours>()
                .HasMany(x => x.ToursInShopCart)
                .WithOne(x => x.TourInShop)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder
               .Entity<User>()
               .HasMany(user => user.CreatedTours)
               .WithOne(tour => tour.Author)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder
             .Entity<User>()
             .HasMany(user => user.AddedToursInShopCart)
             .WithOne(shop => shop.UserWhoAddTheTour)
             .HasForeignKey (k => k.UserId)
             .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
