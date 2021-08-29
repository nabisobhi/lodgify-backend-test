using System;

namespace VacationRental.Api.Domain
{
    public class CalendarDate
    {
        public DateTime Date { get; set; }
        public CalendarDateType Type { get; set; }
        public int BookingId { get; set; }
        public int Unit { get; set; }
    }
}
