using API.Models;

namespace API.DTOs.Rooms
{
    public class CreateRoomDto : GeneralRoomDto
    {
        // Operator konversi implisit dari CreateRoomDto ke Room
        public static implicit operator Room(CreateRoomDto createRoomDto)
        {
            return new Room
            {
                Name = createRoomDto.Name,
                Floor = createRoomDto.Floor,
                Capacity = createRoomDto.Capacity,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
        }
    }
}
