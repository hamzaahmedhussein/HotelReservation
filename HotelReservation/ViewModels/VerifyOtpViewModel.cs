﻿using System.ComponentModel.DataAnnotations;

namespace HotelReservation.ViewModels
{
    public class VerifyOtpViewModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string OTP { get; set; }
    }
}
