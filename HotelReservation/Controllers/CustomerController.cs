using HotelReservation.Models;
using HotelReservation.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Controllers
{
    public class CustomerController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public CustomerController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }


        [HttpGet("Profile/{id?}")]
        public async Task<IActionResult> CustomerProfile(int? id, int pageNumber = 1, int pageSize = 6)
        {
            Customer Customer = null;

            // Fetch customer profile information based on id or current user
            if (id.HasValue)
            {
                Customer = await _context.Customers
                                          .Include(c => c.Reservations)
                                          .FirstOrDefaultAsync(c => c.Id == id.Value);
            }
            else
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    Customer = await _context.Customers
                                             .Include(c => c.Reservations)
                                             .FirstOrDefaultAsync(h => h.ApplicationUserId == user.Id);
                }
            }

            if (Customer == null)
            {
                return NotFound();
            }

            // Calculate total number of reservations
            var totalReservations = Customer.Reservations.Count();

            // Paginate and project to UserReservationViewModel
            var reservation = Customer.Reservations
                                 .Skip((pageNumber - 1) * pageSize)
                                 .Take(pageSize)
                                 .Select(r => new UserReservationViewModel
                                 {
                                     Id = r.Id,
                                     CheckInDate = r.CheckInDate,
                                     CheckOutDate = r.CheckOutDate,
                                     TotalAmount = r.TotalAmount,
                                     ReservationStatus = r.ReservationStatus,
                                 })
                                 .ToList();

            // Create view model to pass to view
            var viewModel = new CustomerProfileViewModel
            {
                Customer = new CustomerViewModel
                {
                    FirstName = Customer.FirstName,
                    LastName = Customer.LastName,
                    City = Customer.City,
                    State = Customer.State,
                    ProfilePicture = Customer.ProfilePicture,
                },
                Reservations = new PagedResult<UserReservationViewModel>(reservation, totalReservations, pageNumber, pageSize)
            };
            return View(viewModel);
        }



        [HttpPost]
        public async Task<IActionResult> UploadCustomerPicture(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var customer = await _context.Customers.FindAsync(user.Id);
            if (customer == null)
            {
                return NotFound();
            }

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                customer.ProfilePicture = memoryStream.ToArray();
            }

            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();

            return Ok(new { imageUrl = $"data:image;base64,{Convert.ToBase64String(customer.ProfilePicture)}" });
        }




        [HttpGet]
        public async Task<IActionResult> UpdateCustomerInfo(int id)
        {
            var user = await _context.Customers.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var model = new UpdateCustomerInfoviewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                City = user.City,
                State = user.State
            };


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCustomerInfo(UpdateCustomerInfoviewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _context.Customers.FindAsync(model.Id);

            if (user == null)
            {
                return NotFound();
            }
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.City = model.City;
            user.State = model.State;

            _context.Customers.Update(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> CreateReservation(int id)
        {

            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound("Room not found");
            }
            var model = new CreateReservationViewModel
            {
                RoomId = id,
                CheckInDate = DateTime.Today,
                CheckOutDate = DateTime.Today.AddDays(1)


            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReservation(CreateReservationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.ApplicationUserId == user.Id);
            if (customer == null)
            {
                return BadRequest("Customer not found");
            }

            var room = await _context.Rooms.FindAsync(model.RoomId);
            if (room == null)
            {
                return NotFound("Room not found");
            }

            var totalDays = (model.CheckOutDate - model.CheckInDate).TotalDays;
            if (totalDays <= 0)
            {
                ModelState.AddModelError("", "Check-out date must be after check-in date.");
                return View(model);
            }

            var totalAmount = room.PricePerNight * (decimal)totalDays;
            var reservation = new Reservation
            {
                UserId = customer.Id,
                RoomId = model.RoomId,
                CheckInDate = model.CheckInDate,
                CheckOutDate = model.CheckOutDate,
                TotalAmount = totalAmount,
                Customer = customer,
                Room = room,
                ReservationStatus = ReservationStatus.Pending,
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return RedirectToAction("ConfirmReservation", new { reservationId = reservation.Id });
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmReservation(int reservationId)
        {
            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(r => r.Id == reservationId);

            if (reservation == null)
            {
                return NotFound("Reservation not found");
            }

            var model = new ConfirmReservationViewModel
            {
                ReservationId = reservation.Id,
                TotalAmount = reservation.TotalAmount,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmReservation(ConfirmReservationViewModel model)
        {
            if (!ModelState.IsValid)
            {

                return View(model);
            }

            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(r => r.Id == model.ReservationId);

            if (reservation == null)
            {
                return NotFound("Reservation not found");
            }

            var payment = new Payment
            {
                ReservationId = model.ReservationId,
                CreditCardNumber = model.CreditCardNumber,
                CVV = model.CVV,
                Amount = reservation.TotalAmount,
                Status = Payment.PaymentStatus.Pending
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            // Simulate payment processing (e.g., call to a payment gateway API)
            payment.Status = Payment.PaymentStatus.Completed;
            reservation.ReservationStatus = ReservationStatus.Confirmed;

            await _context.SaveChangesAsync();

            return RedirectToAction("ReservationSuccess");
        }



        public IActionResult ReservationSuccess()
        {
            return View();
        }





    }
}
