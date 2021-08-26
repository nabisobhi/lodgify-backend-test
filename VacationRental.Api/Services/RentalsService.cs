using VacationRental.Api.Data;
using VacationRental.Api.Domain;

namespace VacationRental.Api.Services
{
    public class RentalsService : IRentalsService
    {
        private readonly IRepository<Rental> _rentalRepository;

        public RentalsService(IRepository<Rental> rentalRepository)
        {
            _rentalRepository = rentalRepository;
        }

        public Rental GetById(int rentalId)
        {
            return _rentalRepository.GetById(rentalId);
        }

        public int Insert(Rental rental)
        {
            return _rentalRepository.Insert(rental);
        }
    }
}
