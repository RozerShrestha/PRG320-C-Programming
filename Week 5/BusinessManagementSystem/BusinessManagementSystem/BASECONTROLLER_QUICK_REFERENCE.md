# BaseController Quick Reference

## Constructor Patterns

### ? New Pattern (Recommended)
```csharp
public class MyController : BaseController
{
    private readonly IMyService _myService;
    
    public MyController(IMyService myService)
    {
        _myService = myService;
    }
}
```

### ? Still Supported (Backward Compatible)
```csharp
public class MyController : BaseController
{
    private readonly IMyService _myService;
    
    public MyController(
        IMyService myService,
        INotyfService notyf,
        IEmailSender emailSender,
        JavaScriptEncoder javaScriptEncoder)
        : base(notyf, emailSender, javaScriptEncoder)
    {
        _myService = myService;
    }
}
```

---

## Common Use Cases

### Check User Authorization
```csharp
if (IsAuthorized(userId))
{
    // Proceed
}
```

### Show Success Notification
```csharp
Notyf.Success("Operation completed");
```

### Show Error Notification  
```csharp
Notyf.Error("Operation failed");
```

### Access Current User Info
```csharp
var user = CurrentUser;
string name = fullName;
string role = roleName;
int id = userId;
```

### Use Base Repositories
```csharp
var menuList = BaseRepository.MenuList(roleName);
var userDetail = BaseRepository.UserDetail(email);
var config = BasicConfigurationRepository.GetSingleOrDefault();
```

### Send Email
```csharp
await EmailSender.SendEmailAsync(email, subject, message);
```

### Encode JavaScript String
```csharp
var encoded = EncodeString(userInput);
```

---

## Migration Checklist

- [ ] Remove base class constructor parameters (keep only specific dependencies)
- [ ] Update constructor base() call or remove if using default constructor
- [ ] Replace `_notyf` with `Notyf` (or keep using `_notyf` alias)
- [ ] Replace `_baseRepository` with `BaseRepository` property
- [ ] Replace `_basicConfigurationRepository` with `BasicConfigurationRepository` property
- [ ] Replace static properties like `userId`, `roleName` with protected properties
- [ ] Test all controller actions work correctly
- [ ] Build and verify no compilation errors

---

## What Changed?

| Before | After |
|--------|-------|
| Constructor params: 7 | Constructor params: Only your specific ones |
| Field injection required | Property injection via DI container |
| Static properties in base | Protected properties that use CurrentUser |
| Manual user data mapping | Automatic via OnActionExecuting |

---

## No Changes Needed In

- Views (no changes required)
- Dependency registration in Program.cs/Startup.cs
- Route definitions
- Action method signatures
- ViewData usage

---

## Key Takeaway

**No more passing `BasicConfigurationRepository`, `BaseRepository`, `INotyfService`, `IEmailSender`, and `JavaScriptEncoder` to every derived controller!**

Just inherit from `BaseController` and use the properties. It's that simple! ??
