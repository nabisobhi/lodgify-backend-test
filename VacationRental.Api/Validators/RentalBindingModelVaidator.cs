using FluentValidation;
using VacationRental.Api.Models;

namespace VacationRental.Api.Validators
{
    public class RentalBindingModelVaidator : AbstractValidator<RentalBindingModel>
    {
        public RentalBindingModelVaidator()
        {
            RuleFor(model => model.Units)
                .GreaterThan(0)
                .WithMessage($"{nameof(RentalBindingModel.Units)} must be positive");
            RuleFor(model => model.PreparationTimeInDays)
                .GreaterThanOrEqualTo(0)
                .WithMessage($"{nameof(RentalBindingModel.PreparationTimeInDays)} must not be negative");
        }
    }
}
