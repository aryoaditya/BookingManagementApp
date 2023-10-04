using API.Models;

namespace API.DTOs.Bookings
{
    public class BookingDto : GeneralBookingDto
    {
        public Guid Guid { get; set; }

        // Operator konversi eksplisit dari Booking ke BookingDto
        public static explicit operator BookingDto(Booking booking)
        {
            return new BookingDto
            {
                Guid = booking.Guid,
                StartDate = booking.StartDate,
                EndDate = booking.EndDate,
                Status = booking.Status,
                Remarks = booking.Remarks,
                RoomGuid = booking.RoomGuid,
                EmployeeGuid = booking.EmployeeGuid
            };
        }

        // Operator konversi implisit dari BookingDto ke Booking
        public static implicit operator Booking(BookingDto bookingDto)
        {
            return new Booking
            {   
                Guid = bookingDto.Guid,
                StartDate = bookingDto.StartDate,
                EndDate = bookingDto.EndDate,
                Status = bookingDto.Status,
                Remarks = bookingDto.Remarks,
                RoomGuid = bookingDto.RoomGuid,
                EmployeeGuid = bookingDto.EmployeeGuid,
                ModifiedDate = DateTime.Now
            };
        }
    }
}
