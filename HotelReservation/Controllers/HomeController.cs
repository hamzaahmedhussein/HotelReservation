using HotelReservation.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index(FilteredPaginatedRooms model)
        {
            if (model == null)
            {
                model = new FilteredPaginatedRooms();
            }


            var query = _context.Rooms.AsQueryable();

            if (!string.IsNullOrEmpty(model.Address?.Trim()))
            {
                query = query.Where(r => r.Hotel.Address.Contains(model.Address.Trim()));
            }
            if (!string.IsNullOrEmpty(model.City?.Trim()))
            {
                query = query.Where(r => r.Hotel.City.Contains(model.City.Trim()));
            }
            if (!string.IsNullOrEmpty(model.State?.Trim()))
            {
                query = query.Where(r => r.Hotel.State.Contains(model.State.Trim()));
            }
            if (model.BedsNumber > 0)
            {
                query = query.Where(r => r.BedsNumber == model.BedsNumber);
            }
            if (model.PricePerNight > 0)
            {
                query = query.Where(r => r.PricePerNight <= model.PricePerNight);
            }
            if (model.CheckInDate != default && model.CheckOutDate != default)
            {
                query = query.Where(r => !r.Reservations.Any(res =>
                    res.ReservationStatus == ReservationStatus.Confirmed &&
                    res.CheckInDate < model.CheckOutDate &&
                    res.CheckOutDate > model.CheckInDate));
            }

            var totalRoomsCount = query.Count();

            var rooms = query
                .Skip((model.PageNumber - 1) * model.PageSize)
                .Take(model.PageSize)
                .Select(r => new RoomViewModel
                {
                    Id = r.Id,
                    HotelId = r.HotelId,
                    RoomNumber = r.RoomNumber,
                    BedsNumber = r.BedsNumber,
                    PricePerNight = r.PricePerNight,
                    RoomPicture = r.RoomPicture
                })
                .ToList();

            var roomsResult = new HomePagePagedResult<RoomViewModel>(rooms, totalRoomsCount, model.PageNumber, model.PageSize, model);
            return View(roomsResult);
        }
    }
}
