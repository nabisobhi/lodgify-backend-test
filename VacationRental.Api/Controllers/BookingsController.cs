using System;
using System.Collections.Generic;
using AutoMapper;
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

        [HttpGet]
        [Route("{bookingId:int}")]
        public BookingViewModel Get(int bookingId)
        {
            var booking = _bookingsService.GetById(bookingId);

            if (booking is null)
                throw new ApplicationException("Booking not found");

            return _mapper.Map<BookingViewModel>(booking);
        }

        [HttpPost]
        public ResourceIdViewModel Post(BookingBindingModel model)
        {
            if (model.Nights <= 0)
                throw new ApplicationException("Nights must be positive");

            var rental = _rentalsService.GetById(model.RentalId);
            if (rental is null)
                throw new ApplicationException("Rental not found");

            var newBooking = _mapper.Map<Booking>(model);

            if (!_bookingsService.IsBookingAvailable(newBooking, rental))
                throw new ApplicationException("Not available");

            var newBookingId = _bookingsService.Insert(newBooking);

            return new ResourceIdViewModel { Id = newBookingId };
        }
    }
}
