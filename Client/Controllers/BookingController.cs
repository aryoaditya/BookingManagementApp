using API.DTOs.Employees;
using API.Models;
using API.Utilities.Handlers;
using Client.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace Client.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingRepository _repository;

        public BookingController(IBookingRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            ViewData["PageTitle"] = "Booking Detail";

            var fullNameClaim = User.FindFirst(ClaimTypes.Name);
            if (fullNameClaim != null)
            {
                var fullName = fullNameClaim.Value;
                ViewData["FullName"] = fullName;
            }

            return View();
        }

        public async Task<JsonResult> GetDetail()
        {
            var result = await _repository.GetDetail();
            return Json(result);
        }
    }
}