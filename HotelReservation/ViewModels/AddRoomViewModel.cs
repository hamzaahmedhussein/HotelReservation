namespace HotelReservation.ViewModels
{
    public class AddRoomViewModel
    {
        public string RoomNumber { get; set; }
        public int BedsNumber { get; set; }
        public decimal PricePerNight { get; set; }
        public IFormFile RoomPicture { get; set; } 
    }
}
