using DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<HotelRoom> HotelRooms { get; set; }
        public DbSet<HotelRoomImage> HotelRoomImages { get; set; }
        public DbSet<HotelAmenity> HotelAmenities { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<RoomOrderDetails> RoomOrderDetails { get; set; }
    }
}