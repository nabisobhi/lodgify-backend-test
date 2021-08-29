using Microsoft.AspNetCore.Mvc;
using System;

namespace VacationRental.Api.Models
{
    public class GetCalendarRequestModel
    {
        [BindProperty(Name = "rentalId")]
        public int RentalId { get; set; }
        [BindProperty(Name = "start")]
        public DateTime Start { get; set; }
        [BindProperty(Name = "nights")]
        public int Nights { get; set; }
    }
}
