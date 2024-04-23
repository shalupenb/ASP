using Microsoft.EntityFrameworkCore;

namespace ASP.Data
{
	public class DataContext : DbContext
	{
		public DbSet<Entities.User> Users { get; set; }
		public DbSet<Entities.Category> Categories { get; set; }
		public DbSet<Entities.Location> Locations { get; set; }
		public DbSet<Entities.Room> Rooms { get; set; }
		public DbSet<Entities.Reservation> Reservations { get; set; }

		public DataContext(DbContextOptions options) : base(options) { }

		//protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		//{
		//	// base.OnConfiguring(optionsBuilder);
		//}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// оскільки slug - ідентифікатор, він має бути унікальним
			modelBuilder.Entity<Entities.Category>()
				.HasIndex(c => c.Slug)
				.IsUnique();

			modelBuilder.Entity<Entities.Location>()
				.HasIndex(c => c.Slug)
				.IsUnique();

			modelBuilder.Entity<Entities.Room>()
				.HasIndex(c => c.Slug)
				.IsUnique();

			modelBuilder.Entity<Entities.Reservation>()
				.HasOne(r => r.User)
				.WithMany(u => u.Reservations)
				.HasForeignKey(r => r.UserId);
			modelBuilder.Entity<Entities.Reservation>()
				.HasOne(r => r.Room)
				.WithMany(r=> r.Reservations)
				.HasForeignKey(r => r.RoomId);


        }
    }
}
