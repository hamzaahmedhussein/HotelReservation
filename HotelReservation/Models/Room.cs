using HotelReservation.Models;
namespace HotelReservation.Models
{
    public class Room
    {
        public int Id { get; set; }
        public int HotelId { get; set; }
        public string RoomNumber { get; set; }
        public int BedsNumber { get; set; }
        public decimal PricePerNight { get; set; }
        public byte[] RoomPicture { get; set; } // Add this property


        public Hotel Hotel { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
    }
}


