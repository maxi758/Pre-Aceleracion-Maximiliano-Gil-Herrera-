using System.ComponentModel.DataAnnotations;

namespace DisneyAPI.ViewModels.Auth.Register
{
    public class RegisterRequestViewModel
    {
        [Required]
        [MinLength(6)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

    }
}