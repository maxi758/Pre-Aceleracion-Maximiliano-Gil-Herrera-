using System.ComponentModel.DataAnnotations;

namespace DisneyAPI.ViewModels.Services.MailService
{
    public class MailServiceRequestViewModel
    {
        [Required]
        [MinLength(6)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
