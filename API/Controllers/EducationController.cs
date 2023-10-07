using API.Contracts;
using API.DTOs.Educations;
using API.Models;
using API.Utilities.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EducationController : ControllerBase
    {
        private readonly IEducationRepository _educationRepository;

        public EducationController(IEducationRepository educationRepository)
        {
            _educationRepository = educationRepository;
        }

        // HTTP GET untuk mengambil semua data Education
        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _educationRepository.GetAll();
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

            var data = result.Select(x => (EducationDto)x);

            // Mengembalikan data Employee jika ada
            return Ok(new ResponseOkHandler<IEnumerable<EducationDto>>(data));
        }

        // HTTP GET untuk mengambil data Education berdasarkan GUID
        [HttpGet("{guid}")]
        public IActionResult GetByGuid(Guid guid)
        {
            var result = _educationRepository.GetByGuid(guid);
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
            return Ok(new ResponseOkHandler<EducationDto>((EducationDto)result));
        }

        // HTTP POST untuk membuat data Education baru
        [HttpPost]
        public IActionResult Create(CreateEducationDto educationDto)
        {
            try
            {
                var result = _educationRepository.Create(educationDto);

                // Mengembalikan data Employee yang baru saja dibuat
                return Ok(new ResponseOkHandler<EducationDto>((EducationDto)result));
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

        // HTTP PUT untuk memperbarui data Education berdasarkan GUID
        [HttpPut]
        public IActionResult Update(EducationDto educationDto)
        {
            try
            {
                var entity = _educationRepository.GetByGuid(educationDto.Guid);
                if (entity is null)
                {
                    return NotFound(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Id Not Found"
                    });
                }

                Education toUpdate = educationDto;
                toUpdate.CreatedDate = entity.CreatedDate;

                _educationRepository.Update(toUpdate);

                return Ok(new ResponseOkHandler<EducationDto>("Data updated successfully")); // Mengembalikan pesan sukses jika pembaruan berhasil
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

        // HTTP DELETE untuk menghapus data Education berdasarkan GUID
        [HttpDelete("{guid}")]
        [Authorize(Roles = "admin")]
        public IActionResult Delete(Guid guid)
        {
            try
            {
                var existingEducation = _educationRepository.GetByGuid(guid);
                if (existingEducation is null)
                {
                    return NotFound(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Id Not Found"
                    });
                }

                _educationRepository.Delete(existingEducation);

                return Ok(new ResponseOkHandler<EducationDto>("Data deleted successfully"));  // Mengembalikan pesan sukses jika penghapusan berhasil
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