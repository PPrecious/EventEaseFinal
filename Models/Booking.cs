using System.ComponentModel.DataAnnotations;

namespace EventEase.Web.Models;

public class Booking
{
    public int BookingId { get; set; }

    [Required]
    public int VenueId { get; set; }
    public Venue? Venue { get; set; }

    [Required]
    public int EventId { get; set; }
    public Event? Event { get; set; }

    public DateTime BookingDate { get; set; } = DateTime.Now;
}