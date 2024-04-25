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
        public DateTime? DeletedDt { get; set; }
		public String? Role { get; set; }


        public List<Reservation> Reservations { get; set; }
    }
}


/*
*	Category: Hotels Apartments Resorts Villas
*	Location: Hotel1 Hotel2 Hotel3
*	Room: Room101 Room102 Room103
*/