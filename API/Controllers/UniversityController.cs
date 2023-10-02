using API.Contracts;
using API.Models;
using Microsoft.AspNetCore.Mvc;

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
                return NotFound("Data Not Found"); // Mengembalikan pesan jika tidak ada data yang ditemukan
            }

            return Ok(result);  // Mengembalikan data Universitas jika ada
        }

        // HTTP GET untuk mengambil data Universitas berdasarkan GUID
        [HttpGet("{guid}")]
        public IActionResult GetByGuid(Guid guid)
        {
            var result = _universityRepository.GetByGuid(guid);
            if (result is null)
            {
                return NotFound("Id Not Found"); // Mengembalikan pesan jika ID tidak ditemukan
            }
            return Ok(result);  // Mengembalikan data Universitas jika ditemukan
        }

        // HTTP POST untuk membuat data Universitas baru
        [HttpPost]
        public IActionResult Create(University university)
        {
            var result = _universityRepository.Create(university);
            if (result is null)
            {
                return BadRequest("Failed to create data"); // Mengembalikan pesan jika gagal membuat data
            }

            return Ok(result); // Mengembalikan data Universitas yang baru saja dibuat
        }

        // HTTP PUT untuk memperbarui data Universitas berdasarkan GUID
        [HttpPut("{guid}")]
        public IActionResult Update(University university)
        {
            var result = _universityRepository.Update(university);
            if (!result)
            {
                return BadRequest("Failed to update data");  // Mengembalikan pesan jika gagal memperbarui data
            }

            return Ok("Data updated successfully"); // Mengembalikan pesan sukses jika pembaruan berhasil
        }

        // HTTP DELETE untuk menghapus data Universitas berdasarkan GUID
        [HttpDelete("{guid}")]
        public IActionResult Delete(Guid guid)
        {
            var existingUniversity = _universityRepository.GetByGuid(guid);
            if (existingUniversity is null)
            {
                return NotFound("Id Not Found");
            }

            var result = _universityRepository.Delete(existingUniversity);
            if (!result)
            {
                return BadRequest("Failed to delete data");  // Mengembalikan pesan jika gagal menghapus data
            }

            return Ok("Data deleted successfully");  // Mengembalikan pesan sukses jika penghapusan berhasil
        }

    }
}
