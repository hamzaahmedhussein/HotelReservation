using System.ComponentModel.DataAnnotations;

namespace HotelReservation.ViewModels
{
    public class CreateReservationViewModel
    {
        public int RoomId { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime CheckInDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CheckOutDate { get; set; }
        public ReservationStatus ReservationStatus { get; set; }
    }
}
