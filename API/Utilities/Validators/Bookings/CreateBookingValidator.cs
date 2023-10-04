using API.DTOs.Bookings;
using API.Utilities.Enums;
using FluentValidation;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Utilities.Validators.Bookings
{
    public class CreateBookingValidator : GeneralBookingValidator<CreateBookingDto>
    {
    }
}
