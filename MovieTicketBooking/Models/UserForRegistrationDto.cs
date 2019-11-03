using System.ComponentModel.DataAnnotations;

namespace MovieTicketBooking.Models
{
    public class UserForRegistrationDto
    {   
        [Required]
        public string Username { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Password should not be shorter than 8 characters")]
        [MaxLength(20, ErrorMessage = "Password should not be exceed 20 characters")]
        public string Password { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Password should not be shorter than 8 characters")]
        [MaxLength(20, ErrorMessage = "Password should not be exceed 20 characters")]
        public string ConfirmPassword { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
