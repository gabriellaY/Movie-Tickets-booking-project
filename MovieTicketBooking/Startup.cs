using System;
using System.Data.SqlClient;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MovieTicketBooking.Services;

namespace MovieTicketBooking
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSingleton<IAuthenticationService, AuthenticationService>();
            services.AddSingleton<ITheaterService, TheaterService>();
            services.AddSingleton<IAccountService, AccountService>();

            var key = Configuration.GetValue<string>("SecretKey");
            var keyBytes = Encoding.ASCII.GetBytes(key);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(x =>
                {
                    x.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = context =>
                        {
                            var accountService = context.HttpContext.RequestServices.GetRequiredService<IAccountService>();
                            var username = context.Principal.Identity.Name;
                    
                            if (!accountService.DoesUserExist(username))
                            {
                                context.Fail("Unauthorized");
                            }
                            return Task.CompletedTask;
                        }
                    };
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            Task.Run(ResetAvailableTickets);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseMvc();

        }

        /// <summary>
        /// Resets the number of the available tickets.
        /// </summary>
        public void ResetAvailableTickets()
        {
            var today = DateTime.Today;

            while (true)
            {
                Thread.Sleep(TimeSpan.FromMinutes(1));

                var current =  DateTime.Today;

                if (today.Date < current.Date)
                {
                    var query = @"UPDATE [MovieTimetable]
                        SET[TicketsAvailable] = 50";

                    using (SqlConnection connection = new SqlConnection(Configuration.GetConnectionString("Development")))
                    {
                        connection.Query(query);
                    }
                }
            }
        }
    }
}
