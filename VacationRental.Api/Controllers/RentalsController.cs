using System;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Domain;
using VacationRental.Api.Models;
using VacationRental.Api.Services;

namespace VacationRental.Api.Controllers
{
    [Produces("application/json")]
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

        /// <summary>
        /// Get a rental by Id.
        /// </summary>
        /// <response code="200">Returns the rental</response>
        /// <response code="404">If the rental is not found</response>
        [HttpGet]
        [Route("{rentalId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<RentalViewModel> Get(int rentalId)
        {
            var rental = _rentalsService.GetById(rentalId);

            if (rental is null)
                return NotFound(nameof(Rental));

            return Ok(_mapper.Map<RentalViewModel>(rental));
        }

        /// <summary>
        /// Creates a rental.
        /// </summary>
        /// <response code="201">Returns the newly created rentalId</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult<ResourceIdViewModel> Post(RentalBindingModel model)
        {
            var rental = _mapper.Map<Rental>(model);

            var rentalId = _rentalsService.Insert(rental);

            return CreatedAtAction(nameof(Get), new { rentalId = rentalId }, new ResourceIdViewModel { Id = rentalId });
        }

        /// <summary>
        /// Updates a rental.
        /// </summary>
        /// <response code="200">Returns a true value</response>
        /// <response code="400">If a conflict in the previous bookings occured.</response>
        /// <response code="404">If the rental is not found</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ResultViewModel> Update(RentalUpdateModel model)
        {
            var originalRental = _rentalsService.GetById(model.Id);

            if (originalRental is null)
                return NotFound(nameof(Rental));

            if (!_bookingsService.ValidateBookingsWithNewParameters(originalRental, model.Units, model.PreparationTimeInDays))
                return BadRequest("Conflict in the previous bookings occured.");

            originalRental.Units = model.Units;
            originalRental.PreparationTimeInDays = model.PreparationTimeInDays;
            var isSuccessful = _rentalsService.Update(originalRental);

            return Ok(new ResultViewModel { IsSuccessful = isSuccessful });
        }
    }
}
