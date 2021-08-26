using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Api.Services;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly IBookingsService _bookingsService;
        private readonly IRentalsService _rentalsService;

        public CalendarController(IBookingsService bookingsService,
            IRentalsService rentalsService)
        {
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

            var result = new CalendarViewModel 
            {
                RentalId = rentalId,
                Dates = new List<CalendarDateViewModel>() 
            };

            for (var i = 0; i < nights; i++)
            {
                var dateTime = start.Date.AddDays(i);
                result.Dates.Add(new CalendarDateViewModel
                {
                    Date = dateTime,
                    Bookings = _bookingsService.GetAllRentalBookings(rentalId, dateTime)
                        .Select(booking => new CalendarBookingViewModel { Id = booking.Id })
                        .ToList(),
                });
            }

            return result;
        }
    }
}
