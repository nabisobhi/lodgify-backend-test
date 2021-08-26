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
        private readonly IMapper _mapper;

        public RentalsController(IRentalsService rentalsService,
            IMapper mapper)
        {
            _rentalsService = rentalsService;
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
    }
}
