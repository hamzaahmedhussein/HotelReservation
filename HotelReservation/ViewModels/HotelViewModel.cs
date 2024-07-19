

namespace HotelReservation.ViewModels
{
    public class HotelViewModel
    {
        public string Name { get; set; }
        public byte[] HotelPicture { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public decimal Rating { get; set; }
    }
}