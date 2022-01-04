using Newtonsoft.Json;
using Projekt_Inzynierski.Models;
using Projekt_Inzynierski.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Projekt_Inzynierski.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class TripDetailViewModel : BaseViewModel
    {
        private int itemId;
        private string startFrom;
        private string destinationPlace;
        private string driver;
        private ICollection<OpenApiService.MemberDto> passengers;
        private OpenApiService.MemberDto passenger;
        private DateTimeOffset departureTime;
        private DateTime destinationTime;
        private int avalibleSeats;
        private string distance;
        private string car;
        private string tripPrice;
        private OpenApiService.MemberDto owner;
        public int Id { get; set; }
        public string StartFrom
        {
            get => startFrom;
            set => SetProperty(ref startFrom, value);
        }
        public string DestinationPlace
        {
            get => destinationPlace;
            set => SetProperty(ref destinationPlace, value);
        }

        public string Driver
        {
            get => driver;
            set => SetProperty(ref driver, value);
        }

        public ICollection<OpenApiService.MemberDto> Passengers
        {
            get => passengers;
            set => SetProperty(ref passengers, value);
        }
        public DateTimeOffset DepartureTime
        {
            get => departureTime;
            set => SetProperty(ref departureTime, value);
        }

        public TripDetailViewModel()
        {
            JoinTripCommand = new Command(JoinTrip);
            OpenChatCommand = new Command(OpenChat);
            DeleteCommand = new Command(DeleteTrip);
            CheckTripRoute = new Command(CheckRoute);
        }

        public int ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
                LoadItemId(value);
            }
        }

        private async void JoinTrip()
        {
            try
            {
                System.Net.Http.HttpClient _client = new System.Net.Http.HttpClient();
                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await Xamarin.Essentials.SecureStorage.GetAsync("token"));
                var client = new OpenApiService.OpenApiService("https://travelapi-app.azurewebsites.net/", _client);
                var user = await client.UserAsync(await Xamarin.Essentials.SecureStorage.GetAsync("hashcode"));
                var join = await client.AddPassengerAsync(new OpenApiService.PassengerDto()
                {
                    TripId = ItemId,
                    UserId = user.Id
                });
                await Shell.Current.DisplayAlert("Potwierdzenie", "Dołączyłeś do przejazdu", "ok");
            }
            catch(OpenApiService.ApiException e)
            {
                await Shell.Current.DisplayAlert("Błąd", JsonConvert.DeserializeObject<ErrorResponse>(e.Response).error, "OK");
            }
        }

        private async void OpenChat()
        {
            await Shell.Current.GoToAsync($"{nameof(ChatPage)}?{nameof(ChatViewModel.TripId)}={itemId}");
        }

        private async void CheckRoute()
        {
            
            var destinationPlaceUri = DestinationPlace.Replace(" ", "+").Replace(",","%2C").Replace("-","%2D");
            var startFromUri = StartFrom.Replace(" ", "+").Replace(",", "%2C").Replace("-", "%2D");
            if (Device.RuntimePlatform == Device.iOS)
            {
                await Launcher.OpenAsync("http://maps.apple.com/?daddr=" + destinationPlaceUri +",+saddr=" + startFromUri);
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                await Launcher.OpenAsync("http://maps.google.com/?daddr=" + destinationPlaceUri + ",+CA&saddr=" + startFromUri);
            }
        }

        private string ConvertStringToAscii(string text)
        {
            var sb = new StringBuilder();
            foreach (char c in text)
            {
                int unicode = c;
                if (unicode < 128)
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public Command JoinTripCommand { get; }

        public Command DeleteCommand { get; }

        public Command OpenChatCommand { get; }

        public Command CheckTripRoute { get; }

        public DateTime DestinationTime { get => destinationTime; set => SetProperty(ref destinationTime, value); }
        public int AvalibleSeats { get => avalibleSeats; set => SetProperty(ref avalibleSeats, value); }
        public string Distance { get => distance; set => SetProperty(ref distance, value); }
        public string Car { get => car; set => SetProperty(ref car, value); }
        public string TripPrice { get => tripPrice; set => SetProperty(ref tripPrice, value); }
        public OpenApiService.MemberDto Passenger { get => passenger; set => passenger = value; }
        public OpenApiService.MemberDto Owner { get => owner; set => owner = value; }

        public async void LoadItemId(int itemId)
        {
            try
            {
                System.Net.Http.HttpClient _client = new System.Net.Http.HttpClient();
                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await Xamarin.Essentials.SecureStorage.GetAsync("token"));
                var client = new OpenApiService.OpenApiService("https://travelapi-app.azurewebsites.net/", _client);
                var trip = await client.TripsAsync(itemId);
                
                Id = trip.Id;
                StartFrom = trip.StartFrom;
                DepartureTime = trip.StartTime;
                DestinationPlace = trip.EndIn;
                TripPrice = trip.Price + "zł";
                Owner = trip.Creator;
                Driver = trip.Creator.Name + " #" + trip.Creator.UserHash;
                Passengers = trip.Passenger;
                AvalibleSeats = trip.NumberOfSeats;
                Car = trip.Car.Mark + " " + trip.Car.Model + " rok " +trip.Car.ProductionYear + " (" + trip.Car.Color + ")" + "\nNr rejestracji: " + trip.Car.RegistrationNumber;
                
            }
            catch (OpenApiService.ApiException e)
            {
                await Shell.Current.DisplayAlert("Błąd",e.StatusCode + e.Message,"OK");
            }
        }

        private async void DeleteTrip()
        {
            try
            {
                System.Net.Http.HttpClient _client = new System.Net.Http.HttpClient();
                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await Xamarin.Essentials.SecureStorage.GetAsync("token"));
                var client = new OpenApiService.OpenApiService("https://travelapi-app.azurewebsites.net/", _client);
                var hash = await Xamarin.Essentials.SecureStorage.GetAsync("hashcode");
                var user = await client.UserAsync(hash);
                List<OpenApiService.MemberDto> pass = new List<OpenApiService.MemberDto>(Passengers);
                var pas = pass.Find(x => (x.UserHash.ToString() == hash));
                if (Owner.UserHash.ToString() == hash)
                {
                    var result = await Shell.Current.DisplayAlert("Usunięcie przejazdu", "Czy chcesz usunąć przejazd?", "TAK", "NIE");
                    if (result) 
                    {
                        await client.DeleteTripAsync(Id);
                        await Shell.Current.DisplayAlert("Potwierdzenie", "Usunięto przejazd", "OK");
                    }
                }
                else if (pas != null)
                {
                    var result = await Shell.Current.DisplayAlert("Anuluj przejazd", "Czy chcesz anulować dołączenie do przejazdu?", "TAK", "NIE");
                    if (result)
                    {
                        await client.DeletePassengerAsync(new OpenApiService.PassengerDto()
                        {
                            UserId = user.Id,
                            TripId = Id
                        });
                        await Shell.Current.DisplayAlert("Potwierdzenie", "Odłączono od przejazdu", "OK");
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlert("Ostrzeżenie", "Nie jesteś kierowcą/pasażerem przejazdu", "OK");
                }
            }
            catch(OpenApiService.ApiException e)
            {
                if(e.StatusCode != 200)
                    await Shell.Current.DisplayAlert("Błąd", e.Message, "ok");
                else
                    await Shell.Current.DisplayAlert("Potwierdzenie", "Usunięto", "OK");

            }
        }
    }
}
