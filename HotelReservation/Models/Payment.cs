namespace HotelReservation.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public string CreditCardNumber { get; set; }
        public string CVV { get; set; }
        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; }

        public enum PaymentStatus
        {
            Pending,
            Completed,
            Failed
        }
    }
}
