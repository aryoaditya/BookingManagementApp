using API.Contracts;
using API.DTOs.Bookings;
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
    public class BookingController : ControllerBase
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IRoomRepository _roomRepository;

        public BookingController(IBookingRepository bookingRepository, IEmployeeRepository employeeRepository, IRoomRepository roomRepository)
        {
            _bookingRepository = bookingRepository;
            _employeeRepository = employeeRepository;
            _roomRepository = roomRepository;
        }

        [HttpGet("booking-duration")]
        public IActionResult GetBookingDuration()
        {
            try
            {
                // Mengambil daftar booking
                var bookings = _bookingRepository.GetAll();

                if (!bookings.Any())
                {
                    return NotFound(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "No booking data found"
                    });
                }

                var rooms = _roomRepository.GetAll();
                var bookingDurations = new List<BookingDurationDto>();

                foreach (var booking in bookings)
                {
                    // Menghitung durasi booking dalam jam
                    var totalHours = (int)(booking.EndDate - booking.StartDate).TotalHours;

                    // Menghitung jumlah jam Sabtu dan Minggu dalam rentang tanggal
                    var weekendHours = Enumerable.Range(0, totalHours + 1)
                        .Select(hour => booking.StartDate.AddHours(hour))
                        .Count(date => date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday);

                    // Mengurangkan jumlah jam Sabtu dan Minggu dari total durasi
                    var bookingLengthInHours = $"{totalHours - weekendHours} jam";

                    // Menambahkan data durasi booking dalam jam ke daftar
                    bookingDurations.Add(new BookingDurationDto
                    {
                        RoomGuid = booking.RoomGuid,
                        RoomName = rooms.FirstOrDefault(r => r.Guid == booking.RoomGuid)?.Name,
                        BookingLength = bookingLengthInHours
                    });
                }

                return Ok(new ResponseOkHandler<IEnumerable<BookingDurationDto>>(bookingDurations));
            }
            catch (ExceptionHandler ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Failed to retrieve booking duration data",
                    Error = ex.Message
                });
            }
        }

        // Endpoint all booking details
        [HttpGet("details")]
        public IActionResult GetDetails()
        {
            var employees = _employeeRepository.GetAll();
            var bookings = _bookingRepository.GetAll();
            var rooms = _roomRepository.GetAll();

            if (!(employees.Any() && rooms.Any() && bookings.Any()))
            {
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
            }

            // Mengambil detail koleksi data booking
            var bookingDetails = from e in employees
                                 join b in bookings on e.Guid equals b.EmployeeGuid
                                 join r in rooms on b.RoomGuid equals r.Guid
                                 select new BookingDetailsDto
                                 {
                                     Guid = b.Guid,
                                     BookedNik = e.Nik,
                                     BookedBy = string.Concat(e.FirstName, " ", e.LastName),
                                     RoomName = r.Name,
                                     StartDate = b.StartDate,
                                     EndDate = b.EndDate,
                                     Status = b.Status.ToString(),
                                     Remarks = b.Remarks
                                 };

            return Ok(new ResponseOkHandler<IEnumerable<BookingDetailsDto>>(bookingDetails));
        }

        // Endpoint booking detail
        [HttpGet("details/{guid}")]
        public IActionResult GetBookingByGuid(Guid guid)
        {
            try
            {
                var booking = _bookingRepository.GetByGuid(guid);

                if (booking is null)
                {
                    // Mengembalikan pesan jika ID tidak ditemukan
                    return NotFound(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "ID Not Found"
                    });
                }

                var employee = _employeeRepository.GetByGuid(booking.EmployeeGuid);
                var room = _roomRepository.GetByGuid(booking.RoomGuid);

                var bookingDetail = new BookingDetailsDto
                {
                    Guid = booking.Guid,
                    BookedNik = employee.Nik,
                    BookedBy = string.Concat(employee.FirstName, " ", employee.LastName),
                    RoomName = room.Name,
                    StartDate = booking.StartDate,
                    EndDate = booking.EndDate,
                    Status = booking.Status.ToString(),
                    Remarks = booking.Remarks
                };

                // Mengembalikan data Employee jika ditemukan
                return Ok(new ResponseOkHandler<BookingDetailsDto>(bookingDetail));
            }
            catch (ExceptionHandler ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Failed to retrieve data",
                    Error = ex.Message
                });
            }
        }

        // HTTP GET untuk mengambil semua data Booking
        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _bookingRepository.GetAll();
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

            var data = result.Select(x => (BookingDto)x);

            return Ok(new ResponseOkHandler<IEnumerable<BookingDto>>(data));
        }

        // HTTP GET untuk mengambil data Booking berdasarkan GUID
        [HttpGet("{guid}")]
        public IActionResult GetByGuid(Guid guid)
        {
            var result = _bookingRepository.GetByGuid(guid);
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
            // Mengembalikan data Employee jika ditemukan
            return Ok(new ResponseOkHandler<BookingDto>((BookingDto)result));
        }

        // HTTP POST untuk membuat data Booking baru
        [HttpPost]
        public IActionResult Create(CreateBookingDto bookingDto)
        {
            try
            {
                var result = _bookingRepository.Create(bookingDto);

                // Mengembalikan data Employee yang baru saja dibuat
                return Ok(new ResponseOkHandler<BookingDto>((BookingDto)result));
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

        // HTTP PUT untuk memperbarui data Booking berdasarkan GUID
        [HttpPut]
        public IActionResult Update(BookingDto bookingDto)
        {
            try
            {
                var entity = _bookingRepository.GetByGuid(bookingDto.Guid);
                if (entity is null)
                {
                    return NotFound(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Id Not Found"
                    });
                }

                Booking toUpdate = bookingDto;
                toUpdate.CreatedDate = entity.CreatedDate;

                _bookingRepository.Update(toUpdate);

                return Ok(new ResponseOkHandler<BookingDto>("Data updated successfully")); // Mengembalikan pesan sukses jika pembaruan berhasil
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

        // HTTP DELETE untuk menghapus data Booking berdasarkan GUID
        [HttpDelete("{guid}")]
        [Authorize(Roles = "admin")]
        public IActionResult Delete(Guid guid)
        {
            try
            {
                var existingBooking = _bookingRepository.GetByGuid(guid);
                if (existingBooking is null)
                {
                    return NotFound(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Id Not Found"
                    });
                }

                _bookingRepository.Delete(existingBooking);

                return Ok(new ResponseOkHandler<BookingDto>("Data deleted successfully"));  // Mengembalikan pesan sukses jika penghapusan berhasil
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