using System.ComponentModel.DataAnnotations;

namespace HotelReservation.ViewModels
{
    public class ConfirmReservationViewModel
    {
        public int ReservationId { get; set; }
        public decimal TotalAmount { get; set; }

        [Required]
        [Display(Name = "Credit Card Number")]
        [CreditCard]
        public string CreditCardNumber { get; set; }


        [Required]
        [Display(Name = "CVV")]
        [RegularExpression(@"^[0-9]{3,4}$", ErrorMessage = "CVV must be 3 or 4 digits.")]
        public string CVV { get; set; }
    }
}
