using HotelReservation.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservation.Controllers
{
    public class HomeController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        [HttpGet]
        public IActionResult Index(FilteredPaginatedRooms model)
        {
            if (model == null)
            {
                model = new FilteredPaginatedRooms();
            }

            model.PageNumber = model.PageNumber == 0 ? 1 : model.PageNumber;
            model.PageSize = model.PageSize == 0 ? 6 : model.PageSize;

            var query = _context.Rooms.AsQueryable();

            if (!string.IsNullOrEmpty(model.Address))
            {
                query = query.Where(r => r.Hotel.Address.Contains(model.Address));
            }
            if (!string.IsNullOrEmpty(model.City))
            {
                query = query.Where(r => r.Hotel.City.Contains(model.City));
            }
            if (!string.IsNullOrEmpty(model.State))
            {
                query = query.Where(r => r.Hotel.State.Contains(model.State));
            }
            if (model.BedsNumber > 0)
            {
                query = query.Where(r => r.BedsNumber == model.BedsNumber);
            }
            if (model.PricePerNight > 0)
            {
                query = query.Where(r => r.PricePerNight <= model.PricePerNight);
            }
            if (model.CheckInDate != default(DateTime) && model.CheckOutDate != default(DateTime))
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
