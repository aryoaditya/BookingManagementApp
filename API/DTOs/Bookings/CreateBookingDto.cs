using API.DTOs.Educations;
using API.Models;
using API.Utilities.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.DTOs.Bookings
{
    public class CreateBookingDto : GeneralBookingDto
    {
        // Operator konversi implisit dari CreateBookingDto ke Booking
        public static implicit operator Booking(CreateBookingDto createBookingDto)
        {
            return new Booking
            {
                StartDate = createBookingDto.StartDate,
                EndDate = createBookingDto.EndDate,
                Status = createBookingDto.Status,
                Remarks = createBookingDto.Remarks,
                RoomGuid = createBookingDto.RoomGuid,
                EmployeeGuid = createBookingDto.EmployeeGuid,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
        }
    }
}
