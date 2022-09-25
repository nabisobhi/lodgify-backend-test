using System;
using System.Collections.Generic;
using VacationRental.Api.Domain;

namespace VacationRental.Api.Services
{
    public interface ICalendarService
    {
        List<CalendarDate> GetCalendar(Rental rental, DateTime start, int nights);
    }
}