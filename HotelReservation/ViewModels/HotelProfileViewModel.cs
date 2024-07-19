

namespace HotelReservation.ViewModels
{ public class HotelProfileViewModel
{
    public HotelViewModel Hotel { get; set; }
     public PagedResult<RoomViewModel> Rooms { get; set; }
    }
}