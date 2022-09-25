using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
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

        /// <summary>
        /// Get a calendar.
        /// </summary>
        /// <response code="200">Returns a calendar</response>
        /// <response code="400">If nights value is not positive</response>
        /// <response code="404">If the rental is not found</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CalendarViewModel> Get([FromQuery]GetCalendarRequestModel requestModel)
        {
            var rental = _rentalsService.GetById(requestModel.RentalId);
            if (rental is null)
                return BadRequest("Rental not found");

            var calendarBookingList = _calendarService.GetCalendar(rental, requestModel.Start, requestModel.Nights);

            var result = new CalendarViewModel
            {
                RentalId = requestModel.RentalId,
                Dates = new List<CalendarDateViewModel>(),
            };

            for (var i = 0; i < requestModel.Nights; i++)
            {
                var dateTime = requestModel.Start.Date.AddDays(i);
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

            return Ok(result);
        }
    }
}
