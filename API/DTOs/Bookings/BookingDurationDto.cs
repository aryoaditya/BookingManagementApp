namespace API.DTOs.Bookings
{
    public class BookingDurationDto
    {
        public Guid RoomGuid { get; set; }
        public string RoomName { get; set; }
        public string BookingLength { get; set; }
    }
}