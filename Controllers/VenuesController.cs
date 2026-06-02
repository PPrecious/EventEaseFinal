using EventEase.Web.Data;
using EventEase.Web.Models;
using EventEase.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventEase.Web.Controllers
{
    public class VenuesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly BlobService _blobService;

        public VenuesController(AppDbContext context, BlobService blobService)
        {
            _context = context;
            _blobService = blobService;
        }

        // GET: Venues
        public async Task<IActionResult> Index()
        {
            return View(await _context.Venues.ToListAsync());
        }

        // GET: Venues/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Venues/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Venue venue, IFormFile imageFile)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please complete all required fields.";
                return View(venue);
            }

            if (imageFile != null)
            {
                var url = await _blobService.UploadAsync(imageFile);
                venue.ImageUrl = url;
            }

            _context.Venues.Add(venue);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Venue created successfully.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Venues/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var venue = await _context.Venues.FindAsync(id);

            if (venue == null) return NotFound();

            return View(venue);
        }

        // POST: Venues/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Venue venue, IFormFile imageFile)
        {
            if (id != venue.VenueId)
                return NotFound();

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid venue data.";
                return View(venue);
            }

            try
            {
                var existingVenue = await _context.Venues.FindAsync(id);

                if (existingVenue == null)
                    return NotFound();

                existingVenue.Name = venue.Name;
                existingVenue.Location = venue.Location;
                existingVenue.Capacity = venue.Capacity;
                existingVenue.IsAvailable = venue.IsAvailable;

                if (imageFile != null)
                {
                    var url = await _blobService.UploadAsync(imageFile);
                    existingVenue.ImageUrl = url;
                }

                await _context.SaveChangesAsync();

                TempData["Success"] = "Venue updated successfully.";
            }
            catch
            {
                TempData["Error"] = "An error occurred while updating.";
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Venues/Delete/5  
        public async Task<IActionResult> Delete(int id)
        {
            var venue = await _context.Venues.FindAsync(id);

            if (venue == null)
            {
                TempData["Error"] = "Venue not found.";
                return RedirectToAction(nameof(Index));
            }

            bool hasBookings = await _context.Bookings
                .AnyAsync(b => b.VenueId == id);

            if (hasBookings)
            {
                TempData["Error"] = "Cannot delete venue with active bookings.";
                return RedirectToAction(nameof(Index));
            }

            _context.Venues.Remove(venue);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Venue deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Venues/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var venue = await _context.Venues
                .FirstOrDefaultAsync(m => m.VenueId == id);

            if (venue == null) return NotFound();

            return View(venue);
        }
    }
}