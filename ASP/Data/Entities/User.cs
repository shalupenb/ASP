namespace ASP.Data.Entities
{
	public class User
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string? AvatarUrl { get; set; }

		public string Salt { get; set; } // 3a RFC-2898
		public string Derivedkey { get; set; } // 3a RFC-2898

		public DateTime? Birthdate { get; set; }
	}
}
