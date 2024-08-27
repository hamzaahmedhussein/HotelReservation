namespace HotelReservation.Models
{
    public class Customer
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public ICollection<Reservation> Reservations { get; set; }

        public byte[]? ProfilePicture { get; set; } // Added property for profile picture
    }
}
