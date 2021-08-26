using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Domain;
using VacationRental.Api.Models;
using VacationRental.Api.Services;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly ICalendarService _calendarService;
        private readonly IBookingsService _bookingsService;
        private readonly IRentalsService _rentalsService;

        public CalendarController(ICalendarService calendarService,
            IBookingsService bookingsService,
            IRentalsService rentalsService)
        {
            _calendarService = calendarService;
            _bookingsService = bookingsService;
            _rentalsService = rentalsService;
        }

        [HttpGet]
        public CalendarViewModel Get(int rentalId, DateTime start, int nights)
        {
            if (nights < 0)
                throw new ApplicationException("Nights must be positive");

            var rental = _rentalsService.GetById(rentalId);
            if (rental is null)
                throw new ApplicationException("Rental not found");

            var calendarBookingList = _calendarService.GetCalendar(rental, start, nights);

            var result = new CalendarViewModel
            {
                RentalId = rentalId,
                Dates = new List<CalendarDateViewModel>(),
            };

            for (var i = 0; i < nights; i++)
            {
                var dateTime = start.Date.AddDays(i);
                result.Dates.Add(new CalendarDateViewModel
                {
                    Date = dateTime,
                    Bookings = calendarBookingList
                    .Where(cb => cb.Date == dateTime && cb.Type == CalendarDateType.Booking)
                        .Select(cb => new CalendarBookingViewModel
                        {
                            Id = cb.BookingId,
                            Unit = cb.Unit
                        })
                        .ToList(),
                    PreparationTimes = calendarBookingList
                    .Where(cb => cb.Date == dateTime && cb.Type == CalendarDateType.PreparationTime)
                        .Select(cb => new CalendarPreparationTimeViewModel
                        {
                            Unit = cb.Unit
                        })
                        .ToList(),
                });
            }

            return result;
        }
    }
}
