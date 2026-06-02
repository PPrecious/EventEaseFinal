using EventEase.Web.Data;
using EventEase.Web.Models;
using EventEase.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EventEase.Web.Controllers
{
    public class EventsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly BlobService _blobService;

        public EventsController(AppDbContext context, BlobService blobService)
        {
            _context = context;
            _blobService = blobService;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            var events = _context.Events
                .Include(e => e.EventType);

            return View(await events.ToListAsync());
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            ViewBag.EventTypes = new SelectList(
                _context.EventTypes,
                "EventTypeId",
                "Name");

            return View();
        }

        // POST: Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event ev, IFormFile imageFile)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please complete all fields.";

                ViewBag.EventTypes = new SelectList(
                    _context.EventTypes,
                    "EventTypeId",
                    "Name");

                return View(ev);
            }

            if (imageFile != null)
            {
                ev.ImageUrl = await _blobService.UploadAsync(imageFile);
            }

            _context.Events.Add(ev);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Event created successfully.";

            return RedirectToAction(nameof(Index));
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var ev = await _context.Events.FindAsync(id);

            if (ev == null) return NotFound();

            ViewBag.EventTypes = new SelectList(
                _context.EventTypes,
                "EventTypeId",
                "Name",
                ev.EventTypeId);

            return View(ev);
        }

        // POST: Events/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Event ev, IFormFile imageFile)
        {
            if (id != ev.EventId)
                return NotFound();

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid event data.";

                ViewBag.EventTypes = new SelectList(
                    _context.EventTypes,
                    "EventTypeId",
                    "Name",
                    ev.EventTypeId);

                return View(ev);
            }

            try
            {
                var existingEvent = await _context.Events.FindAsync(id);

                if (existingEvent == null)
                    return NotFound();

                existingEvent.Name = ev.Name;
                existingEvent.Description = ev.Description;
                existingEvent.StartDate = ev.StartDate;
                existingEvent.EndDate = ev.EndDate;
                existingEvent.EventTypeId = ev.EventTypeId;
                existingEvent.IsAvailable = ev.IsAvailable;

                if (imageFile != null)
                {
                    existingEvent.ImageUrl = await _blobService.UploadAsync(imageFile);
                }

                _context.Update(existingEvent);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Event updated successfully.";
            }
            catch
            {
                TempData["Error"] = "Error updating event.";
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var ev = await _context.Events.FindAsync(id);

            if (ev == null)
                return RedirectToAction(nameof(Index));

            bool hasBookings = await _context.Bookings
                .AnyAsync(b => b.EventId == id);

            if (hasBookings)
            {
                TempData["Error"] = "Cannot delete event with active bookings.";
                return RedirectToAction(nameof(Index));
            }

            _context.Events.Remove(ev);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Event deleted successfully.";

            return RedirectToAction(nameof(Index));
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var ev = await _context.Events
                .Include(e => e.EventType)
                .FirstOrDefaultAsync(m => m.EventId == id);

            if (ev == null) return NotFound();

            return View(ev);
        }
    }
}