using API.DTOs.Rooms;
using FluentValidation;

namespace API.Utilities.Validators.Rooms
{
    public class UpdateRoomValidator : GeneralRoomValidator<RoomDto>
    {
        public UpdateRoomValidator() : base()
        {
            // Guid validation
            RuleFor(r => r.Guid)
                .NotEmpty();
        }
    }
}
