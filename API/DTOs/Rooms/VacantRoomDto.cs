namespace API.DTOs.Rooms
{
    public class VacantRoomDto
    {
        //RoomGuid, RoomName, Floor, Capacity
        public Guid Guid { get; set; }

        public string RoomName { get; set; }
        public int Floor { get; set; }
        public int Capacity { get; set; }
    }
}