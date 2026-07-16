using Microsoft.AspNetCore.Mvc;
using Pozdravlyator.Services;

namespace Pozdravlyator.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBirthdayService _birthdayService;

        public HomeController(IBirthdayService birthdayService)
        {
            _birthdayService = birthdayService;
        }

        public async Task<IActionResult> Index()
        {
            var upcoming = await _birthdayService.GetTodayAndUpcomingAsync(daysAhead: 14);
            return View(upcoming);
        }
    }
}