# ?? Complete Setup Guide - Property Rental System

## Step-by-Step Installation Instructions

### 1. Prerequisites Check
- [ ] Visual Studio 2022 (or VS Code with C# extension)
- [ ] .NET 8 SDK installed
- [ ] SQL Server LocalDB (comes with Visual Studio)
- [ ] Git (optional)

### 2. Open the Project
```bash
# Navigate to project directory
cd "D:\Rozer Internal\PropertyRentalSystem\PropertyRentalSystem"

# Open in Visual Studio
start PropertyRentalSystem.csproj

# OR open in VS Code
code .
```

### 3. Restore NuGet Packages
```bash
dotnet restore
```

This will install:
- Entity Framework Core
- JWT Authentication libraries
- BCrypt for password hashing
- MailKit for emails

### 4. Configure Email Settings (Important!)

Open `appsettings.json` and update the email settings:

**For Gmail:**
```json
"EmailSettings": {
  "SmtpServer": "smtp.gmail.com",
  "SmtpPort": "587",
  "SenderName": "Property Rental System",
  "SenderEmail": "your-gmail@gmail.com",
  "SmtpUsername": "your-gmail@gmail.com",
  "SmtpPassword": "your-app-password-here",
  "RequireAuthentication": "true"
}
```

**How to get Gmail App Password:**
1. Go to https://myaccount.google.com/security
2. Enable 2-Factor Authentication
3. Go to https://myaccount.google.com/apppasswords
4. Generate an App Password
5. Copy and paste it in `SmtpPassword` field

**Alternative (Disable Email):**
If you don't want to configure email, you can comment out email-sending code in:
- `BookingService.cs` (lines in CreateBookingAsync and CancelBookingAsync)
- The application will still work without emails

### 5. Create Database
```bash
# Run migrations
dotnet ef database update
```

This will:
- ? Create `PropertyRentalDb` database
- ? Create all tables with relationships
- ? Seed 3 roles: Admin, Host, Guest
- ? Create default admin user

### 6. (Optional) Add Sample Data

To add demo properties, open `Program.cs` and **uncomment** lines 76-85:

```csharp
// Uncomment these lines:
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        await SampleDataSeeder.SeedSampleDataAsync(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}
```

Then run the app once to seed data.

### 7. Build the Project
```bash
dotnet build
```

### 8. Run the Application
```bash
dotnet run
```

OR press `F5` in Visual Studio.

The app will start at: `https://localhost:XXXX` (check console output)

### 9. Login with Default Admin

Navigate to: `https://localhost:XXXX`

**Admin Credentials:**
- Email: `admin@propertyrentals.com`
- Password: `Admin@123`

## ?? Quick Testing Workflow

### Test 1: Register Users

1. **Register as Host:**
   - Click "Register"
   - Fill in details
   - Select Role: "Host"
   - Register

2. **Register as Guest:**
   - Logout
   - Click "Register"
   - Fill in details
   - Select Role: "Guest"
   - Register

### Test 2: Create Property (as Host)

1. Login with Host account
2. Click "My Properties"
3. Click "List New Property"
4. Fill in property details:
   - Title: "Test Property"
   - Description: "Beautiful test property"
   - Address, City, Country, Zip
   - Price: $100
   - Guests: 2, Bedrooms: 1, Bathrooms: 1
   - Image URLs (one per line):
     ```
     https://images.unsplash.com/photo-1522708323590-d24dbb6b0267?w=800
     https://images.unsplash.com/photo-1502672260266-1c1ef2d93688?w=800
     ```
5. Submit

### Test 3: Book Property (as Guest)

1. Login with Guest account
2. Click "Browse Properties"
3. Select a property
4. Fill in booking details:
   - Check-in: Tomorrow
   - Check-out: Day after tomorrow
   - Guests: 2
5. Click "Book Now"
6. View booking in "My Bookings"

### Test 4: Make Payment

1. Go to "My Bookings"
2. Click "View Details" on unpaid booking
3. Click "Pay Now"
4. Fill in payment info (demo):
   - Payment Method: Credit Card
   - Transaction Reference: TEST123
5. Submit payment

### Test 5: Complete Booking & Review

1. Login as Admin
2. Navigate to booking
3. Manually update booking status to "Completed" in database:
   ```sql
   UPDATE Bookings SET Status = 'Completed' WHERE Id = 1
   ```
   OR wait until check-out date passes

4. Login as Guest
5. Go to completed booking
6. Click "Write Review"
7. Select rating and write comment
8. Submit review

## ?? Troubleshooting

### Problem: Migration fails
**Solution:**
```bash
# Remove existing database
dotnet ef database drop

# Recreate it
dotnet ef database update
```

### Problem: Can't login
**Solution:**
- Check if database was seeded (check Users table)
- Verify password is exactly: `Admin@123`
- Clear browser cookies/cache

### Problem: Email not sending
**Solution:**
- Verify Gmail App Password is correct
- Check SMTP settings match exactly
- Or comment out email code temporarily

### Problem: Images not showing
**Solution:**
- Use Unsplash URLs (they work cross-origin)
- Test URL in browser first
- Example: `https://images.unsplash.com/photo-1522708323590-d24dbb6b0267?w=800`

### Problem: Port already in use
**Solution:**
```bash
# Kill process on port
netstat -ano | findstr :5000
taskkill /PID [process-id] /F

# Or change port in Properties/launchSettings.json
```

## ?? Database Verification

To verify database was created correctly:

1. Open SQL Server Object Explorer in Visual Studio
2. Connect to `(localdb)\mssqllocaldb`
3. Expand `PropertyRentalDb`
4. Check tables exist:
   - Users (should have 1 admin)
   - Roles (should have 3 roles)
   - UserRoles (should have 1 mapping)
   - Properties, Bookings, Reviews, Payments (empty initially)

## ?? Project Structure Overview

```
PropertyRentalSystem/
??? Models/           ? Database entities
??? Data/             ? DbContext & migrations
??? Repositories/     ? Data access layer
??? Services/         ? Business logic
??? Pages/            ? Razor Pages (UI)
??? Helpers/          ? Utility classes
??? wwwroot/          ? Static files
```

## ?? Key Files to Know

- `appsettings.json` - Configuration (DB, JWT, Email)
- `Program.cs` - App startup & DI registration
- `ApplicationDbContext.cs` - Database configuration
- `_Layout.cshtml` - Main layout/navigation
- `_ViewImports.cshtml` - Global Razor imports

## ?? Common Tasks

### Add New Property Image URLs
Use these free image services:
- Unsplash: `https://images.unsplash.com/photo-XXXXX?w=800`
- Pexels: `https://images.pexels.com/photos/XXXXX/pexels-photo-XXXXX.jpeg?w=800`

### Reset Database
```bash
dotnet ef database drop -f
dotnet ef database update
```

### Create New Migration
```bash
dotnet ef migrations add MigrationName
dotnet ef database update
```

### View Logs
Check console output while app is running for:
- Database queries
- Email sending attempts
- Authentication events

## ? Success Checklist

- [ ] Database created successfully
- [ ] Can login as admin
- [ ] Can register new users
- [ ] Can create properties (Host)
- [ ] Can browse properties
- [ ] Can create bookings (Guest)
- [ ] Can make payments
- [ ] Can cancel bookings
- [ ] Can write reviews
- [ ] Email notifications work (optional)

## ?? You're All Set!

The application is now ready to use. Explore all features:
- User registration with roles
- Property management
- Booking system with conflict detection
- Payment tracking
- Review system
- Email notifications

For any issues, check the console logs or review the README.md file.

Happy coding! ??
