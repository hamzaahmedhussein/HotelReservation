namespace HotelReservation.ViewModels
{
    public class UpdateRoomViewModel
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; }
        public int BedsNumber { get; set; }
        public decimal PricePerNight { get; set; }
        public IFormFile RoomPicture { get; set; } // For file upload
    }
}
