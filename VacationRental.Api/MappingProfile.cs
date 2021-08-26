using AutoMapper;
using VacationRental.Api.Domain;
using VacationRental.Api.Models;

namespace VacationRental.Api
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateRentalsMaps();
            CreateBookingsMaps();
        }

        private void CreateRentalsMaps()
        {
            CreateMap<Rental, RentalViewModel>();
            CreateMap<RentalBindingModel, Rental>();
        }

        private void CreateBookingsMaps()
        {
            CreateMap<Booking, BookingViewModel>();
            CreateMap<BookingBindingModel, Booking>()
                .ForMember(b => b.End, c => c.Ignore())
                .ForMember(b => b.Start, c => c.MapFrom(bmm => bmm.Start.Date));
        }
    }
}
