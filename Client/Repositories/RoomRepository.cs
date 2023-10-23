using API.Models;
using Client.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Client.Repositories
{
    public class RoomRepository : GeneralRepository<Room, Guid>, IRoomRepository
    {
        public RoomRepository(string request = "Room/") : base(request)
        {
        }
    }
}