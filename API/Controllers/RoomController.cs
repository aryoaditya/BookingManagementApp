using API.Contracts;
using API.DTOs.Rooms;
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
    public class RoomController : ControllerBase
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public RoomController(IRoomRepository roomRepository, IBookingRepository bookingRepository, IEmployeeRepository employeeRepository)
        {
            _roomRepository = roomRepository;
            _bookingRepository = bookingRepository;
            _employeeRepository = employeeRepository;
        }

        // Endpoint untuk mengambil daftar ruangan yang sedang digunakan pada hari ini
        [HttpGet("occupied-rooms")]
        public IActionResult GetOccupiedRooms()
        {
            try
            {
                // Mengambil daftar booking yang berlangsung hari ini
                var currentBookings = _bookingRepository.GetCurrentBookings();

                if (!currentBookings.Any())
                {
                    return Ok(new ResponseOkHandler<IEnumerable<Booking>>("No rooms are currently booked.", currentBookings)); // Jika tidak ada pemesanan saat ini
                }

                var rooms = _roomRepository.GetAll();
                var employees = _employeeRepository.GetAll();

                // Mengambil data dari ruangan yang sedang digunakan
                var occupiedRooms = from r in rooms
                                    join b in currentBookings on r.Guid equals b.RoomGuid
                                    join e in employees on b.EmployeeGuid equals e.Guid
                                    select new OccupiedRoomsDto
                                    {
                                        BookingGuid = b.Guid,
                                        RoomName = r.Name,
                                        Status = b.Status.ToString(),
                                        Floor = r.Floor,
                                        BookedBy = string.Concat(e.FirstName, " ", e.LastName)
                                    };

                return Ok(new ResponseOkHandler<IEnumerable<OccupiedRoomsDto>>(occupiedRooms));
            }
            catch (ExceptionHandler ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Failed to retrieve current bookings",
                    Error = ex.Message
                });
            }
        }

        // Endpoint untuk mengambil daftar ruangan yang tidak digunakan pada hari ini
        [HttpGet("vacant-rooms")]
        public IActionResult GetVacantRooms()
        {
            try
            {
                // Mengambil daftar semua ruangan
                var allRooms = _roomRepository.GetAll();

                // Mengambil daftar booking yang sedang digunakan hari ini
                var currentBookings = _bookingRepository.GetCurrentBookings();

                // Menentukan GUID ruangan yang sedang digunakan hari ini
                var occupiedRoomGuids = currentBookings.Select(b => b.RoomGuid);

                // Mengambil informasi ruangan yang tidak digunakan hari ini
                var vacantRooms = from r in allRooms
                                  where !occupiedRoomGuids.Contains(r.Guid)
                                  select new RoomDto
                                  {
                                      Guid = r.Guid,
                                      RoomName = r.Name,
                                      Floor = r.Floor,
                                      Capacity = r.Capacity
                                  };

                return Ok(new ResponseOkHandler<IEnumerable<RoomDto>>(vacantRooms));
            }
            catch (ExceptionHandler ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Failed to retrieve vacant rooms",
                    Error = ex.Message
                });
            }
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
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
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