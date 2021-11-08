using System;

namespace DisneyAPI.ViewModels.Auth.Login
{
    public class LoginResponseViewModel
    {
        public string Token { get; set; }
        public DateTime ValidTo { get; set; }
        public LoginResponseViewModel()
        {
        }

        public LoginResponseViewModel(string token, DateTime validTo)
        {
            Token = token;
            ValidTo = validTo;
        }

        
    }
}
