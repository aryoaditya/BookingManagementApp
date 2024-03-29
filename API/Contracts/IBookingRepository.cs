﻿using API.Models;

namespace API.Contracts
{
    public interface IBookingRepository : IGeneralRepository<Booking>
    {
        IEnumerable<Booking> GetCurrentBookings();
    }
}