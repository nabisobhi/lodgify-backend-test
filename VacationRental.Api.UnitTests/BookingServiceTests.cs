using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationRental.Api.Data;
using VacationRental.Api.Domain;
using VacationRental.Api.Services;
using Xunit;
using Xunit.Extensions;

namespace VacationRental.Api.UnitTests
{
    [Collection("Unit")]
    public class BookingServiceTests
    {
        private readonly Rental _fakeRental;
        private readonly Mock<IRepository<Booking>> _mockBookingRepository;
        private readonly BookingsService _bookingsService;

        public BookingServiceTests()
        {
            _fakeRental = new Rental
            {
                Id = 1,
                Units = 2,
                PreparationTimeInDays = 2,
            };
            _mockBookingRepository = new Mock<IRepository<Booking>>();
            _mockBookingRepository.Setup(r => r.Table).Returns(CreateMockBookingData());
            _bookingsService = new BookingsService(_mockBookingRepository.Object);
        }

        private IQueryable<Booking> CreateMockBookingData()
        {
            return new[] {
                new Booking
                {
                    Id = 1,
                    RentalId = _fakeRental.Id,
                    Start = new DateTime(2000, 1, 4),
                    Nights = 1,
                },
                new Booking
                {
                    Id = 2,
                    RentalId = _fakeRental.Id,
                    Start = new DateTime(2000, 1, 2),
                    Nights = 1,
                },
                new Booking
                {
                    Id = 3,
                    RentalId = _fakeRental.Id,
                    Start = new DateTime(2000, 1, 6),
                    Nights = 2,
                },
            }.AsQueryable();
        }

        public static IEnumerable<object[]> NewBookings => new[]
            {
                new object[] { new DateTime(2000, 1, 1), 1, true},
                new object[] { new DateTime(2000, 1, 7), 1, true},
                new object[] { new DateTime(2000, 1, 7), 10, true},
                new object[] { new DateTime(2000, 1, 10), 1, true},
                new object[] { new DateTime(2000, 1, 1), 2, false },
                new object[] { new DateTime(2000, 1, 2), 1, false },
                new object[] { new DateTime(2000, 1, 2), 2, false },
                new object[] { new DateTime(2000, 1, 3), 1, false },
                new object[] { new DateTime(2000, 1, 4), 1, false },
                new object[] { new DateTime(2000, 1, 4), 2, false },
                new object[] { new DateTime(2000, 1, 5), 1, false },
                new object[] { new DateTime(2000, 1, 5), 2, false },
                new object[] { new DateTime(2000, 1, 6), 1, false },
                new object[] { new DateTime(2000, 1, 6), 10, false },
            };

        [Theory, MemberData(nameof(NewBookings))]
        public void GivenNewBookings_WhenCheckIsBookingAvailablity_ThenCheckResult(DateTime start, int nights, bool expectedResult)
        {
            var bookingToTest = new Booking
            {
                RentalId = _fakeRental.Id,
                Start = start,
                Nights = nights,
            };
            Assert.Equal(expectedResult, _bookingsService.IsBookingAvailable(bookingToTest, _fakeRental));
        }

        public static IEnumerable<object[]> TimePeriods => new[]
            {
                new object[] { new DateTime(2000, 1, 1), 1, true, 0 },
                new object[] { new DateTime(2000, 1, 1), 2, true, 1 },
                new object[] { new DateTime(2000, 1, 2), 1, true, 1 },
                new object[] { new DateTime(2000, 1, 2), 2, true, 1 },
                new object[] { new DateTime(2000, 1, 3), 1, true, 1 },
                new object[] { new DateTime(2000, 1, 3), 1, false, 0 },
                new object[] { new DateTime(2000, 1, 3), 3, true, 2 },
                new object[] { new DateTime(2000, 1, 3), 4, true, 3 },
                new object[] { new DateTime(2000, 1, 3), 4, false, 2 },
                new object[] { new DateTime(2000, 1, 8), 10, true, 1 },
                new object[] { new DateTime(2000, 1, 8), 10, false, 0 },
                new object[] { new DateTime(2000, 1, 10), 10, true, 0 },
                new object[] { new DateTime(2000, 1, 1), 10, true, 3 },
            };

        [Theory, MemberData(nameof(TimePeriods))]
        public void GivenTimePeriod_WhenGetAllRentalBookings_ThenCheckResult(DateTime start, int nights, bool considerPreparationTime, int expctedResultLength)
        {
            Assert.Equal(expctedResultLength, _bookingsService.GetAllRentalBookings(_fakeRental, start, nights, considerPreparationTime).Count);
        }


        public static IEnumerable<object[]> NewParameters => new[]
            {
                new object[] { 2, 2, true },
                new object[] { 2, 3, true },
                new object[] { 2, 4, false },
                new object[] { 2, 5, false },
                new object[] { 2, 0, true },
                new object[] { 2, 10, false },
                new object[] { 3, 2, true },
                new object[] { 3, 1, true },
                new object[] { 3, 0, true },
                new object[] { 3, 10, true },
                new object[] { 1, 0, true },
                new object[] { 1, 1, true },
                new object[] { 1, 2, false },
                new object[] { 1, 10, false },
                new object[] { 0, 0, false },
                new object[] { 0, 1, false },
            };

        [Theory, MemberData(nameof(NewParameters))]
        public void GivenNewParameters_WhenValidateBookingsWithNewParameters_ThenCheckResult(int units, int preparationTimeInDays, bool expctedResult)
        {
            Assert.Equal(expctedResult, _bookingsService.ValidateBookingsWithNewParameters(_fakeRental, units, preparationTimeInDays));
        }
    }
}
