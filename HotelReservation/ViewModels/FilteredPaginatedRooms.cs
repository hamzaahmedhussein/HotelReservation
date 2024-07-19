namespace HotelReservation.ViewModels
{
    public class FilteredPaginatedRooms
    {
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int BedsNumber { get; set; }
        public decimal PricePerNight { get; set; }
        public DateTime CheckInDate { get; set; } = DateTime.Today;
        public DateTime CheckOutDate { get; set; } = DateTime.Today.AddDays(1);
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 6;
    }

}
