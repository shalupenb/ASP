using Microsoft.EntityFrameworkCore;

namespace ASP.Data
{
	public class DataContext : DbContext
	{
		public DbSet<Entities.User> Users { get; set; }
		public DbSet<Entities.Category> Categories { get; set; }
        public DbSet<Entities.Location> Locations { get; set; }
        public DbSet<Entities.Room> Rooms { get; set; }
		public object Location { get; internal set; }

		public DataContext(DbContextOptions options) : base(options) { }

		//protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		//{
		//	// base.OnConfiguring(optionsBuilder);
		//}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Entities.Category>()
				.HasIndex(c => c.Slug)
				.IsUnique();

			modelBuilder.Entity<Entities.Location>()
				.HasIndex(c => c.Slug)
				.IsUnique();

			modelBuilder.Entity<Entities.Room>()
				.HasIndex(c => c.Slug)
				.IsUnique();
		}
	}
}
