using EventEase.Web.Data;
using EventEase.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EventEase.Web.Controllers;

public class BookingsController : Controller
{
    private readonly AppDbContext _context;

    public BookingsController(AppDbContext context)
    {
        _context = context;
    }
    public IActionResult Index(
        string? search,
        int? eventTypeId,
        DateTime? startDate,
        DateTime? endDate,
        bool? venueAvailable)
    {
        ViewBag.EventTypes = _context.EventTypes.ToList();

        var bookings = _context.Bookings
            .Include(b => b.Event)
                .ThenInclude(e => e!.EventType)
            .Include(b => b.Venue)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            bookings = bookings.Where(b =>
                b.BookingId.ToString().Contains(search) ||
                (b.Event != null && b.Event.Name.Contains(search)));
        }

        if (eventTypeId.HasValue)
        {
            bookings = bookings.Where(b =>
                b.Event != null &&
                b.Event.EventTypeId == eventTypeId.Value);
        }

        if (startDate.HasValue)
        {
            bookings = bookings.Where(b =>
                b.Event != null &&
                b.Event.StartDate >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            bookings = bookings.Where(b =>
                b.Event != null &&
                b.Event.EndDate <= endDate.Value);
        }

        if (venueAvailable.HasValue)
        {
            bookings = bookings.Where(b =>
                b.Venue != null &&
                b.Venue.IsAvailable == venueAvailable.Value);
        }

        return View(bookings.ToList());
    }

    // 2. CREATE 
    public IActionResult Create()
    {
        LoadDropdowns();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Booking booking)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Please complete all required fields.";
            LoadDropdowns();
            return View(booking);
        }

        var ev = await _context.Events.FindAsync(booking.EventId);

        if (ev == null)
        {
            TempData["Error"] = "Selected event not found.";
            LoadDropdowns();
            return View(booking);
        }

        // DOUBLE BOOKING CHECK
        bool conflict = _context.Bookings
            .Include(b => b.Event)
            .Any(b =>
                b.VenueId == booking.VenueId &&
                b.Event != null &&
                ev.StartDate < b.Event.EndDate &&
                ev.EndDate > b.Event.StartDate);

        if (conflict)
        {
            TempData["Error"] = "Double booking detected for this venue.";
            LoadDropdowns();
            return View(booking);
        }

        booking.BookingDate = DateTime.Now;

        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        TempData["Success"] = "Booking created successfully.";

        return RedirectToAction(nameof(Index));
    }

    // 4. EDIT

    public async Task<IActionResult> Edit(int id)
    {
        var booking = await _context.Bookings.FindAsync(id);

        if (booking == null)
            return NotFound();

        LoadDropdowns();
        return View(booking);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Booking booking)
    {
        if (id != booking.BookingId)
            return NotFound();

        if (!ModelState.IsValid)
        {
            LoadDropdowns();
            return View(booking);
        }

        var ev = await _context.Events.FindAsync(booking.EventId);

        if (ev == null)
        {
            TempData["Error"] = "Event not found.";
            LoadDropdowns();
            return View(booking);
        }

        // DOUBLE BOOKING CHECK (exclude current booking)
        bool conflict = _context.Bookings
            .Include(b => b.Event)
            .Any(b =>
                b.BookingId != booking.BookingId &&
                b.VenueId == booking.VenueId &&
                b.Event != null &&
                ev.StartDate < b.Event.EndDate &&
                ev.EndDate > b.Event.StartDate);

        if (conflict)
        {
            TempData["Error"] = "This venue is already booked for selected time.";
            LoadDropdowns();
            return View(booking);
        }

        _context.Update(booking);
        await _context.SaveChangesAsync();

        TempData["Success"] = "Booking updated successfully.";

        return RedirectToAction(nameof(Index));
    }

    // DELETE (GET)
    public async Task<IActionResult> Delete(int id)
    {
        var booking = await _context.Bookings
            .Include(b => b.Event)
            .Include(b => b.Venue)
            .FirstOrDefaultAsync(b => b.BookingId == id);

        if (booking == null)
            return NotFound();

        return View(booking);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var booking = await _context.Bookings.FindAsync(id);

        if (booking != null)
        {
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Booking deleted successfully.";
        }

        return RedirectToAction(nameof(Index));
    }

    private void LoadDropdowns()
    {
        ViewBag.Venues = new SelectList(
            _context.Venues,
            "VenueId",
            "Name");

        ViewBag.Events = new SelectList(
            _context.Events,
            "EventId",
            "Name");

        ViewBag.EventTypes = _context.EventTypes.ToList();
    }
}