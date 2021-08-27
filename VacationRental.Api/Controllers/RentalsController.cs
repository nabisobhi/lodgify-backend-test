using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Domain;
using VacationRental.Api.Models;
using VacationRental.Api.Services;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IRentalsService _rentalsService;
        private readonly IBookingsService _bookingsService;
        private readonly IMapper _mapper;

        public RentalsController(IRentalsService rentalsService,
            IBookingsService bookingsService,
            IMapper mapper)
        {
            _rentalsService = rentalsService;
            _bookingsService = bookingsService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public RentalViewModel Get(int rentalId)
        {
            var rental = _rentalsService.GetById(rentalId);

            if (rental is null)
                throw new ApplicationException("Rental not found");

            return _mapper.Map<RentalViewModel>(rental);
        }

        [HttpPost]
        public ResourceIdViewModel Post(RentalBindingModel model)
        {
            var rental = _mapper.Map<Rental>(model);

            var rentalId = _rentalsService.Insert(rental);

            return new ResourceIdViewModel { Id = rentalId };
        }

        [HttpPut]
        public ResultViewModel Update(RentalUpdateModel model)
        {
            var originalRental = _rentalsService.GetById(model.Id);

            if (originalRental is null)
                throw new ApplicationException("Rental not found");

            if (!_bookingsService.ValidateBookingsWithNewParameters(originalRental, model.Units, model.PreparationTimeInDays))
                throw new ApplicationException("Cannot set new parameters");

            originalRental.Units = model.Units;
            originalRental.PreparationTimeInDays = model.PreparationTimeInDays;
            var isSuccessful = _rentalsService.Update(originalRental);

            return new ResultViewModel { IsSuccessful = isSuccessful };
        }
    }
}
