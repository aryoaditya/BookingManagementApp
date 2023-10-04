using API.DTOs.Rooms;
using FluentValidation;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Utilities.Validators.Rooms
{
    public class GeneralRoomValidator<T> : AbstractValidator<T> where T : GeneralRoomDto
    {
        public GeneralRoomValidator()
        {
            // Name validation
            RuleFor(r => r.Name)
                .NotEmpty()
                .MaximumLength(100);

            // Floor validation
            RuleFor(r => r.Floor)
                .NotNull();

            // Capacity validation
            RuleFor(r => r.Capacity)
                .NotNull();
        }
    }
}
