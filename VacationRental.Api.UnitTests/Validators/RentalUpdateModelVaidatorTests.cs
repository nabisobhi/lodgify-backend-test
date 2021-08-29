using System.Collections.Generic;
using VacationRental.Api.Validators;
using Xunit;

namespace VacationRental.Api.UnitTests.Validators
{
    [Collection("Validators")]
    public class RentalUpdateModelVaidatorTests
    {
        private readonly RentalUpdateModelVaidator _validator;

        public RentalUpdateModelVaidatorTests()
        {
            _validator = new RentalUpdateModelVaidator();
        }

        public static IEnumerable<object[]> Data => new[]
            {
                new object[] { 0, 1, 0, false },
                new object[] { 1, 1, 0, true },
                new object[] { 1, 0, 1, false },
                new object[] { 1, -1, 1, false },
                new object[] { 1, 1, -1, false },
                new object[] { 1, 1, 1, true },
            };

        [Theory, MemberData(nameof(Data))]
        public void GivenData_WhenCallValidator_ThenCheckResult(int id, int units, int preparationTimeInDays, bool expectedResult)
        {
            Assert.Equal(expectedResult, _validator.Validate(new Models.RentalUpdateModel
            {
                Id = id,
                Units = units,
                PreparationTimeInDays = preparationTimeInDays,
            }).IsValid);
        }
    }
}
