using FuelMarketplace.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace FuelMarketplace.Infrastructure.DataAccess
{
    public class MarketplaceContext : DbContext
    {
        public MarketplaceContext(DbContextOptions<MarketplaceContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<SalesPoint> SalesPoints { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<OfferComment> OfferComments { get; set; }
        public DbSet<PostComment> PostComments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<User>()
                .HasMany(u => u.SalesPoints)
                .WithOne(sp => sp.User)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Entity<User>()
                .HasMany(u => u.Offers)
                .WithOne(o => o.User)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Entity<User>()
                .HasMany(u => u.Posts)
                .WithOne(p => p.User)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Entity<User>()
                .HasMany(u => u.OfferComments)
                .WithOne(oc => oc.User)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Entity<User>()
                .HasMany(u => u.PostComments)
                .WithOne(pc => pc.User)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Entity<Offer>()
                .HasMany(o => o.Comments)
                .WithOne(oc => oc.Offer)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Post>()
                .HasMany(p => p.Comments)
                .WithOne(pc => pc.Post)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<SalesPoint>()
                .HasMany(sp => sp.Offers)
                .WithOne(o => o.SalesPoint)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder
                .Entity<SalesPoint>()
                .Property(sp => sp.Address)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
                    v => JsonSerializer.Deserialize<Address>(v, JsonSerializerOptions.Default)!);

            builder
                .Entity<Offer>()
                .Property(o => o.Address)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
                    v => JsonSerializer.Deserialize<Address>(v, JsonSerializerOptions.Default)!);

            builder
                .Entity<Post>()
                .Property(p => p.Address)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
                    v => JsonSerializer.Deserialize<Address>(v, JsonSerializerOptions.Default));

            builder.Entity<Address>()
                .Property(a => a.Voivodeship)
                .HasConversion(
                    v => v.ToString(),
                    v => Enum.Parse<Voivodeship>(v));

            builder
                .Entity<Offer>()
                .Property(o => o.FuelType)
                .HasConversion(
                    v => v.ToString(),
                    v => Enum.Parse<FuelType>(v));

            builder
                .Entity<Post>()
                .Property(p => p.FuelType)
                .HasConversion(
                    v => v.ToString(),
                    v => Enum.Parse<FuelType>(v));
        }
    }
}
