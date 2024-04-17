using Microsoft.EntityFrameworkCore;

namespace ASP.Data
{
	public class DataContext : DbContext
	{
		public DbSet<Entities.User> Users { get; set; }
        public DbSet<Entities.Category> Categories { get; set; }
        public DbSet<Entities.Location> Locations { get; set; }
        public DbSet<Entities.Room> Rooms { get; set; }

        public DataContext(DbContextOptions options) : base(options) { }

		//protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		//{
		//	// base.OnConfiguring(optionsBuilder);
		//}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

		}
	}
}
