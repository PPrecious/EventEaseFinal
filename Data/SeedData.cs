using EventEase.Web.Models;

namespace EventEase.Web.Data;

public static class SeedData
{
    public static void Initialize(AppDbContext context)
    {
        if (context.Venues.Any())
            return;

      //Event types
        var conference = new EventType { EventTypeId = 1, Name = "Conference" };
        var concert = new EventType { EventTypeId = 2, Name = "Concert" };
        var workshop = new EventType { EventTypeId = 3, Name = "Workshop" };


        context.EventTypes.AddRange(
            conference,
            concert,
            workshop
        );

       //Venues
        var venue1 = new Venue
        {
            VenueId = 1,
            Name = "Grand Hall",
            Location = "Cape Town",
            Capacity = 500,
            IsAvailable = true,
            ImageUrl = "https://eventeasewebstorage.blob.core.windows.net/eventeasewebstorage/https://pin.it/vWi8OKTQN/download (2).jpg"
        };

        var venue2 = new Venue
        {
            VenueId = 2,
            Name = "City Auditorium",
            Location = "Johannesburg",
            Capacity = 700,
            IsAvailable = true,
            ImageUrl = "https://eventeasewebstorage.blob.core.windows.net/eventeasewebstorage/https://pin.it/Fz7qkh9le/download (3).jpg"
        };

        var venue3 = new Venue
        {
            VenueId = 3,
            Name = "Rosey Graden",
            Location = "Stellenbosch",
            Capacity = 100,
            IsAvailable = true,
            ImageUrl = "https://eventeasewebstorage.blob.core.windows.net/eventeasewebstorage/https://pin.it/57hYYTjMG/download (6).jpg"
        };

        context.Venues.AddRange(venue1, venue2);

     //Events
        var event1 = new Event
        {
            EventId = 1,
            Name = "Annual Tech Conference",
            Description = "Technology Event",
            StartDate = DateTime.Now.AddDays(10),
            EndDate = DateTime.Now.AddDays(12),
            EventTypeId = 1,
            IsAvailable = true,
            ImageUrl = "https://eventeasewebstorage.blob.core.windows.net/eventeasewebstorage/https://pin.it/1JZgBJrX6/download (4).jpg"
        };

        var event2 = new Event
        {
            EventId = 2,
            Name = "Music Festival",
            Description = "Live Concert",
            StartDate = DateTime.Now.AddDays(20),
            EndDate = DateTime.Now.AddDays(21),
            EventTypeId = 2,
            IsAvailable = true,
            ImageUrl = "https://eventeasewebstorage.blob.core.windows.net/eventeasewebstorage/https://pin.it/Z21drGua4/download (5).jpg"
        };

        context.Events.AddRange(event1, event2);

       //Bookings
        var booking1 = new Booking
        {
            BookingId = 1,
            VenueId = 1,
            EventId = 1,
            BookingDate = DateTime.Now
        };

        context.Bookings.Add(booking1);

        context.SaveChanges();
    }
}