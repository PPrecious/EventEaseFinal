using EventEase.Web.Data;
using EventEase.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventEase.Web.Controllers;

public class EventTypesController : Controller
{
    private readonly AppDbContext _context;

    public EventTypesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: EventTypes
    public async Task<IActionResult> Index()
    {
        var eventTypes = await _context.EventTypes.ToListAsync();
        return View(eventTypes);
    }

    // GET: EventTypes/Create
    public IActionResult Create()
    {
        return View(new EventType());
    }

    // POST: EventTypes/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EventType model)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Please enter a valid event type.";
            return View(model);
        }

        _context.EventTypes.Add(model);
        await _context.SaveChangesAsync();

        TempData["Success"] = "Event type created successfully.";
        return RedirectToAction(nameof(Index));
    }

    // GET: EventTypes/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var eventType = await _context.EventTypes.FindAsync(id);

        if (eventType == null) return NotFound();

        return View(eventType);
    }

    // POST: EventTypes/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EventType model)
    {
        if (id != model.EventTypeId)
            return NotFound();

        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Invalid event type data.";
            return View(model);
        }

        _context.Update(model);
        await _context.SaveChangesAsync();

        TempData["Success"] = "Event type updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    // GET: EventTypes/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var eventType = await _context.EventTypes
            .FirstOrDefaultAsync(e => e.EventTypeId == id);

        if (eventType == null) return NotFound();

        bool inUse = await _context.Events
            .AnyAsync(e => e.EventTypeId == id);

        if (inUse)
        {
            TempData["Error"] = "Cannot delete event type currently in use.";
            return RedirectToAction(nameof(Index));
        }

        _context.EventTypes.Remove(eventType);
        await _context.SaveChangesAsync();

        TempData["Success"] = "Event type deleted successfully.";
        return RedirectToAction(nameof(Index));
    }
}