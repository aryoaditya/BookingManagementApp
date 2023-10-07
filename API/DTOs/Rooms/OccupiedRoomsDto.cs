namespace API.DTOs.Rooms
{
    public class OccupiedRoomsDto
    {
        public Guid BookingGuid { get; set; }
        public string RoomName { get; set; }
        public string Status { get; set; }
        public int Floor { get; set; }
        public string BookedBy { get; set; }
    }
}