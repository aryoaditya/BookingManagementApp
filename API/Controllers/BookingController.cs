using API.Contracts;
using API.DTOs.Bookings;
using API.DTOs.Roles;
using API.DTOs.Rooms;
using API.Models;
using API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingController(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        // HTTP GET untuk mengambil semua data Booking
        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _bookingRepository.GetAll();
            if (!result.Any())
            {
                return NotFound("Data Not Found"); // Mengembalikan pesan jika tidak ada data yang ditemukan
            }

            var data = result.Select(x => (BookingDto)x);

            return Ok(data);  // Mengembalikan data Booking jika ada
        }

        // HTTP GET untuk mengambil data Booking berdasarkan GUID
        [HttpGet("{guid}")]
        public IActionResult GetByGuid(Guid guid)
        {
            var result = _bookingRepository.GetByGuid(guid);
            if (result is null)
            {
                return NotFound("Id Not Found"); // Mengembalikan pesan jika ID tidak ditemukan
            }
            return Ok((BookingDto)result);  // Mengembalikan data Booking jika ditemukan
        }

        // HTTP POST untuk membuat data Booking baru
        [HttpPost]
        public IActionResult Create(CreateBookingDto bookingDto)
        {
            var result = _bookingRepository.Create(bookingDto);
            if (result is null)
            {
                return BadRequest("Failed to create data"); // Mengembalikan pesan jika gagal membuat data
            }

            return Ok((BookingDto)result); // Mengembalikan data Booking yang baru saja dibuat
        }

        // HTTP PUT untuk memperbarui data Booking berdasarkan GUID
        [HttpPut("{guid}")]
        public IActionResult Update(BookingDto bookingDto)
        {
            var entity = _bookingRepository.GetByGuid(bookingDto.Guid);
            if (entity is null)
            {
                return NotFound("Id Not Found");
            }

            Booking toUpdate = bookingDto;
            toUpdate.CreatedDate = entity.CreatedDate;

            var result = _bookingRepository.Update(toUpdate);
            if (!result)
            {
                return BadRequest("Failed to update data");  // Mengembalikan pesan jika gagal memperbarui data
            }

            return Ok("Data updated successfully"); // Mengembalikan pesan sukses jika pembaruan berhasil
        }

        // HTTP DELETE untuk menghapus data Booking berdasarkan GUID
        [HttpDelete("{guid}")]
        public IActionResult Delete(Guid guid)
        {
            var existingBooking = _bookingRepository.GetByGuid(guid);
            if (existingBooking is null)
            {
                return NotFound("Id Not Found");
            }

            var result = _bookingRepository.Delete(existingBooking);
            if (!result)
            {
                return BadRequest("Failed to delete data");  // Mengembalikan pesan jika gagal menghapus data
            }

            return Ok("Data deleted successfully");  // Mengembalikan pesan sukses jika penghapusan berhasil
        }

    }
}
