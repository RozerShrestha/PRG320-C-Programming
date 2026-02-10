# ?? Property Rental API Documentation

## Architecture Overview

Your project now follows a **proper layered architecture** with clear separation of concerns:

```
???????????????????????????????????????
?   Razor Pages (Frontend/UI)         ?  ? Makes HTTP requests to API
?   - Uses HttpClient to call APIs    ?
???????????????????????????????????????
              ? HTTP/REST
???????????????????????????????????????
?   Controllers (API Layer)            ?  ? Handles HTTP requests/responses
?   - AuthController                   ?
?   - PropertiesController             ?
?   - BookingsController               ?
?   - ReviewsController                ?
?   - PaymentsController               ?
???????????????????????????????????????
              ? Calls
???????????????????????????????????????
?   Services (Business Logic)          ?  ? Implements business rules
?   - AuthService                      ?
?   - BookingService                   ?
?   - EmailService                     ?
???????????????????????????????????????
              ? Calls
???????????????????????????????????????
?   Repositories (Data Access)         ?  ? Database operations
?   - UserRepository                   ?
?   - PropertyRepository               ?
?   - BookingRepository                ?
?   - ReviewRepository                 ?
?   - PaymentRepository                ?
???????????????????????????????????????
              ?
???????????????????????????????????????
?   Database (SQL Server)              ?
???????????????????????????????????????
```

## ? What Was Changed

### Before (Monolithic):
- Razor Pages directly accessed Repositories and Services
- No API layer
- Tight coupling between UI and business logic

### After (Layered with Controllers):
- **Controllers** handle all HTTP requests
- **DTOs** for request/response models
- **Services** contain business logic
- **Repositories** handle data access
- Razor Pages can now call APIs (or continue direct access temporarily)

---

## ?? New Project Structure

```
PropertyRentalSystem/
??? Controllers/              ? NEW: API Controllers
?   ??? AuthController.cs
?   ??? PropertiesController.cs
?   ??? BookingsController.cs
?   ??? ReviewsController.cs
?   ??? PaymentsController.cs
??? DTOs/                     ? NEW: Data Transfer Objects
?   ??? AuthDtos.cs
?   ??? PropertyDtos.cs
?   ??? BookingDtos.cs
?   ??? ReviewDtos.cs
?   ??? PaymentDtos.cs
??? Services/                 ? Business Logic
?   ??? Interfaces/
?   ??? Implementations/
??? Repositories/             ? Data Access
?   ??? Interfaces/
?   ??? Implementations/
??? Models/                   ? Domain Entities
??? Pages/                    ? Razor Pages (UI)
??? Data/                     ? DbContext
```

---

## ?? API Endpoints

### Base URL
```
https://localhost:{PORT}/api
```

Access Swagger UI at:
```
https://localhost:{PORT}/swagger
```

---

## ?? Authentication Endpoints

### 1. Register User
**POST** `/api/auth/register`

**Request Body:**
```json
{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john@example.com",
  "phoneNumber": "+1234567890",
  "password": "Password@123",
  "role": "Guest"
}
```

**Response:**
```json
{
  "success": true,
  "message": "Registration successful",
  "user": {
    "id": 1,
    "firstName": "John",
    "lastName": "Doe",
    "email": "john@example.com",
    "phoneNumber": "+1234567890",
    "roles": ["Guest"],
    "isActive": true,
    "createdAt": "2026-02-05T10:00:00Z"
  }
}
```

### 2. Login
**POST** `/api/auth/login`

**Request Body:**
```json
{
  "email": "john@example.com",
  "password": "Password@123"
}
```

**Response:**
```json
{
  "success": true,
  "message": "Login successful",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "id": 1,
    "firstName": "John",
    "lastName": "Doe",
    "email": "john@example.com",
    "roles": ["Guest"]
  }
}
```

### 3. Get Current User
**GET** `/api/auth/me`

**Headers:**
```
Authorization: Bearer {token}
```

---

## ?? Property Endpoints

### 1. Get All Properties (Search)
**GET** `/api/properties?city=New York&guests=2`

**Query Parameters:**
- `city` (optional): City name
- `checkIn` (optional): Check-in date (YYYY-MM-DD)
- `checkOut` (optional): Check-out date (YYYY-MM-DD)
- `guests` (optional): Number of guests

**Response:**
```json
[
  {
    "id": 1,
    "title": "Cozy Downtown Apartment",
    "description": "Beautiful apartment...",
    "city": "New York",
    "country": "USA",
    "pricePerNight": 150.00,
    "maxGuests": 4,
    "bedrooms": 2,
    "bathrooms": 1,
    "isActive": true,
    "primaryImage": "https://images.unsplash.com/...",
    "averageRating": 4.5,
    "reviewCount": 10
  }
]
```

### 2. Get Property by ID
**GET** `/api/properties/{id}`

### 3. Get My Properties (Host Only)
**GET** `/api/properties/my-properties`

**Headers:**
```
Authorization: Bearer {token}
```

### 4. Create Property (Host Only)
**POST** `/api/properties`

**Headers:**
```
Authorization: Bearer {token}
```

**Request Body:**
```json
{
  "title": "Modern Beach House",
  "description": "Beautiful beachfront property...",
  "address": "123 Ocean Drive",
  "city": "Miami",
  "country": "USA",
  "zipCode": "33139",
  "pricePerNight": 250.00,
  "maxGuests": 6,
  "bedrooms": 3,
  "bathrooms": 2,
  "imageUrls": [
    "https://images.unsplash.com/photo-1.jpg",
    "https://images.unsplash.com/photo-2.jpg"
  ]
}
```

### 5. Update Property (Host Only)
**PUT** `/api/properties/{id}`

### 6. Delete Property (Host/Admin Only)
**DELETE** `/api/properties/{id}`

---

## ?? Booking Endpoints

### 1. Get My Bookings
**GET** `/api/bookings/my-bookings?status=Confirmed`

**Query Parameters:**
- `status` (optional): Pending, Confirmed, Completed, Cancelled

### 2. Get Booking by ID
**GET** `/api/bookings/{id}`

### 3. Create Booking
**POST** `/api/bookings`

**Request Body:**
```json
{
  "propertyId": 1,
  "checkInDate": "2026-03-15",
  "checkOutDate": "2026-03-20",
  "numberOfGuests": 2,
  "specialRequests": "Late check-in please"
}
```

**Response:**
```json
{
  "success": true,
  "message": "Booking created successfully",
  "booking": {
    "id": 1,
    "propertyId": 1,
    "propertyTitle": "Cozy Apartment",
    "checkInDate": "2026-03-15",
    "checkOutDate": "2026-03-20",
    "totalPrice": 750.00,
    "status": "Pending"
  }
}
```

### 4. Cancel Booking
**POST** `/api/bookings/{id}/cancel`

### 5. Check Availability
**GET** `/api/bookings/check-availability?propertyId=1&checkIn=2026-03-15&checkOut=2026-03-20`

**Response:**
```json
{
  "available": true
}
```

---

## ? Review Endpoints

### 1. Get Property Reviews
**GET** `/api/reviews/property/{propertyId}`

### 2. Get Average Rating
**GET** `/api/reviews/property/{propertyId}/average-rating`

### 3. Create Review (Guest Only)
**POST** `/api/reviews`

**Request Body:**
```json
{
  "bookingId": 1,
  "rating": 5,
  "comment": "Amazing property! Highly recommend."
}
```

---

## ?? Payment Endpoints

### 1. Get Payment by Booking
**GET** `/api/payments/booking/{bookingId}`

### 2. Process Payment (Guest Only)
**POST** `/api/payments/process`

**Request Body:**
```json
{
  "bookingId": 1,
  "paymentMethod": "Credit Card",
  "transactionReference": "TXN123456789"
}
```

---

## ?? Authorization

### JWT Token
All protected endpoints require a JWT token in the Authorization header:

```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### Roles
- **Admin**: Full access
- **Host**: Can manage properties, view bookings for their properties
- **Guest**: Can book properties, write reviews

---

## ?? Testing with Swagger

1. **Start the application**
   ```bash
   dotnet run
   ```

2. **Open Swagger UI**
   ```
   https://localhost:7XXX/swagger
   ```

3. **Authenticate**:
   - Call `/api/auth/login` first
   - Copy the `token` from response
   - Click "Authorize" button in Swagger
   - Enter: `Bearer {your-token}`
   - Click "Authorize"

4. **Test Endpoints**:
   - Now you can test all endpoints
   - Swagger will automatically include the JWT token

---

## ?? Example Usage Flow

### Complete Booking Flow

1. **Register as Guest**
   ```
   POST /api/auth/register
   ```

2. **Login**
   ```
   POST /api/auth/login
   ```

3. **Search Properties**
   ```
   GET /api/properties?city=Miami&guests=2
   ```

4. **Get Property Details**
   ```
   GET /api/properties/1
   ```

5. **Check Availability**
   ```
   GET /api/bookings/check-availability?propertyId=1&checkIn=2026-03-15&checkOut=2026-03-20
   ```

6. **Create Booking**
   ```
   POST /api/bookings
   ```

7. **Process Payment**
   ```
   POST /api/payments/process
   ```

8. **After Stay, Write Review**
   ```
   POST /api/reviews
   ```

---

## ? Key Features

### 1. Proper Separation of Concerns
- **Controllers**: HTTP handling only
- **Services**: Business logic
- **Repositories**: Data access

### 2. DTOs (Data Transfer Objects)
- Request/Response models separate from domain entities
- Clean API contracts
- Validation attributes

### 3. Authorization
- Role-based access control
- JWT authentication
- Attribute-based authorization (`[Authorize(Roles = "Host")]`)

### 4. API Documentation
- Swagger/OpenAPI integration
- Interactive API testing
- Auto-generated documentation

### 5. CORS Support
- API can be called from any frontend
- Ready for React/Angular/Vue integration

---

## ?? Benefits of This Architecture

1. **Scalability**: Easy to add new endpoints
2. **Testability**: Each layer can be tested independently
3. **Maintainability**: Clear separation of concerns
4. **Flexibility**: Can support multiple clients (Web, Mobile, Desktop)
5. **API-First**: Frontend and backend are decoupled

---

## ?? Next Steps

### Option 1: Keep Razor Pages (Hybrid Approach)
- Razor Pages for UI
- Controllers for API
- Best of both worlds

### Option 2: Full API Separation
1. Create a separate React/Angular/Vue frontend
2. Frontend calls these APIs
3. Complete SPA experience

### Option 3: Update Razor Pages to Call APIs
- Modify PageModels to use HttpClient
- Call your own APIs
- Prepare for future SPA migration

---

## ?? Code Examples

### Calling API from Razor Page

```csharp
public class IndexModel : PageModel
{
    private readonly HttpClient _httpClient;
    
    public IndexModel(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri("https://localhost:7XXX/api/");
    }
    
    public async Task<IActionResult> OnGetAsync()
    {
        // Get JWT token from session
        var token = HttpContext.Session.GetString("JwtToken");
        
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", token);
        }
        
        // Call API
        var response = await _httpClient.GetAsync("properties");
        
        if (response.IsSuccessStatusCode)
        {
            Properties = await response.Content
                .ReadFromJsonAsync<List<PropertyListDto>>();
        }
        
        return Page();
    }
}
```

---

## ? Summary

Your project now has:
- ? **5 API Controllers** (Auth, Properties, Bookings, Reviews, Payments)
- ? **DTOs** for clean API contracts
- ? **Swagger** for API documentation
- ? **JWT Authentication** on Controllers
- ? **Role-based Authorization**
- ? **CORS** enabled for cross-origin requests
- ? **Proper layered architecture**: Controllers ? Services ? Repositories

**Architecture**: No longer monolithic! You now have a proper **REST API with Controllers** that follow industry best practices.

?? **Your application is now production-ready with a professional API architecture!**
