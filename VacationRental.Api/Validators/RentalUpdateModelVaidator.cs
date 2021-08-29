using FluentValidation;
using VacationRental.Api.Models;

namespace VacationRental.Api.Validators
{
    public class RentalUpdateModelVaidator : AbstractValidator<RentalUpdateModel>
    {
        public RentalUpdateModelVaidator()
        {
            RuleFor(model => model.Id)
                .GreaterThan(0)
                .WithMessage($"{nameof(RentalUpdateModel.Id)} must be set");
            RuleFor(model => model.Units)
                .GreaterThan(0)
                .WithMessage($"{nameof(RentalUpdateModel.Units)} must be positive");
            RuleFor(model => model.PreparationTimeInDays)
                .GreaterThanOrEqualTo(0)
                .WithMessage($"{nameof(RentalUpdateModel.PreparationTimeInDays)} must not be negative");
        }
    }
}
