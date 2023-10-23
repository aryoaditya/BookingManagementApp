using Client.Contracts;
using Client.Models;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using System.Diagnostics;
using System.Security.Claims;

namespace Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IClaimsRepository _claimsRepository;

        public HomeController(ILogger<HomeController> logger, IClaimsRepository claimsRepository)
        {
            _logger = logger;
            _claimsRepository = claimsRepository;
        }

        public IActionResult Index()
        {
            ViewData["PageTitle"] = "Dashboard";

            var fullNameClaim = User.FindFirst(ClaimTypes.Name);
            if (fullNameClaim != null)
            {
                var fullName = fullNameClaim.Value;
                ViewData["FullName"] = fullName;
            }

            return View();
        }

        public async Task<JsonResult> GetClaims()
        {
            var result = await _claimsRepository.Get();
            return Json(result);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}