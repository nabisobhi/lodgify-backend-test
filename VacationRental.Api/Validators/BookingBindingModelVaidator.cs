using FluentValidation;
using VacationRental.Api.Models;

namespace VacationRental.Api.Validators
{
    public class BookingBindingModelVaidator : AbstractValidator<BookingBindingModel>
    {
        public BookingBindingModelVaidator()
        {
            RuleFor(model => model.RentalId)
                .GreaterThan(0)
                .WithMessage($"{nameof(BookingBindingModel.RentalId)} must be positive");
            RuleFor(model => model.Start)
                .NotEmpty()
                .WithMessage($"{nameof(BookingBindingModel.Start)} must be set");
            RuleFor(model => model.Nights)
                .GreaterThan(0)
                .WithMessage($"{nameof(BookingBindingModel.Nights)} must be positive");
        }
    }
}
