using System;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Services
{
    public class AccountService : IAccountService
    {
        private readonly string _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationService"/> class.
        /// Sets values to the configurations.
        /// </summary>
        /// <param name="configuration"></param>
        public AccountService(IConfiguration configuration)
        {
#if DEBUG
            _connectionString = configuration.GetConnectionString("Development");
#else
            _connectionString = configuration.GetConnectionString("Production");
#endif
        }

        /// <summary>
        /// Changes password using as parameter <see cref="UserToChangePassword"/>.
        /// </summary>
        /// <param name="user">User for log in</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public void ChangePassword(UserToChangePassword user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User can not be null.");
            }

            if (!IsPasswordConfirmed(user.Username, user.OldPassword))
            {
                throw new InvalidOperationException("Wrong password.");
            }

            var query = @"UPDATE [User]
                        SET [Password] = @newPassword
                        WHERE [Username] = @name";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@name", user.Username);

                if (user.NewPassword != user.ConfirmNewPassword)
                {
                    throw new InvalidOperationException("Confirmation password is not equal to the password.");
                }

                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.NewPassword);

                parameters.Add("@newPassword", hashedPassword);

                connection.QueryFirstOrDefault<UserToChangePassword>(query, parameters);
            }
        }

        /// <summary>
        /// Changes email using as parameter <see cref="UserToChangeEmail"/>.
        /// </summary>
        /// <param name="user">User for log in</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public void ChangeEmail(UserToChangeEmail user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User can not be null.");
            }

            var query = @"UPDATE [User]
                            SET [Email] = @email
                            WHERE [Username] = @name";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                DynamicParameters parameters = new DynamicParameters();

                parameters.Add("@name", user.Username);
                parameters.Add("@email", user.NewEmail);

                connection.QueryFirstOrDefault<UserToChangeEmail>(query, parameters);
            }
        }

        /// <summary>
        /// Deletes user account from the database taking as parameter <see cref="UserToDelete"/>.
        /// </summary>
        /// <param name="user">User for log in</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public void DeleteAccount(UserToDelete user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User can not be null.");
            }

            if (!IsPasswordConfirmed(user.Username, user.Password))
            {
                throw new InvalidOperationException("Wrong password.");
            }

            var query = @"DELETE FROM [User]
                            WHERE [Username] = @name";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.QueryFirstOrDefault<UserToDelete>(query, new {name = user.Username});
            }
        }

        /// <summary>
        /// Checks if user exists in the database.
        /// </summary>
        /// <param name="username">User for log in</param>
        public bool DoesUserExist(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return false;
            }
            
            var query = @"SELECT COUNT(*) FROM [User] WHERE Username = @username";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var userCount = connection.QueryFirstOrDefault<int>(query, new {username });

                return userCount > 0;
            }
        }

        private bool IsPasswordConfirmed(string username, string password)
        {
            if (username == null)
            {
                throw new ArgumentNullException(nameof(username), "Username can not be null.");
            }

            if (password == null)
            {
                throw new ArgumentNullException(nameof(password), "Password can not be null.");
            }

            var query = @"SELECT Password FROM [User] WHERE Username = @user";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var hashedPassword = connection.QueryFirstOrDefault<string>(query, new { user = username });

                return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            }
        }
    }
}
