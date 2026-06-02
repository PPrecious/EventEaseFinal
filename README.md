# EventEase Final

EventEase is a cloud-enabled ASP.NET Core MVC web application developed to streamline venue booking and event management processes for EventEase, an event management company.

The system enables booking specialists to manage venues, events, event types, and bookings through a centralized administration platform. It prevents double bookings, supports advanced search and filtering capabilities, provides dashboard reporting, and is designed to support future Microsoft Azure cloud integrations.

This project was developed as part of a Portfolio of Evidence (POE) focusing on cloud application development, database design, Azure services, and ASP.NET Core MVC development.

# Features
## Dashboard
The dashboard provides an overview of the system and includes:

* Total number of venues
* Total number of events
* Total number of bookings
* Total number of event types
* Upcoming events display
* Recent bookings display

## Venue Management
The system allows administrators to:

* Create venues
* View venue details
* Edit venue information
* Delete venues
* Upload and manage venue images
* Store venue location information
* Store venue capacity information
* Manage venue availability status

## Event Management
The system allows administrators to:

* Create events
* View event details
* Edit event information
* Delete events
* Assign event types
* Upload and manage event images
* Store event dates and descriptions

## Event Type Management
The system supports:

* Creating event types
* Editing event types
* Deleting event types
* Categorizing events for improved organization and filtering

Examples include:

* Conference
* Workshop
* Concert
* Corporate Event
* Wedding

## Booking Management
The booking module provides:

* Create bookings
* View bookings
* Edit bookings
* Delete bookings
* Search bookings
* Filter bookings
* Display booking information
* Prevent venue double-bookings
* Validate booking data before saving

## Search Functionality
Users can search bookings using:

* Booking ID
* Event Name

## Advanced Filtering
Users can filter bookings using:

* Event Type
* Date Range
* Venue Availability

These filters help booking specialists quickly locate relevant bookings and venue information.

## Validation and Business Rules
The system includes validation and business rules to improve data integrity and user experience:

### Double Booking Prevention
A venue cannot be booked for multiple events that overlap in date and time.

### Delete Restrictions
The system prevents:

* Deleting a venue that has existing bookings
* Deleting an event that has existing bookings

### Required Field Validation

The system validates mandatory fields and displays user-friendly error messages when required information is missing.

# Technologies Used
## Frontend
* HTML
* CSS
* Razor Views

## Backend
* ASP.NET Core MVC (.NET 8)
* C#

## Database
* Entity Framework Core
* Azure SQL Database 

## Cloud Services
* Azure App Service
* Azure Blob Storage
* Azure SQL Database

## Development Tools
* Visual Studio 2022
* GitHub
* Microsoft Azure


# Installation Instructions
Ensure the following software is installed:

* Visual Studio 2022
* .NET 8 SDK

## Running the Application
1. Open the solution in Visual Studio 2022.
2. Restore NuGet packages.
3. Build the solution.
4. Run the application using IIS Express or Kestrel.
5. The application will launch in your default web browser.


# Azure Deployment
### Azure App Service
Azure App Service is used to host and deploy the EventEase web application in the cloud. It provides scalability, security, and high availability.

### Azure Blob Storage
Azure Blob Storage is used to store and manage uploaded venue and event images. This provides a scalable and reliable solution for file storage.

### Azure SQL Database
Azure SQL Database is used in production environments to store application data securely in the cloud while providing backup, recovery, and high availability features.

**Student Name:** _________________________
