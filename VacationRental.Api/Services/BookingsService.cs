using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Api.Data;
using VacationRental.Api.Domain;

namespace VacationRental.Api.Services
{
    public class BookingsService : IBookingsService
    {
        private readonly IRepository<Booking> _bookingsRepository;

        public BookingsService(IRepository<Booking> bookingsRepository)
        {
            _bookingsRepository = bookingsRepository;
        }

        public Booking GetById(int BookingId)
        {
            return _bookingsRepository.GetById(BookingId);
        }

        public int Insert(Booking Booking)
        {
            return _bookingsRepository.Insert(Booking);
        }

        public bool IsBookingAvailable(Booking newBooking, Rental rental, bool considerPreparationTime = true)
        {
            for (var i = 0; i < newBooking.Nights; i++)
            {
                var count = _bookingsRepository.Table.Count(booking => booking.RentalId == rental.Id
                        && (booking.Start <= newBooking.Start.Date && booking.End > newBooking.Start.Date)
                        || (booking.Start < newBooking.End && booking.End >= newBooking.End)
                        || (booking.Start > newBooking.Start && booking.End < newBooking.End));

                if (count >= rental.Units)
                    return false;
            }

            return true;
        }

        public IList<Booking> GetAllRentalBookings(int rentalId, DateTime date)
        {
            return _bookingsRepository.Table.Where(booking => booking.RentalId == rentalId
                    && booking.Start <= date && booking.End > date).ToList();
        }
    }
}
