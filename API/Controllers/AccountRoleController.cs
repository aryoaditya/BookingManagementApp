using API.Contracts;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountRoleController : ControllerBase
    {
        private readonly IAccountRoleRepository _accountRoleRepository;

        public AccountRoleController(IAccountRoleRepository accountRoleRepository)
        {
            _accountRoleRepository = accountRoleRepository;
        }

        // HTTP GET untuk mengambil semua data AccountRole
        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _accountRoleRepository.GetAll();
            if (!result.Any())
            {
                return NotFound("Data Not Found"); // Mengembalikan pesan jika tidak ada data yang ditemukan
            }

            return Ok(result);  // Mengembalikan data AccountRole jika ada
        }

        // HTTP GET untuk mengambil data AccountRole berdasarkan GUID
        [HttpGet("{guid}")]
        public IActionResult GetByGuid(Guid guid)
        {
            var result = _accountRoleRepository.GetByGuid(guid);
            if (result is null)
            {
                return NotFound("Id Not Found"); // Mengembalikan pesan jika ID tidak ditemukan
            }
            return Ok(result);  // Mengembalikan data AccountRole jika ditemukan
        }

        // HTTP POST untuk membuat data AccountRole baru
        [HttpPost]
        public IActionResult Create(AccountRole accountRole)
        {
            var result = _accountRoleRepository.Create(accountRole);
            if (result is null)
            {
                return BadRequest("Failed to create data"); // Mengembalikan pesan jika gagal membuat data
            }

            return Ok(result); // Mengembalikan data AccountRole yang baru saja dibuat
        }

        // HTTP PUT untuk memperbarui data AccountRole berdasarkan GUID
        [HttpPut("{guid}")]
        public IActionResult Update(AccountRole accountRole)
        {
            var result = _accountRoleRepository.Update(accountRole);
            if (!result)
            {
                return BadRequest("Failed to update data");  // Mengembalikan pesan jika gagal memperbarui data
            }

            return Ok("Data updated successfully"); // Mengembalikan pesan sukses jika pembaruan berhasil
        }

        // HTTP DELETE untuk menghapus data AccountRole berdasarkan GUID
        [HttpDelete("{guid}")]
        public IActionResult Delete(Guid guid)
        {
            var existingAccountRole = _accountRoleRepository.GetByGuid(guid);
            if (existingAccountRole is null)
            {
                return NotFound("Id Not Found");
            }

            var result = _accountRoleRepository.Delete(guid);
            if (!result)
            {
                return BadRequest("Failed to delete data");  // Mengembalikan pesan jika gagal menghapus data
            }

            return Ok("Data deleted successfully");  // Mengembalikan pesan sukses jika penghapusan berhasil
        }

    }
}
