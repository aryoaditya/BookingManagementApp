using API.DTOs.Universities;
using API.Models;

namespace API.DTOs.Rooms
{
    public class RoomDto : GeneralRoomDto
    {
        public Guid Guid { get; set; }

        // Operator konversi eksplisit dari Room ke RoomDto
        public static explicit operator RoomDto(Room room)
        {
            return new RoomDto
            {
                Guid = room.Guid,
                RoomName = room.Name,
                Floor = room.Floor,
                Capacity = room.Capacity
            };
        }

        // Operator konversi implisit dari RoomDto ke Room
        public static implicit operator Room(RoomDto roomDto)
        {
            return new Room
            {
                Guid = roomDto.Guid,
                Name = roomDto.RoomName,
                Floor = roomDto.Floor,
                Capacity = roomDto.Capacity,
                ModifiedDate = DateTime.Now
            };
        }
    }
}