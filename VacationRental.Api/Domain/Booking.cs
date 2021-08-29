using System;

namespace VacationRental.Api.Domain
{
    public class Booking : IEntity
    {
        public int Id { get; set; }
        public int RentalId { get; set; }
        public DateTime Start { get; set; }
        public int Nights { get; set; }
    }
}
