# Property Rental & Booking Platform - ASP.NET Core Razor Pages

A comprehensive property rental platform similar to Airbnb, built with ASP.NET Core 8 Razor Pages as a student final semester project.

## ?? Features

### Core Functionality
- **User Authentication & Authorization**
  - JWT-based authentication
  - Custom role system (Admin, Host, Guest)
  - Secure password hashing with BCrypt
  - Session management

- **Property Management**
  - Hosts can create, edit, and manage property listings
  - Property details with images, pricing, and amenities
  - Search and filter properties by city, dates, and guests
  - Dynamic property availability

- **Booking System**
  - Date conflict detection (prevents overlapping bookings)
  - Automatic price calculation
  - Booking status tracking (Pending, Confirmed, Completed, Cancelled)
  - Special requests functionality

- **Review System**
  - Guests can review properties after stay completion
  - 1-5 star rating with written comments
  - Only allows reviews after booking ends
  - Average rating display

- **Payment Management**
  - Simple payment status tracking (Unpaid/Paid/Refunded)
  - No external payment gateway (demo mode)
  - Payment confirmation workflow

- **Email Notifications**
  - Booking confirmation emails
  - Cancellation notifications
  - SMTP configuration via appsettings.json

## ?? Prerequisites

- .NET 8 SDK
- SQL Server LocalDB (included with Visual Studio)
- Visual Studio 2022 or VS Code

## ??? Technology Stack

- **Framework**: ASP.NET Core 8 (Razor Pages)
- **Database**: SQL Server LocalDB with EF Core 8
- **Authentication**: JWT + Custom Roles
- **Email**: MailKit (SMTP)
- **Password Hashing**: BCrypt.Net
- **UI Framework**: Bootstrap 5 + Bootstrap Icons
- **Architecture**: Repository Pattern (per entity)

## ?? Setup Instructions

### 1. Configure Email Settings
Update `appsettings.json`:
```json
"EmailSettings": {
  "SmtpServer": "smtp.gmail.com",
  "SmtpPort": "587",
  "SenderEmail": "your-email@gmail.com",
  "SmtpPassword": "your-app-password"
}
```

### 2. Run Database Migrations
```bash
dotnet ef database update
```

### 3. Run the Application
```bash
dotnet run
```

## ?? Default Credentials

**Admin Account:**
- Email: `admin@propertyrentals.com`
- Password: `Admin@123`

## ?? Key Features

- Date conflict detection
- Dynamic pricing calculation
- Review system (only after booking completion)
- Role-based authorization
- Email notifications

## ?? Database Entities

- Users
- Roles
- UserRoles
- Properties
- PropertyImages
- Bookings
- Reviews
- Payments

## ?? Quick Start Commands

```bash
# Restore packages
dotnet restore

# Update database
dotnet ef database update

# Run application
dotnet run
```

Happy coding! ??
