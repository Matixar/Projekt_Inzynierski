using Projekt_Inzynierski.Models;
using Projekt_Inzynierski.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Projekt_Inzynierski.ViewModels
{
    public class SearchViewModel : BaseViewModel
    {

        private OpenApiService.TripInfoDto _selectedItem;
        public ObservableCollection<OpenApiService.TripInfoDto> Items { get; }
        public Command<OpenApiService.TripInfoDto> ItemTapped { get; }

        private string startFrom;

        private string endIn;

        public string EndIn
        {
            get => endIn;

            set => endIn = value;
        }

        private DateTimeOffset startTime;

        public SearchViewModel()
        {
            Items = new ObservableCollection<OpenApiService.TripInfoDto>();
            SearchCommand = new Command(OnSearch);
            ItemTapped = new Command<OpenApiService.TripInfoDto>(OnItemSelected);
        }

        public Command SearchCommand { get; }
        public string StartFrom { get => startFrom; set => startFrom = value; }
        public DateTimeOffset StartTime { get => startTime; set => startTime = value; }

        private async void OnSearch()
        {
            try
            {
                Items.Clear();
                System.Net.Http.HttpClient _client = new System.Net.Http.HttpClient();
                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await Xamarin.Essentials.SecureStorage.GetAsync("token"));
                var client = new OpenApiService.OpenApiService("https://travelapi-app.azurewebsites.net/", _client);
                var tripSearch = new OpenApiService.TripsSearchDto();
                tripSearch.StartFrom = StartFrom;
                tripSearch.EndIn = EndIn;
                tripSearch.StartTime = StartTime;
                var result = await client.GetTripsAsync(tripSearch);
                foreach (var item in result)
                {
                    Items.Add(item);
                }
            }
            catch (OpenApiService.ApiException e)
            {
                await Shell.Current.DisplayAlert("Błąd", e.StatusCode + e.Message, "OK");
            }


        }

        public OpenApiService.TripInfoDto SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        async void OnItemSelected(OpenApiService.TripInfoDto item)
        {

            if (item == null)
                return;
            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(TripDetailPage)}?{nameof(TripDetailViewModel.ItemId)}={item.Id}");
        }
    }
}