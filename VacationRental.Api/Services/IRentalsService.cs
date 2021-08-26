using VacationRental.Api.Domain;

namespace VacationRental.Api.Services
{
    public interface IRentalsService
    {
        Rental GetById(int rentalId);
        int Insert(Rental rental);
    }
}