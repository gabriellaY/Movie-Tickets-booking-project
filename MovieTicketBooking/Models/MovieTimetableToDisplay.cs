
using System;

namespace MovieTicketBooking.Models
{
    public class MovieTimetableToDisplay
    {
        public Guid Id { get; set; }

        public string StartsAt { get; set; }

        public string ProjectionType { get; set; }

        public string MovieCategory { get; set; }
    }
}
