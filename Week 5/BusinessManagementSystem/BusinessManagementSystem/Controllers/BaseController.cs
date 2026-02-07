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
    protected readonly BasicConfigurationRepository _basicConfigurationRepository;
    protected readonly BaseRepository _baseRepository;
    protected readonly INotyfService _notyf;
    protected readonly IEmailSender _emailSender;
    protected readonly JavaScriptEncoder _javaScriptEncoder;
    public static int roleId;
    public static int userId;
    public static string roleName = string.Empty;
    public static string username = string.Empty;
    public static string email = string.Empty;
    public static string fullName = string.Empty;
    public static string PhoneNumber = string.Empty;
    protected UserDto userDto;
    protected UserDto CurrentUser { get; private set; }

    public BaseController(
        BasicConfigurationRepository basicConfigurationRepository,
        BaseRepository baseRepository,
        INotyfService notyf,
        IEmailSender emailSender,
        JavaScriptEncoder javaScriptEncoder)
    {
        _basicConfigurationRepository = basicConfigurationRepository;
        _baseRepository = baseRepository;
        _notyf = notyf;
        _emailSender = emailSender;
        _javaScriptEncoder = javaScriptEncoder;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        CurrentUser = GetUserDetail();

        ViewData["UserDetail"] = CurrentUser;
        ViewData["Menu"] = GetMenuList(CurrentUser?.RoleName);
        ViewData["Title"] = _basicConfigurationRepository.GetSingleOrDefault().Data.ApplicationTitle;

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

            var userDto = _baseRepository.UserDetail(loggedInEmail);
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
            : _baseRepository.MenuList(roleName);
    }

    protected bool IsAuthorized(int userId)
    {
        return (CurrentUser?.RoleName == "admin" || CurrentUser?.RoleName == "hradmin")
               || CurrentUser?.UserId == userId;
    }

    protected string EncodeString(string text)
    {
        return _javaScriptEncoder.Encode(text);
    }
}
