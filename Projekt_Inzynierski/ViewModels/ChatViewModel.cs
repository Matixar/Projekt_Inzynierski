using Newtonsoft.Json;
using OpenApiService;
using Projekt_Inzynierski.Models;
using Projekt_Inzynierski.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Projekt_Inzynierski.ViewModels
{
    [QueryProperty(nameof(TripId), nameof(TripId))]
    class ChatViewModel:BaseViewModel
    {

        private int tripId;
        public ObservableCollection<ChatDto> Items { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public int TripId { get => tripId; set => tripId = value; }

        public ChatViewModel()
        {
            Title = "Czat";
            Items = new ObservableCollection<ChatDto>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            AddItemCommand = new Command(OnAddItem);
        }


        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                System.Net.Http.HttpClient _client = new System.Net.Http.HttpClient();
                var authHeaderValue = await Xamarin.Essentials.SecureStorage.GetAsync("token");
                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authHeaderValue);
                var client = new OpenApiService.OpenApiService("https://travelapi-app.azurewebsites.net/", _client);
                var items = await client.MessagesAsync(TripId);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (ApiException ex)
            {
                await Shell.Current.DisplayAlert("Błąd", JsonConvert.DeserializeObject<ErrorResponse>(ex.Response).error, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync($"{nameof(AddMessagePage)}?{nameof(AddMessageViewModel.TripId)}={TripId}");
        }
    }
}
