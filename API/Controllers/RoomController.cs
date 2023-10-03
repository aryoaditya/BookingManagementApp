using API.Contracts;
using API.DTOs.Rooms;
using API.Models;
using API.Utilities.Handlers;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly IRoomRepository _roomRepository;

        public RoomController(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        // HTTP GET untuk mengambil semua data Room
        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _roomRepository.GetAll();
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

            var data = result.Select(x => (RoomDto)x);

            return Ok(new ResponseOkHandler<IEnumerable<RoomDto>>(data));  // Mengembalikan data Room jika ada
        }

        // HTTP GET untuk mengambil data Room berdasarkan GUID
        [HttpGet("{guid}")]
        public IActionResult GetByGuid(Guid guid)
        {
            var result = _roomRepository.GetByGuid(guid);
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
            return Ok(new ResponseOkHandler<RoomDto>((RoomDto)result));  // Mengembalikan data Room jika ditemukan
        }

        // HTTP POST untuk membuat data Room baru
        [HttpPost]
        public IActionResult Create(CreateRoomDto roomDto)
        {
            try
            {
                var result = _roomRepository.Create(roomDto);

                return Ok(new ResponseOkHandler<RoomDto>((RoomDto)result)); // Mengembalikan data Room yang baru saja dibuat berhasil
            }
            catch (ExceptionHandler ex)
            {
                // Mengembalikan pesan jika gagal membuat data
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Failed to create data",
                    Error = ex.Message
                });
            }
            
        }

        // HTTP PUT untuk memperbarui data Room berdasarkan GUID
        [HttpPut]
        public IActionResult Update(RoomDto roomDto)
        {
            try
            {
                var entity = _roomRepository.GetByGuid(roomDto.Guid);

                Room toUpdate = roomDto;
                toUpdate.CreatedDate = entity.CreatedDate;

                _roomRepository.Update(toUpdate);

                return Ok(new ResponseOkHandler<RoomDto>("Data updated successfully")); // Mengembalikan pesan sukses jika pembaruan berhasil
            }
            catch (ExceptionHandler ex)
            {
                // Mengembalikan pesan jika gagal membuat data
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Failed to update data",
                    Error = ex.Message
                });
            }            
        }

        // HTTP DELETE untuk menghapus data Room berdasarkan GUID
        [HttpDelete("{guid}")]
        public IActionResult Delete(Guid guid)
        {
            try
            {
                var existingRoom = _roomRepository.GetByGuid(guid);

                _roomRepository.Delete(existingRoom);

                return Ok(new ResponseOkHandler<RoomDto>("Data deleted successfully"));  // Mengembalikan pesan sukses jika penghapusan berhasil
            }
            catch (ExceptionHandler ex)
            {
                // Mengembalikan pesan jika gagal membuat data
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
