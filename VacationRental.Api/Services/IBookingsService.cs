﻿using System;
using System.Collections.Generic;
using VacationRental.Api.Domain;

namespace VacationRental.Api.Services
{
    public interface IBookingsService
    {
        Booking GetById(int BookingId);
        int Insert(Booking Booking);
        bool IsBookingAvailable(Booking newBooking, Rental rental);
        IList<Booking> GetAllRentalBookings(Rental rental, DateTime start, int nights, bool considerPreparationTime = false);
        bool ValidateBookingsWithNewParameters(Rental rental, int units, int preparationTimeInDays);
    }
}