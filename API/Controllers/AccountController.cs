using API.Contracts;
using API.DTOs.Accounts;
using API.DTOs.Roles;
using API.DTOs.Rooms;
using API.Models;
using API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        // HTTP GET untuk mengambil semua data Account
        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _accountRepository.GetAll();
            if (!result.Any())
            {
                return NotFound("Data Not Found"); // Mengembalikan pesan jika tidak ada data yang ditemukan
            }

            var data = result.Select(x => (AccountDto)x);

            return Ok(data);  // Mengembalikan data Account jika ada
        }

        // HTTP GET untuk mengambil data Account berdasarkan GUID
        [HttpGet("{guid}")]
        public IActionResult GetByGuid(Guid guid)
        {
            var result = _accountRepository.GetByGuid(guid);
            if (result is null)
            {
                return NotFound("Id Not Found"); // Mengembalikan pesan jika ID tidak ditemukan
            }
            return Ok((AccountDto)result);  // Mengembalikan data Account jika ditemukan
        }

        // HTTP POST untuk membuat data Account baru
        [HttpPost]
        public IActionResult Create(CreateAccountDto accountDto)
        {
            var result = _accountRepository.Create(accountDto);
            if (result is null)
            {
                return BadRequest("Failed to create data"); // Mengembalikan pesan jika gagal membuat data
            }

            return Ok((AccountDto)result); // Mengembalikan data Account yang baru saja dibuat
        }

        // HTTP PUT untuk memperbarui data Account berdasarkan GUID
        [HttpPut("{guid}")]
        public IActionResult Update(AccountDto accountDto)
        {
            var entity = _accountRepository.GetByGuid(accountDto.Guid);
            if (entity is null)
            {
                return NotFound("Id Not Found");
            }

            Account toUpdate = accountDto;
            toUpdate.CreatedDate = entity.CreatedDate;

            var result = _accountRepository.Update(toUpdate);
            if (!result)
            {
                return BadRequest("Failed to update data");  // Mengembalikan pesan jika gagal memperbarui data
            }

            return Ok("Data updated successfully"); // Mengembalikan pesan sukses jika pembaruan berhasil
        }

        // HTTP DELETE untuk menghapus data Account berdasarkan GUID
        [HttpDelete("{guid}")]
        public IActionResult Delete(Guid guid)
        {
            var existingAccount = _accountRepository.GetByGuid(guid);
            if (existingAccount is null)
            {
                return NotFound("Id Not Found");
            }

            var result = _accountRepository.Delete(existingAccount);
            if (!result)
            {
                return BadRequest("Failed to delete data");  // Mengembalikan pesan jika gagal menghapus data
            }

            return Ok("Data deleted successfully");  // Mengembalikan pesan sukses jika penghapusan berhasil
        }

    }
}
