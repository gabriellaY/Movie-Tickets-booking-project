using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTicketBooking.Models
{
    public class Theater
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid Location { get; set; }
    }
}
