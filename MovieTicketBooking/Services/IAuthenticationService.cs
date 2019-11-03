using MovieTicketBooking.Models;

namespace MovieTicketBooking.Services
{
    public interface IAuthenticationService
    {
        /// <summary>
        /// Registers new user in the system.
        /// </summary>
        /// <param name="user">User for registration</param>
        void Register(UserForRegistrationDto user);

        /// <summary>
        /// Registers new user in the system.
        /// </summary>
        /// <param name="user">User for registration</param>
        string Login(UserForLoginDto user);

        /// <summary>
        /// Gets email of a user by given username
        /// </summary>
        /// <param name="username">User for registration</param>
        string GetEmailByUsername(string username);
    }
}
