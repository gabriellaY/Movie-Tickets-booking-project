using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTicketBooking.Models
{
    public class MovieTicket
    {
        private Guid Id { get; set; }

        public string Type { get; set; }

        public double Price { get; set; }
    }
}
