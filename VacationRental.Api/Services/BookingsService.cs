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

        public bool IsBookingAvailable(Booking newBooking, Rental rental)
        {
            var fullUnits = _bookingsRepository.Table.Count(booking => booking.RentalId == rental.Id
                    && (booking.Start <= newBooking.Start.Date && EndOfBlocking(booking, rental) > newBooking.Start.Date)
                    || (booking.Start < EndOfBlocking(newBooking, rental) && EndOfBlocking(booking, rental) >= EndOfBlocking(newBooking, rental))
                    || (booking.Start > newBooking.Start && EndOfBlocking(booking, rental) < EndOfBlocking(newBooking, rental)));

            return fullUnits < rental.Units;
        }

        public IList<Booking> GetAllRentalBookings(Rental rental, DateTime start, int nights, bool considerPreparationTime = false)
        {
            var bookings = _bookingsRepository.Table.Where(booking => booking.RentalId == rental.Id
                    && booking.Start < start.AddDays(nights) 
                    && (considerPreparationTime ? EndOfBlocking(booking, rental) : EndOfBookig(booking)) > start);
            return bookings.ToList();
        }

        public bool ValidateBookingsWithNewParameters(Rental rental, int units, int preparationTimeInDays)
        {
            if (units >= rental.Units && preparationTimeInDays <= rental.PreparationTimeInDays)
                return true;

            return !_bookingsRepository.Table.Where(booking => booking.RentalId == rental.Id)
                .Select(booking =>
                    Enumerable.Range(0, booking.Nights + preparationTimeInDays).Select(r => booking.Start.AddDays(r)))
                .SelectMany(bookingDate => bookingDate)
                .GroupBy(bookingDate => bookingDate)
                .Any(bookingDateGroups => bookingDateGroups.Count() > units);
        }

        protected DateTime EndOfBookig(Booking booking)
        {
            return booking.Start.AddDays(booking.Nights);
        }

        protected DateTime EndOfBlocking(Booking booking, Rental rental)
        {
            return booking.Start.AddDays(booking.Nights + rental.PreparationTimeInDays);
        }
    }
}
