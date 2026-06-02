using EventEase.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventEase.Web.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        // Dashboard Counts
        ViewBag.TotalVenues = _context.Venues.Count();
        ViewBag.TotalEvents = _context.Events.Count();
        ViewBag.TotalBookings = _context.Bookings.Count();
        ViewBag.TotalEventTypes = _context.EventTypes.Count();

        // Venue Availability Statistics
        ViewBag.AvailableVenues = _context.Venues.Count(v => v.IsAvailable);

        ViewBag.UnavailableVenues = _context.Venues.Count(v => !v.IsAvailable);

        // Upcoming Events
        ViewBag.UpcomingEvents = _context.Events
            .Include(e => e.EventType)
            .Where(e => e.StartDate >= DateTime.Today)
            .OrderBy(e => e.StartDate)
            .Take(5)
            .ToList();

        // Recent Bookings
        ViewBag.RecentBookings = _context.Bookings
            .Include(b => b.Event)
            .Include(b => b.Venue)
            .OrderByDescending(b => b.BookingDate)
            .Take(5)
            .ToList();

        return View();
    }
}