using System.ComponentModel.DataAnnotations;

namespace MovieTicketBooking.Models
{
    public class UserToChangeEmail
    {
        [Required]
        public string Username { get; set; }

        [EmailAddress]
        public string NewEmail { get; set; }
    }
}
