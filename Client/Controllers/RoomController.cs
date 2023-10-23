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
    public class RoomController : Controller
    {
        private readonly IRoomRepository repository;

        public RoomController(IRoomRepository repository)
        {
            this.repository = repository;
        }

        [Authorize]
        public IActionResult Index()
        {
            ViewData["PageTitle"] = "Room Info";

            var fullNameClaim = User.FindFirst(ClaimTypes.Name);
            if (fullNameClaim != null)
            {
                var fullName = fullNameClaim.Value;
                ViewData["FullName"] = fullName;
            }

            return View();
        }

        public async Task<JsonResult> GetAll()
        {
            var result = await repository.Get();
            return Json(result);
        }
    }
}