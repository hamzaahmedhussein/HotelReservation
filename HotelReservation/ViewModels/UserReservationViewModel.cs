using Microsoft.AspNetCore.Mvc;

namespace HotelReservation.ViewModels
{
    public class UserReservationViewModel 
    {
        public int Id { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public decimal TotalAmount { get; set; }
        public ReservationStatus ReservationStatus { get; set; }
    }
}
