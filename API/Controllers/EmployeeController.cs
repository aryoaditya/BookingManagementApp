using API.Contracts;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        // HTTP GET untuk mengambil semua data Employee
        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _employeeRepository.GetAll();
            if (!result.Any())
            {
                return NotFound("Data Not Found"); // Mengembalikan pesan jika tidak ada data yang ditemukan
            }

            return Ok(result);  // Mengembalikan data Employee jika ada
        }

        // HTTP GET untuk mengambil data Employee berdasarkan GUID
        [HttpGet("{guid}")]
        public IActionResult GetByGuid(Guid guid)
        {
            var result = _employeeRepository.GetByGuid(guid);
            if (result is null)
            {
                return NotFound("Id Not Found"); // Mengembalikan pesan jika ID tidak ditemukan
            }
            return Ok(result);  // Mengembalikan data Employee jika ditemukan
        }

        // HTTP POST untuk membuat data Employee baru
        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            var result = _employeeRepository.Create(employee);
            if (result is null)
            {
                return BadRequest("Failed to create data"); // Mengembalikan pesan jika gagal membuat data
            }

            return Ok(result); // Mengembalikan data Employee yang baru saja dibuat
        }

        // HTTP PUT untuk memperbarui data Employee berdasarkan GUID
        [HttpPut("{guid}")]
        public IActionResult Update(Employee employee)
        {
            var result = _employeeRepository.Update(employee);
            if (!result)
            {
                return BadRequest("Failed to update data");  // Mengembalikan pesan jika gagal memperbarui data
            }

            return Ok("Data updated successfully"); // Mengembalikan pesan sukses jika pembaruan berhasil
        }

        // HTTP DELETE untuk menghapus data Employee berdasarkan GUID
        [HttpDelete("{guid}")]
        public IActionResult Delete(Guid guid)
        {
            var existingEmployee = _employeeRepository.GetByGuid(guid);
            if (existingEmployee is null)
            {
                return NotFound("Id Not Found");
            }

            var result = _employeeRepository.Delete(guid);
            if (!result)
            {
                return BadRequest("Failed to delete data");  // Mengembalikan pesan jika gagal menghapus data
            }

            return Ok("Data deleted successfully");  // Mengembalikan pesan sukses jika penghapusan berhasil
        }

    }
}
