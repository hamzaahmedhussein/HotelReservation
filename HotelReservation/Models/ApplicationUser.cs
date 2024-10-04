using Microsoft.AspNetCore.Identity;

namespace HotelReservation.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? OTP { get; set; }
        public DateTime? OTPExpiry { get; set; }
    }
}
