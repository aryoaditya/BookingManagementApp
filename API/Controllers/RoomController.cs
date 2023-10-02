using API.Contracts;
using API.Models;
using Microsoft.AspNetCore.Mvc;

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
                return NotFound("Data Not Found"); // Mengembalikan pesan jika tidak ada data yang ditemukan
            }

            return Ok(result);  // Mengembalikan data Room jika ada
        }

        // HTTP GET untuk mengambil data Room berdasarkan GUID
        [HttpGet("{guid}")]
        public IActionResult GetByGuid(Guid guid)
        {
            var result = _roomRepository.GetByGuid(guid);
            if (result is null)
            {
                return NotFound("Id Not Found"); // Mengembalikan pesan jika ID tidak ditemukan
            }
            return Ok(result);  // Mengembalikan data Room jika ditemukan
        }

        // HTTP POST untuk membuat data Room baru
        [HttpPost]
        public IActionResult Create(Room room)
        {
            var result = _roomRepository.Create(room);
            if (result is null)
            {
                return BadRequest("Failed to create data"); // Mengembalikan pesan jika gagal membuat data
            }

            return Ok(result); // Mengembalikan data Room yang baru saja dibuat
        }

        // HTTP PUT untuk memperbarui data Room berdasarkan GUID
        [HttpPut("{guid}")]
        public IActionResult Update(Room room)
        {
            var result = _roomRepository.Update(room);
            if (!result)
            {
                return BadRequest("Failed to update data");  // Mengembalikan pesan jika gagal memperbarui data
            }

            return Ok("Data updated successfully"); // Mengembalikan pesan sukses jika pembaruan berhasil
        }

        // HTTP DELETE untuk menghapus data Room berdasarkan GUID
        [HttpDelete("{guid}")]
        public IActionResult Delete(Guid guid)
        {
            var existingRoom = _roomRepository.GetByGuid(guid);
            if (existingRoom is null)
            {
                return NotFound("Id Not Found");
            }

            var result = _roomRepository.Delete(guid);
            if (!result)
            {
                return BadRequest("Failed to delete data");  // Mengembalikan pesan jika gagal menghapus data
            }

            return Ok("Data deleted successfully");  // Mengembalikan pesan sukses jika penghapusan berhasil
        }

    }
}
