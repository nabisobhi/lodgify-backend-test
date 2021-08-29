using System.Collections.Generic;
using VacationRental.Api.Validators;
using Xunit;

namespace VacationRental.Api.UnitTests.Validators
{
    [Collection("Validators")]
    public class RentalBindingModelVaidatorTests
    {
        private readonly RentalBindingModelVaidator _validator;

        public RentalBindingModelVaidatorTests()
        {
            _validator = new RentalBindingModelVaidator();
        }

        public static IEnumerable<object[]> Data => new[]
            {
                new object[] { 1, 0, true },
                new object[] { 0, 1, false },
                new object[] { -1, 1, false },
                new object[] { 1, -1, false },
                new object[] { 1, 1, true },
            };

        [Theory, MemberData(nameof(Data))]
        public void GivenData_WhenCallValidator_ThenCheckResult(int units, int preparationTimeInDays, bool expectedResult)
        {
            Assert.Equal(expectedResult, _validator.Validate(new Models.RentalBindingModel
            {
                Units = units,
                PreparationTimeInDays = preparationTimeInDays,
            }).IsValid);
        }
    }
}
