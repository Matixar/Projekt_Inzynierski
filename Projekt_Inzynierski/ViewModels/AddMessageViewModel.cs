using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Projekt_Inzynierski.ViewModels
{
    [QueryProperty(nameof(TripId), nameof(TripId))]
    class AddMessageViewModel:BaseViewModel
    {
        private int tripId;

        public int TripId { get => tripId; set => tripId = value; }
        public string Message { get => message; set => message = value; }

        private string message;

        public Command AddMessageCommand { get; }

        public AddMessageViewModel()
        {
            AddMessageCommand = new Command(AddMessage);
        }

        private async void AddMessage()
        {
            try
            {
                System.Net.Http.HttpClient _client = new System.Net.Http.HttpClient();
                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await Xamarin.Essentials.SecureStorage.GetAsync("token"));
                var client = new OpenApiService.OpenApiService("https://travelapi-app.azurewebsites.net/", _client);
                await client.AddMessageAsync(new OpenApiService.MessageDto()
                {
                    TextMessage = Message,
                    TripId = TripId
                }) ;
                await Shell.Current.GoToAsync("..");
            }
            catch(OpenApiService.ApiException e)
            {
                await Shell.Current.DisplayAlert("Błąd", e.Message, "OK");
            }
        }
    }
}
