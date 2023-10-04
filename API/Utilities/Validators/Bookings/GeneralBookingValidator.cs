using API.DTOs.Bookings;
using API.Utilities.Enums;
using FluentValidation;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Utilities.Validators.Bookings
{
    public class GeneralBookingValidator<T> : AbstractValidator<T> where T : GeneralBookingDto
    {
        public GeneralBookingValidator()
        {
            // StartDate validation
            RuleFor(b => b.StartDate)
                .NotEmpty();

            // EndDate validation
            RuleFor(b => b.EndDate)
                .NotEmpty();

            // Status validation
            RuleFor(b => b.Status)
                .NotNull()
                .IsInEnum();

            // Remarks validation
            RuleFor(b => b.Remarks)
                .NotEmpty();

            // RoomGuid validation
            RuleFor(b => b.RoomGuid)
                .NotEmpty();

            // EmployeeGuid validation
            RuleFor(b => b.EmployeeGuid)
                .NotEmpty();
        }
    }
}
