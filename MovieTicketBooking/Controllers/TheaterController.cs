using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Controllers
{
    /// <summary>
    /// The <see cref="TheaterController"/> class controls the operations in the movie theater.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TheaterController : ControllerBase
    {
        private readonly Services.ITheaterService _theaterService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TheaterController"/> class.
        /// </summary>
        /// <param name="theaterService"></param>
        public TheaterController(Services.ITheaterService theaterService)
        {
            _theaterService = theaterService;
        }

        /// <summary>
        /// Books movie tickets by choice of the user <see cref="UserBookTicketDto"/> in the system.
        /// </summary>
        /// <param name="ticketsToBook"></param>
        /// <param name="bookingUser">User for registration</param>
        /// <param name="timetableId"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        [HttpPost("bookTicket/{timetableId}/{ticketsToBook}")]
        public IActionResult BookTicket(Guid timetableId, int ticketsToBook, UserBookTicketDto bookingUser)
        {
            try
            {
                _theaterService.BookTicket(timetableId, ticketsToBook, bookingUser);
                return Ok(new
                {
                    Success = true,
                    Message = $"Successfully booked {ticketsToBook} tickets."
                });
            }
            catch (ArgumentException argumentException)
            {
                return BadRequest(new
                {
                    Success = false,
                    Error = argumentException.Message
                });
            }
            catch (Exception exception)
            {
                return new ObjectResult(new
                {
                    Success = false,
                    Error = exception.Message
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        /// <summary>
        /// Gets movie theaters by given city.
        /// </summary>
        /// <param name="city"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        [HttpGet("theaters/{city}")]
        public IActionResult GetTheaters(string city)
        {
            try
            {
                var theaters = _theaterService.GetTheatersByCity(city);
                return Ok(new
                {
                    Success = true,
                    Theaters = theaters
                });
            }
            catch (ArgumentNullException argumentNullException)
            {
                return BadRequest(new
                {
                    Success = false,
                    Error = argumentNullException.Message
                });
            }
            catch (Exception exception)
            {
                return new ObjectResult(new
                {
                    Success = false,
                    Error = exception.Message
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        /// <summary>
        /// Get 'size' number of movies for given number of page.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        [HttpGet("movies/{page}/{size}")]
        public IActionResult GetMovies(int page, int size)
        {
            try
            {
                var movies = _theaterService.GetMovies(page, size);
                return Ok(new
                {
                    Success = true,
                    Movies = movies
                });
            }
            catch (InvalidOperationException invalidOperationException)
            {
                return BadRequest(new
                {
                    Success = false,
                    Error = invalidOperationException.Message
                });
            }
            catch (Exception exception)
            {
                return new ObjectResult(new
                {
                    Success = false,
                    Error = exception.Message
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        /// <summary>
        /// Calculates the needed number of pages by given number of movies that should be on one page.
        /// </summary>
        /// <param name="size"></param>
        /// <exception cref="Exception"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        [HttpGet("pages/{size}")]
        public IActionResult GetPagesCount(int size)
        {
            try
            {
                var pages = _theaterService.GetPagesCount(size);

                return Ok(new
                {
                    Success = true,
                    Pages = pages
                });
            }
            catch (InvalidOperationException invalidOperationException)
            {
                return BadRequest(new
                {
                    Success = false,
                    Error = invalidOperationException.Message
                });
            }
            catch (Exception exception)
            {
                return new ObjectResult(new
                {
                    Success = false,
                    Error = exception.Message
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        /// <summary>
        /// Gets movie categories.
        /// </summary>
        /// <exception cref="Exception"></exception>
        [HttpGet("categories")]
        public IActionResult GetMovieCategories()
        {
            try
            {
                var categories = _theaterService.GetMovieCategories();
                return Ok(new
                {
                    Success = true,
                    Categories = categories
                });
            }
            catch (Exception exception)
            {
                return new ObjectResult(new
                {
                    Success = false,
                    Error = exception.Message
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        /// <summary>
        /// Gets the name of movie poster by given title.
        /// </summary>
        /// <param name="title"></param>
        /// <exception cref="Exception"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        [HttpGet("moviePoster/{title}")]
        public IActionResult GetMoviePosterName(string title)
        {
            try
            {
                var poster = _theaterService.GetMoviePosterName(title);
                return Ok(new
                {
                    Success = true,
                    Poster = poster
                });
            }
            catch (ArgumentNullException argumentNullException)
            {
                return BadRequest(new
                {
                    Success = false,
                    Error = argumentNullException.Message
                });
            }
            catch (Exception exception)
            {
                return new ObjectResult(new
                {
                    Success = false,
                    Error = exception.Message
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        /// <summary>
        /// Gets the types of different movie tickets.
        /// </summary>
        /// <exception cref="Exception"></exception>
        [HttpGet("tickets")]
        public IActionResult GetMovieTicketTypes()
        {
            try
            {
                var types = _theaterService.GetMovieTicketTypes();
                return Ok(new
                {
                    Success = true,
                    Types = types
                });
            }
            catch (Exception exception)
            {
                return new ObjectResult(new
                {
                    Success = false,
                    Error = exception.Message
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        /// <summary>
        /// Gets all the cities.
        /// </summary>
        /// <exception cref="Exception"></exception>
        [HttpGet("cities")]
        public IActionResult GetCities()
        {
            try
            {
                var cities = _theaterService.GetCities();
                return Ok(new
                {
                    Success = true,
                    Cities = cities
                });
            }
            catch (Exception exception)
            {
                return new ObjectResult(new
                {
                    Success = false,
                    Error = exception.Message
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        /// <summary>
        /// Gets all movies which are on screen in given movie theater.
        /// </summary>
        /// <param name="theater"></param>
        /// <exception cref="Exception"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        [HttpGet("moviesInTheater/{theater}")]
        public IActionResult GetMoviesByTheater(string theater)
        {
            try
            {
                var movies = _theaterService.GetMoviesByTheater(theater);
                return Ok(new
                {
                    Success = true,
                    Movies = movies
                });
            }
            catch (ArgumentNullException argumentNullException)
            {
                return BadRequest(new
                {
                    Success = false,
                    Error = argumentNullException.Message
                });
            }
            catch (Exception exception)
            {
                return new ObjectResult(new
                {
                    Success = false,
                    Error = exception.Message
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        /// <summary>
        /// Gets the number of available tickets for particular timetable
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="Exception"></exception>
        /// <exception cref="ArgumentException"></exception>
        [HttpGet("timetableTickets/{id}")]
        public IActionResult GetTimetableAvailableTickets(Guid id)
        {
            try
            {
                var ticketsAvailable = _theaterService.GetTimetableAvailableTickets(id);

                return Ok(new
                {
                    Success = true,
                    TicketsAvailable = ticketsAvailable
                });
            }
            catch (ArgumentException argumentException)
            {
                return BadRequest(new
                {
                    Success = false,
                    Error = argumentException.Message
                });
            }
            catch (Exception exception)
            {
                return new ObjectResult(new
                {
                    Success = false,
                    Error = exception.Message
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        /// <summary>
        /// Gets the timetable in given movie theater of a movie by given movie title.
        /// </summary>
        /// <param name="movieTheater"></param>
        /// <param name="title"></param>
        /// <exception cref="Exception"></exception>
        /// <exception cref="ArgumentException"></exception>
        [HttpGet("timetable/{movieTheater}/{title}")]
        public IActionResult GetTimetableByMovie(string movieTheater, string title)
        {
            try
            {
                var timetable = _theaterService.GetTimetableByMovie(movieTheater, title);
                return Ok(new
                {
                    Success = true,
                    Timetable = timetable
                });
            }
            catch (ArgumentException argumentNullException)
            {
                return BadRequest(new
                {
                    Success = false,
                    Error = argumentNullException.Message
                });
            }
            catch (Exception exception)
            {
                return new ObjectResult(new
                {
                    Success = false,
                    Error = exception.Message
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        /// <summary>
        /// Gets the movie title and summary by given id of a movie.
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="Exception"></exception>
        /// <exception cref="ArgumentException"></exception>
        [HttpGet("summary/{id}")]
        public IActionResult GetMovieTitleAndSummaryById(Guid id)
        {
            try
            {
                var movieColumns = _theaterService.GetMovieTitleAndSummaryById(id);

                return Ok(new
                {
                    Success = true,
                    Columns = movieColumns
                });
            }
            catch (ArgumentException argumentException)
            {
                return BadRequest(new
                {
                    Success = false,
                    Error = argumentException.Message
                });
            }
            catch (Exception exception)
            {
                return new ObjectResult(new
                {
                    Success = false,
                    Error = exception.Message
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }

        }

        /// <summary>
        /// Gets the price of a ticket of given type.
        /// </summary>
        /// <param name="type"></param>
        /// <exception cref="Exception"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        [HttpGet("ticketPrice/{type}")]
        public IActionResult GetTicketPriceByType(string type)
        {
            try
            {
                var ticketPrice = _theaterService.GetTicketPriceByType(type);

                return Ok(new
                {
                    Success = true,
                    Price = ticketPrice
                });
            }
            catch (ArgumentNullException argumentNullException)
            {
                return BadRequest(new
                {
                    Success = false,
                    Error = argumentNullException.Message
                });
            }
            catch (Exception exception)
            {
                return new ObjectResult(new
                {
                    Success = false,
                    Error = exception.Message
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }
    }
}
