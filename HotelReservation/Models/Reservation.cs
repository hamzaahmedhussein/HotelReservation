using HotelReservation.Models;

public class Reservation
{
    public int Id { get; set; }
    public int UserId { get; set; } // Foreign key to User
    public int RoomId { get; set; } // Foreign key to Room
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public decimal TotalAmount { get; set; }
    public ReservationStatus ReservationStatus { get; set; } // Confirmed, Cancelled, Completed

    // Navigation properties
    public Customer Customer { get; set; }
    public Room Room { get; set; }
}

public enum ReservationStatus
{
    Pending,    // 0
    Confirmed,  // 1
    Cancelled,  // 2
    Completed   // 3
}


