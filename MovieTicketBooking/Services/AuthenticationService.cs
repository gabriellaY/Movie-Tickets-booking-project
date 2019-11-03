using System;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Services
{
    /// <summary>
    /// The <see cref="AuthenticationService"/> class contains methods for performing operations in the database
    /// when there is a user to register or login.
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        private readonly string _connectionString;
        private readonly string _secretKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationService"/> class.
        /// Sets values to the configurations.
        /// </summary>
        /// <param name="configuration"></param>
        public AuthenticationService(IConfiguration configuration)
        {
#if DEBUG
            _connectionString = configuration.GetConnectionString("Development");
            _secretKey = configuration.GetValue<string>("SecretKey");
#else
            _connectionString = configuration.GetConnectionString("Production");
#endif
        }

        /// <summary>
        /// Registers new user <see cref="UserForRegistrationDto"/> in the system.
        /// </summary>
        /// <param name="user">User for registration</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public void Register(UserForRegistrationDto user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User to register cannot be null.");
            }

            var query = @"INSERT INTO [User](Id, Username, Password, Email)
                                VALUES(NEWID(), @name, @password, @userEmail)";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@name", user.Username);

                if (user.Password != user.ConfirmPassword)
                {
                    throw new InvalidOperationException("Confirmation password is not equal to the password.");
                }

                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);

                parameters.Add("@password", hashedPassword);
                parameters.Add("@userEmail", user.Email);

                if (IsUsernameUnique(connection, user.Username) && IsEmailUnique(connection, user.Email))
                {
                    connection.Query(query, parameters);
                }
                else
                {
                    throw new InvalidOperationException("User with this username or email already exists.");
                }
            }
        }

        /// <summary>
        /// Logs in a user <see cref="UserForLoginDto"/>  which is already registered.
        /// </summary>
        /// <param name="user">User for log in</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public string Login(UserForLoginDto user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "Parameter user for login cannot be null.");
            }

            var query = @"SELECT Password FROM [User] WHERE Username = @user";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var hashedPassword = connection.QueryFirstOrDefault<string>(query, new { user = user.Username });

                if (hashedPassword == null)
                {
                    throw new InvalidOperationException("Invalid username.");
                }

                if (!BCrypt.Net.BCrypt.Verify(user.Password, hashedPassword))
                {
                    throw new InvalidOperationException("Invalid password.");
                }

                var token = GenerateJwt(user);

                return token;
            }
        }

        /// <summary>
        /// Gets email of a user by given username
        /// </summary>
        /// <param name="username">User for registration</param>
        public string GetEmailByUsername(string username)
        {
            if (username == null)
            {
                throw new ArgumentNullException(nameof(username), "Parameter for username cannot be null.");
            }

            var query = @"SELECT Email FROM [User] WHERE Username = @user";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                return connection.QueryFirstOrDefault<string>(query, new {user = username});
            }
        }

        private string GenerateJwt(UserForLoginDto user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username)
                }),
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private bool IsUsernameUnique(SqlConnection connection, string username)
        {
            var query = "SELECT Username FROM [User] WHERE Username = @name";

            return connection.QueryFirstOrDefault<string>(query, new { name = username }) == null;
        }

        private bool IsEmailUnique(SqlConnection connection, string email)
        {
            var query = "SELECT Email FROM [User] WHERE Email = @userEmail";

            return connection.QueryFirstOrDefault<string>(query, new { userEmail = email }) == null;
        }
    }
}
