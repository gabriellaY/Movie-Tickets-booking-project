using System;

namespace MovieTicketBooking.Models
{
    public class MovieTimetable
    {
        public Guid Id { get; set; }

        public Guid MovieId { get; set; }

        public string StartsAt { get; set; }

        public Guid ProjectionTypeId { get; set; }

        public Guid MovieCategoryId{ get; set; }

        public int TicketsAvailable { get; set; }
    }
}
