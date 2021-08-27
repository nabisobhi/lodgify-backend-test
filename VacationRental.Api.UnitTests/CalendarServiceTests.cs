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
    public class CalendarServiceTests
    {
        private readonly Rental _fakeRental;
        private readonly DateTime _startDate;
        private readonly int _nights;
        private readonly Booking _booking1;
        private readonly Booking _booking2;
        private readonly Booking _booking3;
        private readonly Mock<IBookingsService> _mockBookingsService;
        private readonly CalendarService _calendarService;

        public CalendarServiceTests()
        {
            _fakeRental = new Rental
            {
                Id = 1,
                Units = 2,
                PreparationTimeInDays = 2,
            };
            _startDate = new DateTime(2000, 1, 1);
            _nights = 10;
            _booking1 = new Booking
            {
                Id = 1,
                RentalId = _fakeRental.Id,
                Start = new DateTime(2000, 1, 4),
                Nights = 1,
            };
            _booking2 = new Booking
            {
                Id = 2,
                RentalId = _fakeRental.Id,
                Start = new DateTime(2000, 1, 2),
                Nights = 1,
            };
            _booking3 = new Booking
            {
                Id = 3,
                RentalId = _fakeRental.Id,
                Start = new DateTime(2000, 1, 6),
                Nights = 2,
            };
            _mockBookingsService = new Mock<IBookingsService>();
            _mockBookingsService.Setup(r => r.GetAllRentalBookings(_fakeRental, _startDate, _nights, true)).Returns(CreateMockBookingData());
            _calendarService = new CalendarService(_mockBookingsService.Object);
        }

        private List<Booking> CreateMockBookingData()
        {
            return new[] {
                _booking1,
                _booking2,
                _booking3,
            }.ToList();
        }

        [Fact]
        public void GivenDateRange_WhenGettingCalendar_ThenCheckResult()
        {
            var calendarDates = _calendarService.GetCalendar(_fakeRental, _startDate, _nights);

            Assert.Equal(10, calendarDates.Count);
            Assert.Equal(4, calendarDates.Count(cd => cd.Type == CalendarDateType.Booking));
            Assert.Equal(6, calendarDates.Count(cd => cd.Type == CalendarDateType.PreparationTime));
            
            Assert.Contains(calendarDates, cd => cd.Type == CalendarDateType.Booking && cd.BookingId == _booking1.Id && cd.Date == _booking1.Start && cd.Unit == 2);
            Assert.Contains(calendarDates, cd => cd.Type == CalendarDateType.Booking && cd.BookingId == _booking2.Id && cd.Date == _booking2.Start && cd.Unit == 1);
            Assert.Contains(calendarDates, cd => cd.Type == CalendarDateType.Booking && cd.BookingId == _booking3.Id && cd.Date == _booking3.Start && cd.Unit == 1);
            Assert.Contains(calendarDates, cd => cd.Type == CalendarDateType.Booking && cd.BookingId == _booking3.Id && cd.Date == _booking3.Start.AddDays(1) && cd.Unit == 1);
            
            Assert.Contains(calendarDates, cd => cd.Type == CalendarDateType.PreparationTime && cd.Date == _booking1.Start.AddDays(1) && cd.Unit == 2);
            Assert.Contains(calendarDates, cd => cd.Type == CalendarDateType.PreparationTime && cd.Date == _booking1.Start.AddDays(2) && cd.Unit == 2);
            Assert.Contains(calendarDates, cd => cd.Type == CalendarDateType.PreparationTime && cd.Date == _booking2.Start.AddDays(1) && cd.Unit == 1);
            Assert.Contains(calendarDates, cd => cd.Type == CalendarDateType.PreparationTime && cd.Date == _booking2.Start.AddDays(2) && cd.Unit == 1);
            Assert.Contains(calendarDates, cd => cd.Type == CalendarDateType.PreparationTime && cd.Date == _booking3.Start.AddDays(2) && cd.Unit == 1);
            Assert.Contains(calendarDates, cd => cd.Type == CalendarDateType.PreparationTime && cd.Date == _booking3.Start.AddDays(3) && cd.Unit == 1);
        }
    }
}
