using API.DTOs.Bookings;
using API.Models;
using API.Utilities.Handlers;
using Client.Contracts;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Client.Repositories
{
    public class BookingRepository : GeneralRepository<BookingDetailsDto, Guid>, IBookingRepository
    {
        public BookingRepository(string request = "Booking/details") : base(request)
        {
        }

        public async Task<ResponseOkHandler<IEnumerable<BookingDetailsDto>>> GetDetail()
        {
            ResponseOkHandler<IEnumerable<BookingDetailsDto>> entityVM = null;
            using (var response = await httpClient.GetAsync(request))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseOkHandler<IEnumerable<BookingDetailsDto>>>(apiResponse);
            }
            return entityVM;
        }
    }
}