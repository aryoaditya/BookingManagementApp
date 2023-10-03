using API.Contracts;
using API.DTOs.Employees;
using API.DTOs.Rooms;
using API.Models;
using API.Utilities.Handlers;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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

            var data = result.Select(x => (EmployeeDto)x);

            return Ok(data);  // Mengembalikan data Employee jika ada
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
            return Ok((EmployeeDto)result);  // Mengembalikan data Employee jika ditemukan
        }

        // HTTP POST untuk membuat data Employee baru
        [HttpPost]
        public IActionResult Create(CreateEmployeeDto employeeDto)
        {
            //Employee toCreate = employeeDto;
            //toCreate.Nik = _generateHandler.GenerateNIK();

            var result = _employeeRepository.Create(employeeDto);
            if (result is null)
            {
                return BadRequest("Failed to create data"); // Mengembalikan pesan jika gagal membuat data
            }

            return Ok((EmployeeDto)result); // Mengembalikan data Employee yang baru saja dibuat
        }

        // HTTP PUT untuk memperbarui data Employee berdasarkan GUID
        [HttpPut]
        public IActionResult Update(EmployeeDto employeeDto)
        {
            var entity = _employeeRepository.GetByGuid(employeeDto.Guid);
            if (entity is null)
            {
                return NotFound("Id Not Found");
            }

            Employee toUpdate = employeeDto;
            toUpdate.CreatedDate = entity.CreatedDate;

            var result = _employeeRepository.Update(toUpdate);
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

            var result = _employeeRepository.Delete(existingEmployee);
            if (!result)
            {
                return BadRequest("Failed to delete data");  // Mengembalikan pesan jika gagal menghapus data
            }

            return Ok("Data deleted successfully");  // Mengembalikan pesan sukses jika penghapusan berhasil
        }

    }
}
