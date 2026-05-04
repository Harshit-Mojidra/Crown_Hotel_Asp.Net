using hrbs_project.Models;
using Microsoft.EntityFrameworkCore;

namespace hrbs_project.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        // ================= TABLES =================
        public DbSet<User> Users { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<Booking> BookingOrder { get; set; }
        public DbSet<BookingDetails> BookingDetails { get; set; }
        public DbSet<Carousel> Carousel { get; set; }
        public DbSet<ContactDetails> ContactDetails { get; set; }
        public DbSet<RatingReview> RatingReviews { get; set; }
        public DbSet<RoomImage> RoomImages { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<TeamDetails> TeamDetails { get; set; }
        public DbSet<UserQuery> UserQueries { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().ToTable("UserCred");

            modelBuilder.Entity<Booking>().ToTable("booking_order");
            modelBuilder.Entity<Room>(entity =>
            {
                entity.ToTable("rooms");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
            });
        }
    }
}