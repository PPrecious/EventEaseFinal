using System.ComponentModel.DataAnnotations;

namespace EventEase.Web.Models;

public class Venue
{
    public int VenueId { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Location { get; set; }

    [Range(1, 100000)]
    public int Capacity { get; set; }

    public bool IsAvailable { get; set; } = true;

    public string? ImageUrl { get; set; }

    public ICollection<Booking>? Bookings { get; set; }
}