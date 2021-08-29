using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Domain;
using VacationRental.Api.Models;
using VacationRental.Api.Services;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingsService _bookingsService;
        private readonly IRentalsService _rentalsService;
        private readonly IMapper _mapper;

        public BookingsController(IBookingsService bookingsService,
            IRentalsService rentalsService,
            IMapper mapper)
        {
            _bookingsService = bookingsService;
            _rentalsService = rentalsService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get a booking by Id.
        /// </summary>
        /// <response code="200">Returns the booking</response>
        /// <response code="404">If the booking is not found</response>
        [HttpGet]
        [Route("{bookingId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<BookingViewModel> Get(int bookingId)
        {
            var booking = _bookingsService.GetById(bookingId);

            if (booking is null)
                return NotFound(nameof(Booking));

            return Ok(_mapper.Map<BookingViewModel>(booking));
        }

        /// <summary>
        /// Creates a booking.
        /// </summary>
        /// <response code="201">Returns the newly created bookingId</response>
        /// <response code="400">If nights value is not positive, or the booking is not available</response>
        /// <response code="404">If the rental is not found</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ResourceIdViewModel> Post(BookingBindingModel model)
        {
            var rental = _rentalsService.GetById(model.RentalId);
            if (rental is null)
                return NotFound(nameof(Rental));

            var newBooking = _mapper.Map<Booking>(model);

            if (!_bookingsService.IsBookingAvailable(newBooking, rental))
                return BadRequest("Booking is not available.");

            var newBookingId = _bookingsService.Insert(newBooking);

            return CreatedAtAction(nameof(Get), new { bookingId = newBookingId }, new ResourceIdViewModel { Id = newBookingId });
        }
    }
}
