using System.ComponentModel.DataAnnotations;

namespace EventEase.Web.Models;

public class Event
{
    public int EventId { get; set; }

    [Required]
    public string Name { get; set; }

    public string? Description { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public bool IsAvailable { get; set; } = true;

    public string? ImageUrl { get; set; }

    public int EventTypeId { get; set; }
    public EventType? EventType { get; set; }

    public ICollection<Booking>? Bookings { get; set; }
}