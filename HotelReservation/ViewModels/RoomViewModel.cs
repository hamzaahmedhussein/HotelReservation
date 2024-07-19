
namespace HotelReservation.ViewModels
{
    public class RoomViewModel
    {
        public int Id { get; set; }
        public int HotelId { get; set; }
        public string RoomNumber { get; set; }
        public int BedsNumber { get; set; }
        public decimal PricePerNight { get; set; }
        public byte[] RoomPicture { get; set; }
    }
}