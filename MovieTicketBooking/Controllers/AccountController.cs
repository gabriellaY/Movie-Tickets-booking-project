using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Controllers
{
    /// <summary>
    /// The <see cref="AccountController"/> class controls the account operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly Services.IAccountService _accountService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="accountService"></param>
        public AccountController(Services.IAccountService accountService)
        {
            _accountService = accountService;
        }

        /// <summary>
        /// Changes password for already logged in user <see cref="UserToChangePassword"/>.
        /// </summary>
        /// <param name="user">User to change password.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="Exception"></exception>
        [HttpPost("changePassword")]
        public IActionResult ChangePassword(UserToChangePassword user)
        {
            try
            {
                _accountService.ChangePassword(user);

                return Ok(new
                {
                    Success = true,
                    Message = "Successfully changed password."
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
        /// Changes email for already logged in user <see cref="UserToChangeEmail"/>.
        /// </summary>
        /// <param name="user">User to change email.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        [HttpPost("changeEmail")]
        public IActionResult ChangeEmail(UserToChangeEmail user)
        {
            try
            {
                _accountService.ChangeEmail(user);

                return Ok(new
                {
                    Success = true,
                    Message = "Successfully changed email."
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
        /// Deletes account of a user <see cref="UserToDelete"/>.
        /// </summary>
        /// <param name="user">User to change email.</param>
        /// <exception cref="ArgumentNullException"></exception
        /// <exception cref="InvalidOperationException"></exception
        /// <exception cref="Exception"></exception>
        [HttpPost("deleteAccount")]
        public IActionResult DeleteAccount(UserToDelete user)
        {
            try
            {
                _accountService.DeleteAccount(user);

                return Ok(new
                {
                    Success = true,
                    Message = "Successfully deleted account."
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
    }
}

