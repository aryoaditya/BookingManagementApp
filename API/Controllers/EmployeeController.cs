using API.Contracts;
using API.DTOs.Employees;
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
                // Mengembalikan pesan jika tidak ada data yang ditemukan
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                }); 
            }

            var data = result.Select(x => (EmployeeDto)x);

            // Mengembalikan data Employee jika ada
            return Ok(new ResponseOkHandler<IEnumerable<EmployeeDto>>(data));  
        }

        // HTTP GET untuk mengambil data Employee berdasarkan GUID
        [HttpGet("{guid}")]
        public IActionResult GetByGuid(Guid guid)
        {
            var result = _employeeRepository.GetByGuid(guid);
            if (result is null)
            {
                // Mengembalikan pesan jika ID tidak ditemukan
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "ID Not Found"
                }); 
            }
            
            // Mengembalikan data Employee jika ditemukan
            return Ok(new ResponseOkHandler<EmployeeDto>((EmployeeDto)result));
        }

        // HTTP POST untuk membuat data Employee baru
        [HttpPost]
        public IActionResult Create(CreateEmployeeDto employeeDto)
        {
            try
            {
                Employee toCreate = employeeDto;
                toCreate.Nik = GenerateHandler.GenerateNIK(_employeeRepository.GetLastNik());
                var result = _employeeRepository.Create(toCreate);

                // Mengembalikan data Employee yang baru saja dibuat
                return Ok(new ResponseOkHandler<EmployeeDto>((EmployeeDto)result));
            }
            catch (ExceptionHandler ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Failed to create data",
                    Error = ex.Message
                });
            }             
        }

        // HTTP PUT untuk memperbarui data Employee berdasarkan GUID
        [HttpPut]
        public IActionResult Update(EmployeeDto employeeDto)
        {
            try
            {
                var entity = _employeeRepository.GetByGuid(employeeDto.Guid);
                if (entity is null)
                {
                    return NotFound(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Id Not Found"
                    });
                }

                Employee toUpdate = employeeDto;
                toUpdate.CreatedDate = entity.CreatedDate;
                toUpdate.Nik = entity.Nik;

                _employeeRepository.Update(toUpdate);

                return Ok(new ResponseOkHandler<EmployeeDto>("Data updated successfully")); // Mengembalikan pesan sukses jika pembaruan berhasil
            }
            catch (ExceptionHandler ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Failed to update data",
                    Error = ex.Message
                });
            }
        }

        // HTTP DELETE untuk menghapus data Employee berdasarkan GUID
        [HttpDelete("{guid}")]
        public IActionResult Delete(Guid guid)
        {
            try
            {
                var existingEmployee = _employeeRepository.GetByGuid(guid);
                if (existingEmployee is null)
                {
                    return NotFound(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Id Not Found"
                    });
                }

                _employeeRepository.Delete(existingEmployee);

                return Ok(new ResponseOkHandler<EmployeeDto>("Data deleted successfully"));  // Mengembalikan pesan sukses jika penghapusan berhasil
            }
            catch (ExceptionHandler ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Failed to delete data",
                    Error = ex.Message
                });
            }
        }
    }
}
