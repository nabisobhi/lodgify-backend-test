using System;
using System.Collections.Generic;
using VacationRental.Api.Validators;
using Xunit;

namespace VacationRental.Api.UnitTests.Validators
{
    [Collection("Validators")]
    public class BookingBindingModelVaidatorTests
    {
        private readonly BookingBindingModelVaidator _validator;

        public BookingBindingModelVaidatorTests()
        {
            _validator = new BookingBindingModelVaidator();
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
            Assert.Equal(expectedResult, _validator.Validate(new Models.BookingBindingModel
            {
                RentalId = rentalId,
                Start = start,
                Nights = nights,
            }).IsValid);
        }
    }
}
