using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Api.Domain;

namespace VacationRental.Api.Services
{
    public partial class CalendarService : ICalendarService
    {
        private readonly IBookingsService _bookingsService;

        public CalendarService(IBookingsService bookingsService)
        {
            _bookingsService = bookingsService;
        }

        public List<CalendarDate> GetCalendar(Rental rental, DateTime start, int nights)
        {
            var bookings = _bookingsService.GetAllRentalBookings(rental, start, nights, true);

            var calendarBookingList = new List<CalendarDate>();

            foreach (var booking in bookings)
            {
                var unit = Enumerable.Range(1, rental.Units)
                        .FirstOrDefault(r => !calendarBookingList.Any(cb => cb.Date == booking.Start && cb.Unit == r));

                for (int i = 0; i < booking.Nights; i++)
                {
                    var datetime = booking.Start.AddDays(i);
                    calendarBookingList.Add(new CalendarDate
                    {
                        Date = datetime,
                        Type = CalendarDateType.Booking, 
                        BookingId = booking.Id,
                        Unit = unit
                    });
                }
                for (int i = 0; i < rental.PreparationTimeInDays; i++)
                {
                    var datetime = booking.Start.AddDays(booking.Nights + i);
                    calendarBookingList.Add(new CalendarDate
                    {
                        Date = datetime,
                        Type = CalendarDateType.PreparationTime,
                        Unit = unit
                    });
                }
            }

            return calendarBookingList;
        }
    }
}
