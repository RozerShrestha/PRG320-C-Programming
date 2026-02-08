# BaseController Refactoring - Service Locator Pattern

## Overview
The `BaseController` has been refactored to use **service locator pattern** through dependency injection, eliminating the need to pass all dependencies to every derived controller constructor.

## Benefits

? **No More Dependency Parameter Passing**: New controllers only need to inject their specific dependencies  
? **Cleaner Code**: Derived controllers have simpler constructors  
? **Backward Compatible**: Existing controllers continue to work without modifications  
? **Lazy Loading**: Dependencies are resolved on-demand from the DI container  
? **Consistency**: All properties use null-safe operators (`?.`)  

---

## How to Use

### Before (Old Way)
```csharp
public class DashboardController : BaseController
{
    private readonly IWebHostEnvironment _env;
    
    public DashboardController(
        BasicConfigurationRepository basicConfigurationRepository,  // Required for base
        BaseRepository baseRepository,                              // Required for base
        IWebHostEnvironment env,                                    // Actual dependency
        ILogger<DashboardController> logger,                       // Actual dependency
        INotyfService notyf,                                       // Required for base
        IEmailSender emailSender,                                  // Required for base
        JavaScriptEncoder javaScriptEncoder)                       // Required for base
        : base(basicConfigurationRepository, baseRepository, notyf, emailSender, javaScriptEncoder)
    {
        _env = env;
    }
}
```

### After (New Way)
```csharp
public class DashboardController : BaseController
{
    private readonly IWebHostEnvironment _env;
    
    public DashboardController(IWebHostEnvironment env, ILogger<DashboardController> logger)
    {
        _env = env;
    }
}
```

---

## Available Protected Properties

### Service Properties (Auto-resolved from DI)
- `BasicConfigurationRepository` - Access to basic configuration data
- `BaseRepository` - Access to user and menu data
- `Notyf` - Notification service
- `EmailSender` - Email sending service
- `JavaScriptEncoder` - JavaScript encoder
- `_notyf` - Backward compatible alias for `Notyf`

### User Context Properties
- `CurrentUser` - Complete user DTO (UserDto)
- `userId` - Current user's ID
- `username` - Current user's username
- `email` - Current user's email
- `fullName` - Current user's full name
- `PhoneNumber` - Current user's phone number
- `roleId` - Current user's role ID
- `roleName` - Current user's role name

### Helper Methods
- `IsAuthorized(int userId)` - Check if current user is authorized
- `EncodeString(string text)` - JavaScript encode a string
- `HandleError(Exception ex)` - Handle exceptions with proper HTTP response

---

## Examples

### Using Base Properties
```csharp
[Authorize]
public class UsersController : BaseController
{
    private readonly UserRepository _userRepository;
    
    public UsersController(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public IActionResult Edit(int id)
    {
        // Check authorization
        if (!IsAuthorized(id))
        {
            Notyf.Warning($"{fullName} is not authorized");
            return Forbid();
        }
        
        // Use base repositories
        var user = BaseRepository.UserDetail(email);
        
        return View(user);
    }
}
```

### Creating New Notification
```csharp
public IActionResult Create(UserDto userDto)
{
    var result = _userRepository.CreateUser(userDto);
    
    if (result.StatusCode == HttpStatusCode.OK)
    {
        Notyf.Success("User created successfully");
    }
    else
    {
        Notyf.Error(result.Message);
    }
    
    return RedirectToAction(nameof(Index));
}
```

### Getting User Details
```csharp
public IActionResult Profile()
{
    // CurrentUser is automatically populated in OnActionExecuting
    ViewData["UserDetail"] = CurrentUser;
    ViewData["FullName"] = fullName;
    ViewData["Email"] = email;
    
    return View();
}
```

---

## Migration Guide for Existing Controllers

If you have a controller with the old pattern:

**Before:**
```csharp
public MenuController(
    INotyfService notyf, 
    IEmailSender emailSender, 
    ILogger<MenuController> logger, 
    JavaScriptEncoder javaScriptEncoder) 
    : base(notyf, emailSender, javaScriptEncoder)
```

**After:**
```csharp
public MenuController(ILogger<MenuController> logger)
{
    _logger = logger;
}
```

Then replace all `_notyf` calls with `Notyf`:
- `_notyf.Success()` ? `Notyf.Success()`
- `_notyf.Error()` ? `Notyf.Error()`
- `_notyf.Warning()` ? `Notyf.Warning()`

Or keep using `_notyf` (backward compatible property alias).

---

## Technical Details

### Service Resolution
Dependencies are resolved lazily from `HttpContext.RequestServices` using:
```csharp
protected INotyfService Notyf => HttpContext?.RequestServices?.GetService(typeof(INotyfService)) as INotyfService;
```

This approach:
- ? Resolves services on-first-access
- ? Handles null cases gracefully with null-conditional operators
- ? Works without explicit constructor injection
- ? Maintains backward compatibility

### Why Service Locator?
While dependency injection is generally preferred, service locator is useful here for:
1. **Inheritance Hierarchies**: Base class dependencies don't need to cascade down
2. **Backward Compatibility**: Existing code works without changes
3. **Optional Dependencies**: Not all controllers need all base services
4. **Controller Simplification**: Focus on actual controller-specific dependencies

---

## Summary

The refactored `BaseController` significantly simplifies controller constructors while maintaining full functionality and backward compatibility. New controllers are now easier to create with minimal boilerplate code.
