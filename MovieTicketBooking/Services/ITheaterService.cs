using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Services
{
    public interface ITheaterService
    {
        List<string> GetTheatersByCity(string city);

        List<MovieInTheater> GetMoviesByTheater(string theater);

        List<MovieTimetableToDisplay> GetTimetableByMovie(string movieTheater, string title);

        List<Movie> GetMovies(int page, int size);

        int GetPagesCount(int size);

        List<Location> GetCities();

        List<MovieCategory> GetMovieCategories();

        string GetMoviePosterName(string title);

        List<MovieTicket> GetMovieTicketTypes();

        double GetTicketPriceByType(string type);

        MovieColumns GetMovieTitleAndSummaryById(Guid id);

        int GetTimetableAvailableTickets(Guid timeTableId);

        void BookTicket(Guid movieTimetableId, int ticketsToBook, UserBookTicketDto bookingUser);
    }
}
