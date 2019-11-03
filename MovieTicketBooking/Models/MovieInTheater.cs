using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTicketBooking.Models
{
    public class MovieInTheater
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Genre { get; set; }
    }
}
