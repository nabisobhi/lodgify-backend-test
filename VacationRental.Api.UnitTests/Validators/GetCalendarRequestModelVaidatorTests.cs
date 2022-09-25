using System;
using System.Collections.Generic;
using VacationRental.Api.Validators;
using Xunit;

namespace VacationRental.Api.UnitTests.Validators
{
    [Collection("Validators")]
    public class GetCalendarRequestModelVaidatorTests
    {
        private readonly GetCalendarRequestModelVaidator _validator;

        public GetCalendarRequestModelVaidatorTests()
        {
            _validator = new GetCalendarRequestModelVaidator();
        }

        public static IEnumerable<object[]> Data => new[]
            {
                new object[] { 1, new DateTime(2000, 1, 1), 0, false },
                new object[] { 0, new DateTime(2000, 1, 1), 1, false },
                new object[] { -1, new DateTime(2000, 1, 1), 1, false },
                new object[] { 1, new DateTime(2000, 1, 1), -1, false },
                new object[] { 1, null, 1, false },
                new object[] { 1, new DateTime(2000, 1, 1), 1, true },
            };

        [Theory, MemberData(nameof(Data))]
        public void GivenData_WhenCallValidator_ThenCheckResult(int rentalId, DateTime start, int nights, bool expectedResult)
        {
            Assert.Equal(expectedResult, _validator.Validate(new Models.GetCalendarRequestModel
            {
                RentalId = rentalId,
                Start = start,
                Nights = nights,
            }).IsValid);
        }
    }
}
