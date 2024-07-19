namespace HotelReservation.ViewModels
{
    public class FilteredRoomViewModel
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; }
        public int BedsNumber { get; set; }
        public decimal PricePerNight { get; set; }
        public byte[] RoomPicture { get; set; }
        public string HotelName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}
