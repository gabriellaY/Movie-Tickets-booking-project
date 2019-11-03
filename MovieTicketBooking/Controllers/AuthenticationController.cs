using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.Models;
using System;
using Microsoft.AspNetCore.Authorization;

namespace MovieTicketBooking.Controllers
{
    /// <summary>
    /// The <see cref="AuthenticationController"/> class controls the authentication of users when register or login.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthenticationController : ControllerBase
    {
        private readonly Services.IAuthenticationService _authenticationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationController"/> class.
        /// </summary>
        /// <param name="authenticationService"></param>
        public AuthenticationController(Services.IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// Registers new user <see cref="UserForRegistrationDto"/> in the system.
        /// </summary>
        /// <param name="user">User for registration</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="Exception"></exception>
        [HttpPost("register")]
        [AllowAnonymous]
        public IActionResult Register(UserForRegistrationDto user)
        {
            try
            {
                _authenticationService.Register(user);

                return Ok(new
                {
                    Success = true,
                    Message = "Successfully created account."
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
        /// Logs in a user <see cref="UserForLoginDto"/>  which is already registered.
        /// </summary>
        /// <param name="user">User for log in</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="Exception"></exception> 
        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login(UserForLoginDto user)
        {
            try
            {
                var token = _authenticationService.Login(user);

                return Ok(new
                {
                    Success = true,
                    Message = "Successfully logged in.",
                    Username = user.Username,
                    Token = token
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

        [HttpGet("email/{username}")]
        public IActionResult GetEmailByUsrname(string username)
        {
            try
            {
                var email = _authenticationService.GetEmailByUsername(username);

                return Ok(new
                {
                    Success = true,
                    Email = email
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
