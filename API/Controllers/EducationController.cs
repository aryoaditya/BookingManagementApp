using API.Contracts;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
                return NotFound("Data Not Found"); // Mengembalikan pesan jika tidak ada data yang ditemukan
            }

            return Ok(result);  // Mengembalikan data Education jika ada
        }

        // HTTP GET untuk mengambil data Education berdasarkan GUID
        [HttpGet("{guid}")]
        public IActionResult GetByGuid(Guid guid)
        {
            var result = _educationRepository.GetByGuid(guid);
            if (result is null)
            {
                return NotFound("Id Not Found"); // Mengembalikan pesan jika ID tidak ditemukan
            }
            return Ok(result);  // Mengembalikan data Education jika ditemukan
        }

        // HTTP POST untuk membuat data Education baru
        [HttpPost]
        public IActionResult Create(Education education)
        {
            var result = _educationRepository.Create(education);
            if (result is null)
            {
                return BadRequest("Failed to create data"); // Mengembalikan pesan jika gagal membuat data
            }

            return Ok(result); // Mengembalikan data Education yang baru saja dibuat
        }

        // HTTP PUT untuk memperbarui data Education berdasarkan GUID
        [HttpPut("{guid}")]
        public IActionResult Update(Education education)
        {
            var result = _educationRepository.Update(education);
            if (!result)
            {
                return BadRequest("Failed to update data");  // Mengembalikan pesan jika gagal memperbarui data
            }

            return Ok("Data updated successfully"); // Mengembalikan pesan sukses jika pembaruan berhasil
        }

        // HTTP DELETE untuk menghapus data Education berdasarkan GUID
        [HttpDelete("{guid}")]
        public IActionResult Delete(Guid guid)
        {
            var existingEducation = _educationRepository.GetByGuid(guid);
            if (existingEducation is null)
            {
                return NotFound("Id Not Found");
            }

            var result = _educationRepository.Delete(guid);
            if (!result)
            {
                return BadRequest("Failed to delete data");  // Mengembalikan pesan jika gagal menghapus data
            }

            return Ok("Data deleted successfully");  // Mengembalikan pesan sukses jika penghapusan berhasil
        }

    }
}
