namespace ASP.Data.Entities
{
	public class User
	{
		public Guid Id { get; set; }
		public String Name { get; set; }
		public String Email { get; set; }
		public String? EmailConfirmCode { get; set; } // code or null: null - confirmed
		public String? AvatarUrl { get; set; }
		public String Salt { get; set; } // 3a RFC-2898
		public String Derivedkey { get; set; } // 3a RFC-2898
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