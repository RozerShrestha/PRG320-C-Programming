using Microsoft.AspNetCore.Mvc;
using PropertyRentalSystem.Models;
using PropertyRentalSystem.Repositories.Interfaces;

namespace PropertyRentalSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPropertyRepository _propertyRepository;

        public HomeController(ILogger<HomeController> logger, IPropertyRepository propertyRepository)
        {
            _logger = logger;
            _propertyRepository = propertyRepository;
        }

        // GET: /Home/Index or /
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Home page visited");
            
            var properties = await _propertyRepository.GetActivePropertiesAsync();
            var featuredProperties = properties.Take(6).ToList();
            
            return View(featuredProperties);
        }

        // GET: /Home/Privacy
        public IActionResult Privacy()
        {
            return View();
        }

        // GET: /Home/Error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
