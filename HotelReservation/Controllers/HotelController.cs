using HotelReservation.Models;
using HotelReservation.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Controllers
{
    public class HotelController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public HotelController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet("hotelProfile/{id?}")]
        public async Task<IActionResult> HotelProfile(int? id, int pageNumber = 1, int pageSize = 3)
        {
            Hotel hotel = null;

            // Fetch hotel profile information based on id or current user
            if (id.HasValue)
            {
                hotel = await _context.Hotels
                                      .Include(h => h.Rooms)
                                      .FirstOrDefaultAsync(h => h.Id == id.Value);
            }
            else
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    hotel = await _context.Hotels
                                          .Include(h => h.Rooms)
                                          .FirstOrDefaultAsync(h => h.ApplicationUserId == user.Id);
                }
            }

            if (hotel == null)
            {
                return NotFound();
            }

            // Calculate total number of rooms
            var totalRooms = hotel.Rooms.Count();

            // Paginate and project to RoomViewModel
            var rooms = hotel.Rooms
                             .Skip((pageNumber - 1) * pageSize)
                             .Take(pageSize)
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

            // Create view model to pass to view
            var viewModel = new HotelProfileViewModel
            {
                Hotel = new HotelViewModel
                {
                    Name = hotel.Name,
                    Address = hotel.Address,
                    City = hotel.City,
                    State = hotel.State,
                    Rating = hotel.Rating
                },

                Rooms = new PagedResult<RoomViewModel>(rooms, totalRooms, pageNumber, pageSize)
            };

            return View(viewModel);
        }



        [HttpPost]
        public async Task<IActionResult> UploadHotelPicture(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            // Ensure file is an image
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
            {
                return BadRequest("Invalid file type. Please upload an image.");
            }

            // Limit file size to 2MB
            const long maxFileSize = 2 * 1024 * 1024;
            if (file.Length > maxFileSize)
            {
                return BadRequest("File size exceeds 2MB.");
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var hotel = await _context.Hotels.FirstOrDefaultAsync(h => h.ApplicationUserId == user.Id);

            if (hotel == null)
            {
                return NotFound();
            }

            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    hotel.HotelPicture = memoryStream.ToArray();
                }

                _context.Hotels.Update(hotel);
                await _context.SaveChangesAsync();

                var imageUrl = $"data:image;base64,{Convert.ToBase64String(hotel.HotelPicture)}";
                return Ok(new { imageUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }




        [HttpGet]
        public async Task<IActionResult> AddRoom()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> AddRoom(AddRoomViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var hotel = await _context.Hotels.FirstOrDefaultAsync(h => h.ApplicationUserId == user.Id);

                if (hotel != null)
                {

                    byte[] picture = null;
                    if (model.RoomPicture != null)
                    {
                        picture = await ConvertToByteArrayAsync(model.RoomPicture);
                    }

                    var room = new Room
                    {
                        RoomNumber = model.RoomNumber,
                        BedsNumber = model.BedsNumber,
                        PricePerNight = model.PricePerNight,
                        HotelId = hotel.Id,
                        Hotel = hotel,
                        RoomPicture = picture
                    };

                    _context.Rooms.Add(room);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "User does not have a hotel associated.");
                }
            }

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> UpdateRoom(int id)
        {
            var room = await _context.Rooms.FindAsync(id);

            if (room == null)
            {
                return NotFound();
            }

            var model = new UpdateRoomViewModel
            {
                Id = room.Id,
                RoomNumber = room.RoomNumber,
                BedsNumber = room.BedsNumber,
                PricePerNight = room.PricePerNight,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRoom(UpdateRoomViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var room = await _context.Rooms.FindAsync(model.Id);

            if (room == null)
            {
                return NotFound();
            }

            room.RoomNumber = model.RoomNumber;
            room.BedsNumber = model.BedsNumber;
            room.PricePerNight = model.PricePerNight;


            if (model.RoomPicture != null)
            {
                room.RoomPicture = await ConvertToByteArrayAsync(model.RoomPicture);
            }

            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }



        [HttpGet]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var room = await _context.Rooms.FindAsync(id);

            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        [HttpPost, ActionName("DeleteRoom")]
        public async Task<IActionResult> DeleteRoomConfirmed(int id)
        {
            var room = await _context.Rooms.FindAsync(id);

            if (room == null)
            {
                return NotFound();
            }

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }


        private async Task<byte[]> ConvertToByteArrayAsync(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}