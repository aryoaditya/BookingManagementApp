using API.Contracts;
using API.DTOs.Universities;
using API.Models;
using API.Utilities.Handlers;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UniversityController : ControllerBase
    {
        private readonly IUniversityRepository _universityRepository;

        public UniversityController(IUniversityRepository universityRepository)
        {
            _universityRepository = universityRepository;
        }

        // HTTP GET untuk mengambil semua data Universitas
        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _universityRepository.GetAll();
            if (!result.Any())
            {
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                }); // Mengembalikan pesan jika tidak ada data yang ditemukan
            }

            var data = result.Select(x => (UniversityDto)x);

            /*var universityDto = new List<UniversityDto>();
            foreach (var university in result)
            {
                universityDto.Add((UniversityDto) university);
            }*/

            // Mengembalikan data Universitas jika ada
            return Ok(new ResponseOkHandler<IEnumerable<UniversityDto>>(data));
        }

        // HTTP GET untuk mengambil data Universitas berdasarkan GUID
        [HttpGet("{guid}")]
        public IActionResult GetByGuid(Guid guid)
        {
            var result = _universityRepository.GetByGuid(guid);
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

            // Mengembalikan data Universitas jika ditemukan
            return Ok(new ResponseOkHandler<UniversityDto>((UniversityDto)result));
        }

        // HTTP POST untuk membuat data Universitas baru
        [HttpPost]
        public IActionResult Create(CreateUniversityDto universityDto)
        {
            try
            {
                var result = _universityRepository.Create(universityDto);

                // Mengembalikan data Universitas yang baru saja dibuat
                return Ok(new ResponseOkHandler<UniversityDto>((UniversityDto)result));
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

        // HTTP PUT untuk memperbarui data Universitas berdasarkan GUID
        [HttpPut]
        public IActionResult Update(UniversityDto universityDto)
        {
            try
            {
                var entity = _universityRepository.GetByGuid(universityDto.Guid);
                if (entity is null)
                {
                    return NotFound(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "ID Not Found"
                    });
                }

                University toUpdate = universityDto;
                toUpdate.CreatedDate = entity.CreatedDate;

                _universityRepository.Update(toUpdate);

                // Mengembalikan pesan sukses jika pembaruan berhasil
                return Ok(new ResponseOkHandler<UniversityDto>("Data updated successfully"));
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

        // HTTP DELETE untuk menghapus data Universitas berdasarkan GUID
        [HttpDelete("{guid}")]
        public IActionResult Delete(Guid guid)
        {
            try
            {
                var existingUniversity = _universityRepository.GetByGuid(guid);
                if (existingUniversity is null)
                {
                    return NotFound(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "ID Not Found"
                    });
                }

                _universityRepository.Delete(existingUniversity);

                // Mengembalikan pesan sukses jika penghapusan berhasil
                return Ok(new ResponseOkHandler<UniversityDto>("Data deleted successfully"));
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