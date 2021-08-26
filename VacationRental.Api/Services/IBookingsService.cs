using System;
using System.Collections.Generic;
using VacationRental.Api.Domain;

namespace VacationRental.Api.Services
{
    public interface IBookingsService
    {
        Booking GetById(int BookingId);
        int Insert(Booking Booking);
        bool IsBookingAvailable(Booking newBooking, Rental rental, bool considerPreparationTime = true);
        IList<Booking> GetAllRentalBookings(int rentalId, DateTime date);
    }
}