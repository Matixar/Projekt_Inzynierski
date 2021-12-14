using Projekt_Inzynierski.Views;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Projekt_Inzynierski.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        private string username;
        public AboutViewModel()
        {
            Title = "Strona główna";
            GetLoggedUser();
            AddTripCommand = new Command(AddTrip);
            AddCarCommand = new Command(AddCar);
            SearchTripsCommand = new Command(SearchTrip);
            LogoutCommand = new Command(Logout);
        }

        public Command AddTripCommand { get; }

        public Command AddCarCommand { get; }

        public Command SearchTripsCommand { get; }

        public Command LogoutCommand { get; }

        public Command LoadUsernameCommand { get; }

        async void GetLoggedUser()
        {
            try
            {
                System.Net.Http.HttpClient _client = new System.Net.Http.HttpClient();
                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await Xamarin.Essentials.SecureStorage.GetAsync("token"));
                var client = new OpenApiService.OpenApiService("https://travelapi-app.azurewebsites.net/", _client);
                var user = await client.UserAsync(await Xamarin.Essentials.SecureStorage.GetAsync("hashcode"));
                Device.BeginInvokeOnMainThread(() => Username = "Witaj " + user.Name + "!");
            }
            catch (OpenApiService.ApiException e)
            {
                await Shell.Current.DisplayAlert("Błąd", e.Response, "OK");
            }
        }

        public string Username 
        { 
            get => username; 
            set 
            {
                username = value;
                OnPropertyChanged(nameof(Username));
            } 
        }

        private async void AddTrip()
        {
            await Shell.Current.GoToAsync($"{nameof(NewRidePage)}");
        }

        private async void AddCar()
        {
            await Shell.Current.GoToAsync($"{nameof(NewCarPage)}");
        }

        private async void SearchTrip()
        {
            await Shell.Current.GoToAsync($"//{nameof(SearchPage)}");
        }

        private async void Logout()
        {
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}