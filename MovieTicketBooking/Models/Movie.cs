using System;

namespace MovieTicketBooking.Models
{
    public class Movie
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Genre { get; set; }

        public string Producer { get; set; }

        public string Production { get; set; }

        public int Length { get; set; }
    }
}
