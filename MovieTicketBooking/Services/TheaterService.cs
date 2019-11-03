using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Policy;
using Dapper;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Services
{
    /// <summary>
    /// The <see cref="TheaterService"/> class contains methods for getting information from the database
    /// </summary>
    public class TheaterService : ITheaterService
    {
        private readonly string _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="TheaterService"/> class.
        /// Sets values to the configurations.
        /// </summary>
        /// <param name="configuration"></param>
        public TheaterService(IConfiguration configuration)
        {
#if DEBUG
            _connectionString = configuration.GetConnectionString("Development");
#else
            _connectionString = configuration.GetConnectionString("Production");
#endif
        }

        /// <summary>
        /// Gets all movie theaters by given city from the database.
        /// </summary>
        /// <param name="city"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public List<string> GetTheatersByCity(string city)
        {
            if (city == null)
            {
                throw new ArgumentNullException(nameof(city), "The city cannot be null.");
            }

            var query = @"SELECT [Name] FROM [Theater] 
                            WHERE Id_Location IN (SELECT Id FROM [Location] WHERE City = @City)";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                return connection.Query<string>(query, new {City = city}).ToList();
            }
        }

        /// <summary>
        /// Gets all movies in given movie theater.
        /// </summary>
        /// <param name="theater"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public List<MovieInTheater> GetMoviesByTheater(string theater)
        {
            if (theater == null)
            {
                throw new ArgumentNullException(nameof(theater), "Theater can not be null.");
            }

            var query = @"SELECT Id, Title, Genre, Producer, Production, [Length], Summary 
                        FROM [Movie]
                        WHERE Id IN (SELECT Id_Movie AS MovieId  FROM [MovieTimetable] 
			            WHERE Id IN (SELECT Id_MovieTimetable FROM [Theater_MovieTimetable]
							            WHERE Id_Theater IN (SELECT Id FROM [Theater] WHERE [Name] = @Theater)))";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                return connection.Query<MovieInTheater>(query, new {Theater = theater}).ToList();
            }
        }

        /// <summary>
        /// Gets the timetable of a given movie by id.
        /// </summary>
        /// <param name="movieTheater"></param>
        /// <param name="title"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public List<MovieTimetableToDisplay> GetTimetableByMovie(string movieTheater, string title)
        {
            if (title == null)
            {
                throw new ArgumentNullException(nameof(title), "Movie can not be null.");
            }

            if (movieTheater == null)
            {
                throw new ArgumentNullException(nameof(title), "Movie theater can not be null.");
            }

            var id = GetIdByMovieTitle(title);

            var query =
                @"SELECT MovieTimetable.Id, StartsAt, ProjectionType.Type AS ProjectionType, MovieCategory.Type AS MovieCategory
                            FROM [MovieTimetable]  JOIN [ProjectionType] ON Id_ProjectionType = ProjectionType.Id
						                            JOIN [MovieCategory] ON Id_MovieCategory = MovieCategory.Id
                            WHERE MovieTimetable.Id_Movie = @movieId 
                                AND movieTimetable.Id IN (SELECT Id_MovieTimetable FROM [Theater_MovieTimetable] 
                                                            WHERE Id_Theater IN 
                                                                (SELECT Id FROM [Theater] WHERE [Name] = @MovieTheater)) ORDER BY [StartsAt]";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                SqlParameter parameter = new SqlParameter();
                parameter.ParameterName = "@movieId";
                parameter.Value = id;

                command.Parameters.Add(parameter);

                SqlParameter movieTheaterParameter = new SqlParameter();
                movieTheaterParameter.ParameterName = "@MovieTheater";
                movieTheaterParameter.Value = movieTheater;

                command.Parameters.Add(movieTheaterParameter);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                List<MovieTimetableToDisplay> timetables = new List<MovieTimetableToDisplay>();

                while (reader.Read())
                {
                    MovieTimetableToDisplay timetable = new MovieTimetableToDisplay();

                    timetable.Id = reader.GetGuid(0);
                    timetable.StartsAt = reader.GetSqlString(1).IsNull ? null : reader.GetSqlString(1).Value;
                    timetable.ProjectionType = reader.GetSqlString(2).IsNull ? null : reader.GetSqlString(2).Value;
                    timetable.MovieCategory = reader.GetSqlString(3).IsNull ? null : reader.GetSqlString(3).Value;

                    timetables.Add(timetable);
                }

                connection.Close();
                return timetables;
            }
        }

        /// <summary>
        /// Gets all the movies which are on screen at the moment.
        /// </summary>
        public List<Movie> GetMovies(int page, int size)
        {
            if (page <= 0 || size <= 0)
            {
                throw new InvalidOperationException("Page and size should be positive numbers.");
            }

            var query = @"SELECT Id, Title, Genre, Producer, Production, Length FROM [Movie]
                            ORDER BY Title OFFSET @rowsToSkip ROWS FETCH NEXT @rowsToGet ROWS ONLY";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {

                return connection.Query<Movie>(query, new
                {
                    rowsToSkip = (page - 1) * size,
                    rowsToGet = size
                }).ToList();
            }
        }

        /// <summary>
        /// Gets pages count.
        /// </summary>
        public int GetPagesCount(int size)
        {
            if (size <= 0)
            {
                throw new InvalidOperationException("Size should be positive numbers.");
            }

            var query = @"SELECT Id, Title, Genre, Producer, Production, Length FROM [Movie]";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                List<Movie> movies = connection.Query<Movie>(query).ToList();
                return movies.Count / size;
            }
        }

        /// <summary>
        /// Gets all the cities where you can see choose a movie theater.
        /// </summary>
        public List<Location> GetCities()
        {
            var query = @"SELECT [Id], [City] FROM [Location]";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                return connection.Query<Location>(query).ToList();
            }
        }

        /// <summary>
        /// Gets movie categories and their descriptions.
        /// </summary>
        public List<MovieCategory> GetMovieCategories()
        {
            var query = "SELECT * FROM [MovieCategory] ORDER BY [Type]";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                return connection.Query<MovieCategory>(query).ToList();
            }
        }

        /// <summary>
        /// Gets movie poster name from the data base, to be used in the UI.
        /// </summary>
        public string GetMoviePosterName(string title)
        {
            if (title == null)
            {
                throw new ArgumentNullException(nameof(title), "Title can not be null.");
            }

            if (title.Contains('&'))
            {
                title = title.Replace("amp;", "");
            }

            var query = "SELECT [PosterName] AS Name FROM [Movie] WHERE [Title] = @Title";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                return connection.QueryFirstOrDefault<string>(query, new {Title = title});
            }
        }

        /// <summary>
        /// Returns types of movie tickets.
        /// </summary>
        public List<MovieTicket> GetMovieTicketTypes()
        {
            var query = "SELECT * FROM [TicketType]";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                return connection.Query<MovieTicket>(query).ToList();
            }
        }

        /// <summary>
        /// Gets timetable available tickets by given id.
        /// </summary>
        public int GetTimetableAvailableTickets(Guid timeTableId)
        {
            if (timeTableId == default)
            {
                throw new ArgumentNullException(nameof(timeTableId), "Id can not have the default value.");
            }

            var query = "SELECT [TicketsAvailable] FROM [MovieTimetable] WHERE [Id] = @id";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                return connection.QueryFirstOrDefault<int>(query, new {id = timeTableId});
            }
        }

        /// <summary>
        /// Books a ticket for currently logged in user.
        /// </summary>
        /// <param name="movieTimetableId"></param>
        /// <param name="ticketsToBook"></param>
        /// <param name="bookingUser" cref="UserBookTicketDto"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void BookTicket(Guid movieTimetableId, int ticketsToBook, UserBookTicketDto bookingUser)
        {
            if (bookingUser == null)
            {
                throw new ArgumentNullException(nameof(bookingUser), "User cannot be null.");
            }

            if (movieTimetableId == default)
            {
                throw new ArgumentNullException(nameof(movieTimetableId),
                    "Timetable id should not have default value.");
            }

            User user = GetUserByUsername(bookingUser.Username);

            var query = @"UPDATE [User] 
                            SET [TicketsBooked] = @number
                            WHERE [Username] = @name";

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@number", user.TicketsBooked + ticketsToBook);
            parameters.Add("@name", user.Username);

            var updatedAvailableTickets = GetAvailableTickets(movieTimetableId) - ticketsToBook;

            var availableTicketsQuery = @"UPDATE [MovieTimetable]
                                            SET [TicketsAvailable] = @updated
                                            WHERE [Id] = @timetableId";

            DynamicParameters ticketsParameters = new DynamicParameters();

            ticketsParameters.Add("@updated", updatedAvailableTickets);
            ticketsParameters.Add("@timetableId", movieTimetableId);

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.QueryFirstOrDefault(query, parameters);
                connection.QueryFirstOrDefault(availableTicketsQuery, ticketsParameters);
            }
        }

        /// <summary>
        /// Gets movie by given id.
        /// </summary>
        /// <param name="id" ></param>
        /// <exception cref="ArgumentNullException"></exception>
        public MovieColumns GetMovieTitleAndSummaryById(Guid id)
        {
            if (id == default)
            {
                throw new ArgumentNullException(nameof(id), "Id should not have default value.");
            }

            var query = "SELECT Title, Summary FROM [Movie] WHERE Id = @Id";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                return connection.QueryFirstOrDefault<MovieColumns>(query, new {Id = id});
            }
        }

        /// <summary>
        /// Gets the price of a ticket of particular type given.
        /// </summary>
        /// <param name="type" ></param>
        /// <exception cref="ArgumentNullException"></exception>
        public double GetTicketPriceByType(string type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type), "Ticket type can not be null.");
            }

            var query = @"SELECT Price FROM [TicketType] WHERE Type = @ticketType";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                return connection.QueryFirstOrDefault<int>(query, new { ticketType = type });
            }
        }

        private User GetUserByUsername(string username)
        {
            if (username == null)
            {
                throw new ArgumentNullException(nameof(username), "Username can not be null.");
            }

            var query = @"SELECT * FROM [User] WHERE [Username] = @name";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                return connection.QueryFirstOrDefault<User>(query, new {name = username});
            }
        }

        private Guid GetIdByMovieTitle(string title)
        {
            if (title == null)
            {
                throw new ArgumentNullException(nameof(title), "Title can not be null.");
            }

            var query = @"SELECT [Id] FROM [Movie] WHERE [Title] = @movieTitle";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                return connection.QueryFirstOrDefault<Guid>(query, new {movieTitle = title});
            }
        }

        private int GetAvailableTickets(Guid movieTimetableId)
        {
            if (movieTimetableId == default)
            {
                throw new ArgumentNullException(nameof(movieTimetableId),
                    "Timetable id should not have default value.");
            }

            var query = @"SELECT [TicketsAvailable] FROM [MovieTimetable] WHERE [Id] = @timetableId";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                return connection.QueryFirstOrDefault<int>(query, new {timetableId = movieTimetableId});
            }
        }
    }
}
