using Projekt_Inzynierski.Models;
using Projekt_Inzynierski.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Projekt_Inzynierski.ViewModels
{
    public class TripsViewModel : BaseViewModel
    {

        private OpenApiService.TripInfoDto _selectedItem;

        public ObservableCollection<OpenApiService.TripInfoDto> Items { get; }

        public Command<OpenApiService.TripInfoDto> ItemTapped { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }

        public TripsViewModel()
        {
            Title = "Przejazdy";
            Items = new ObservableCollection<OpenApiService.TripInfoDto>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            ItemTapped = new Command<OpenApiService.TripInfoDto>(OnItemSelected);

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
                var trips = client.TripsAllAsync().Result;
                foreach (var item in trips)
                {
                    Items.Add(item);
                }
            }
            catch (OpenApiService.ApiException e)
            {
                await Shell.Current.DisplayAlert("Błąd", e.Response, "OK");
            }
            catch(AggregateException e)
            {
                await Shell.Current.DisplayAlert("Błąd", e.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
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

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewRidePage));
        }

        async void OnItemSelected(OpenApiService.TripInfoDto item)
        {
            if (item == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(TripDetailPage)}?{nameof(TripDetailViewModel.ItemId)}={item.Id}");
        }
    }
}
