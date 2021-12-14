using Newtonsoft.Json;
using OpenApiService;
using Projekt_Inzynierski.Models;
using Projekt_Inzynierski.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Projekt_Inzynierski.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string email;
        private string password;
        public Command LoginCommand { get; }
        public string Email { get => email; set => SetProperty(ref email, value); }
        public string Password { get => password; set => SetProperty(ref password, value); }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
        }

        private async void OnLoginClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            var client = new OpenApiService.OpenApiService("https://travelapi-app.azurewebsites.net/", new System.Net.Http.HttpClient());
            var login = new LoginDto
            {
                Email = Email,
                Password = Password
            };
            try
            {
                
                var result = await client.LoginAsync(login);
                await Xamarin.Essentials.SecureStorage.SetAsync("hashcode", result.UserHash.ToString());
                await Xamarin.Essentials.SecureStorage.SetAsync("token", result.Token);
                await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
            }

            catch(ApiException e)
            {
                await Shell.Current.DisplayAlert("Błąd", JsonConvert.DeserializeObject<ErrorResponse>(e.Response).error, "OK");
            }
        }
    }
}
