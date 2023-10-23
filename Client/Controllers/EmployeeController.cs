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
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _repository;

        public EmployeeController(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        [Authorize(Roles = "admin, manager")]
        public IActionResult Index()
        {
            ViewData["PageTitle"] = "Employee Info";

            var fullNameClaim = User.FindFirst(ClaimTypes.Name);
            if (fullNameClaim != null)
            {
                var fullName = fullNameClaim.Value;
                ViewData["FullName"] = fullName;
            }

            return View();
        }

        // GetAll using HttpClient
        public async Task<IActionResult> List()
        {
            ViewData["PageTitle"] = "Employee Info";
            var result = await _repository.Get();

            var listEmployee = new List<EmployeeDto>();
            if (result != null && result.Data != null) // Periksa apakah result dan result.Data tidak null
            {
                listEmployee = result.Data.Select(x => (EmployeeDto)x).ToList();
            }

            return View(listEmployee);
        }

        // Create page using HttpClient
        [HttpGet]
        public async Task<IActionResult> CreateEmp()
        {
            return View();
        }

        // Create object using HttpClient
        [HttpPost]
        public async Task<IActionResult> CreateEmp(CreateEmployeeDto employeeDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _repository.Post(employeeDto);
                if (result.Code == 200)
                {
                    return RedirectToAction(nameof(List));
                }
                else if (result.Code == 409)
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                    return View();
                }
            }
            return View();
        }

        // Update object using HttpClient
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateEmployeeDto employee)
        {
            if (ModelState.IsValid)
            {
                var result = await _repository.Put(employee.Guid, employee);
                if (result.Code == 200)
                {
                    return RedirectToAction(nameof(List));
                }
                else if (result.Code == 409)
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                    return View();
                }
            }
            return View();
        }

        // Edit object view using HttpClient
        [HttpGet]
        public async Task<IActionResult> Edit(Guid guid)
        {
            var result = await _repository.Get(guid);
            var employee = new UpdateEmployeeDto();
            if (result.Data?.Guid is null)
            {
                return View(employee);
            }
            else
            {
                employee.Guid = result.Data.Guid;
                employee.FirstName = result.Data.FirstName;
                employee.LastName = result.Data.LastName;
                employee.Email = result.Data.Email;
                employee.BirthDate = result.Data.BirthDate;
                employee.HiringDate = result.Data.HiringDate;
                employee.PhoneNumber = result.Data.PhoneNumber;
                employee.Nik = result.Data.Nik;
            }

            return View(employee);
        }

        // Delete object confitmation view using HttpClient
        [HttpGet]
        public async Task<IActionResult> DeleteEmp(Guid guid)
        {
            var result = await _repository.Get(guid);
            var employee = new Employee();
            if (result.Data?.Guid is null)
            {
                return View(employee);
            }
            else
            {
                employee.Guid = result.Data.Guid;
                employee.FirstName = result.Data.FirstName;
                employee.LastName = result.Data.LastName;
                employee.Email = result.Data.Email;
                employee.BirthDate = result.Data.BirthDate;
                employee.HiringDate = result.Data.HiringDate;
                employee.PhoneNumber = result.Data.PhoneNumber;
                employee.Nik = result.Data.Nik;
            }
            return View(employee);
        }

        // Delete object using HttpClient
        [HttpPost]
        public async Task<IActionResult> Remove(Guid guid)
        {
            var result = await _repository.Delete(guid);
            if (result.Code == 200)
            {
                return RedirectToAction(nameof(List));
            }
            return View();
        }
    }
}