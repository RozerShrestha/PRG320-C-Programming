using AspNetCoreHero.ToastNotification.Abstractions;
using BusinessManagementSystem.Dto;
using BusinessManagementSystem.Models;
using BusinessManagementSystem.Repositories;
using BusinessManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Encodings.Web;

public abstract class BaseController : Controller
{
    // Service Locator Pattern - Services resolved from DI container at runtime
    protected BasicConfigurationRepository BasicConfigurationRepository =>
        HttpContext?.RequestServices?.GetService(typeof(BasicConfigurationRepository)) as BasicConfigurationRepository;

    protected BaseRepository BaseRepository =>
        HttpContext?.RequestServices?.GetService(typeof(BaseRepository)) as BaseRepository;

    protected INotyfService Notyf =>
        HttpContext?.RequestServices?.GetService(typeof(INotyfService)) as INotyfService;

    protected IEmailSender EmailSender =>
        HttpContext?.RequestServices?.GetService(typeof(IEmailSender)) as IEmailSender;

    protected JavaScriptEncoder JavaScriptEncoder =>
        HttpContext?.RequestServices?.GetService(typeof(JavaScriptEncoder)) as JavaScriptEncoder;

    // Backward compatibility field property
    protected INotyfService _notyf => Notyf;


    //protected readonly BasicConfigurationRepository _basicConfigurationRepository;
    //protected readonly BaseRepository _baseRepository;
    //protected readonly INotyfService _notyf;
    //protected readonly IEmailSender _emailSender;
    //protected readonly JavaScriptEncoder _javaScriptEncoder;
    public static int roleId;
    public static int userId;
    public static string roleName = string.Empty;
    public static string username = string.Empty;
    public static string email = string.Empty;
    public static string fullName = string.Empty;
    public static string PhoneNumber = string.Empty;
    protected UserDto userDto;
    protected UserDto CurrentUser { get; private set; }


    // Default parameterless constructor for new controllers
    public BaseController()
    {
    }
    // Constructor for backward compatibility with existing controllers
    public BaseController(
        BasicConfigurationRepository basicConfigurationRepository,
        BaseRepository baseRepository,
        INotyfService notyf,
        IEmailSender emailSender,
        JavaScriptEncoder javaScriptEncoder)
    {
        // Dependencies are now resolved from DI container via properties
        // This constructor is kept for backward compatibility only
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        CurrentUser = GetUserDetail();

        ViewData["UserDetail"] = CurrentUser;
        ViewData["Menu"] = GetMenuList(CurrentUser?.RoleName);
        ViewData["Title"] = BasicConfigurationRepository?.GetSingleOrDefault()?.Data?.ApplicationTitle ?? "Application";

        base.OnActionExecuting(context);
    }

    protected IActionResult HandleError(Exception ex)
    {
        return StatusCode(500, new { message = ex.Message });
    }

    private UserDto GetUserDetail()
    {
        try
        {
            var claims = User.Identities.FirstOrDefault()?.Claims;
            var loggedInEmail = claims?.FirstOrDefault(x =>
                x.Type.Contains("emailaddress", StringComparison.OrdinalIgnoreCase))?.Value;

            if (string.IsNullOrEmpty(loggedInEmail))
                return null;

            var userDto = BaseRepository?.UserDetail(loggedInEmail);
            //these are base variables which will be shared among all the controller which enherite base controller
            if (userDto != null)
            {
                // Populate static properties for backward compatibility
                userId = userDto.UserId;
                username = userDto.UserName ?? string.Empty;
                email = userDto.Email ?? string.Empty;
                PhoneNumber = userDto.PhoneNumber ?? string.Empty;
                roleId = userDto.RoleId;
                roleName = userDto.RoleName ?? string.Empty;
                fullName = userDto.FullName ?? string.Empty;
            }
            return userDto;
        }
        catch
        {
            return null;
        }
    }

    private List<MenuDto> GetMenuList(string roleName)
    {
        return string.IsNullOrEmpty(roleName)
            ? new List<MenuDto>()
            : BaseRepository?.MenuList(roleName) ?? new List<MenuDto>();
    }

    protected bool IsAuthorized(int userId)
    {
        return (CurrentUser?.RoleName == "admin" || CurrentUser?.RoleName == "hradmin")
               || CurrentUser?.UserId == userId;
    }

    protected string EncodeString(string text)
    {
        return JavaScriptEncoder?.Encode(text) ?? text ?? string.Empty;
    }
}
