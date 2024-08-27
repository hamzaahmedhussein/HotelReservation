namespace HotelReservation.ViewModels
{
    public class CustomerProfileViewModel
    {
        public CustomerViewModel Customer { get; set; }
        public PagedResult<UserReservationViewModel> Reservations { get; set; }

    }
}
