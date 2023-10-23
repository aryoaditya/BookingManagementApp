using API.DTOs.Accounts;
using API.DTOs.Bookings;
using API.Models;
using API.Utilities.Handlers;
using Client.Models;

namespace Client.Contracts
{
    public interface IBookingRepository : IGeneralRepository<BookingDetailsDto, Guid>
    {
        Task<ResponseOkHandler<IEnumerable<BookingDetailsDto>>> GetDetail();
    }
}