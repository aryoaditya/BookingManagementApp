using API.Contracts;
using API.DTOs.Roles;
using API.DTOs.Rooms;
using API.Models;
using API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;

        public RoleController(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        // HTTP GET untuk mengambil semua data Role
        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _roleRepository.GetAll();
            if (!result.Any())
            {
                return NotFound("Data Not Found"); // Mengembalikan pesan jika tidak ada data yang ditemukan
            }

            var data = result.Select(x => (RoleDto)x);

            return Ok(data);  // Mengembalikan data Role jika ada
        }

        // HTTP GET untuk mengambil data Role berdasarkan GUID
        [HttpGet("{guid}")]
        public IActionResult GetByGuid(Guid guid)
        {
            var result = _roleRepository.GetByGuid(guid);
            if (result is null)
            {
                return NotFound("Id Not Found"); // Mengembalikan pesan jika ID tidak ditemukan
            }
            return Ok((RoleDto)result);  // Mengembalikan data Role jika ditemukan
        }

        // HTTP POST untuk membuat data Role baru
        [HttpPost]
        public IActionResult Create(CreateRoleDto roleDto)
        {
            var result = _roleRepository.Create(roleDto);
            if (result is null)
            {
                return BadRequest("Failed to create data"); // Mengembalikan pesan jika gagal membuat data
            }

            return Ok((RoleDto)result); // Mengembalikan data Role yang baru saja dibuat
        }

        // HTTP PUT untuk memperbarui data Role berdasarkan GUID
        [HttpPut("{guid}")]
        public IActionResult Update(RoleDto roleDto)
        {
            var entity = _roleRepository.GetByGuid(roleDto.Guid);
            if (entity is null)
            {
                return NotFound("Id Not Found");
            }

            Role toUpdate = roleDto;
            toUpdate.CreatedDate = entity.CreatedDate;

            var result = _roleRepository.Update(toUpdate);
            if (!result)
            {
                return BadRequest("Failed to update data");  // Mengembalikan pesan jika gagal memperbarui data
            }

            return Ok("Data updated successfully"); // Mengembalikan pesan sukses jika pembaruan berhasil
        }

        // HTTP DELETE untuk menghapus data Role berdasarkan GUID
        [HttpDelete("{guid}")]
        public IActionResult Delete(Guid guid)
        {
            var existingRole = _roleRepository.GetByGuid(guid);
            if (existingRole is null)
            {
                return NotFound("Id Not Found");
            }

            var result = _roleRepository.Delete(existingRole);
            if (!result)
            {
                return BadRequest("Failed to delete data");  // Mengembalikan pesan jika gagal menghapus data
            }

            return Ok("Data deleted successfully");  // Mengembalikan pesan sukses jika penghapusan berhasil
        }

    }
}
