using Projekt_Inzynierski.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Projekt_Inzynierski.ViewModels
{
    public class CarViewModel:BaseViewModel
    {
        private OpenApiService.Car _selectedItem;

        public ObservableCollection<OpenApiService.Car> Items { get; }

        public Command<OpenApiService.Car> ItemTapped { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }

        public CarViewModel()
        {
            Title = "Moje pojazdy";
            Items = new ObservableCollection<OpenApiService.Car>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            ItemTapped = new Command<OpenApiService.Car>(OnItemSelected);

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
                var cars = await client.CarsAllAsync();
                foreach (var item in cars)
                {
                    Items.Add(item);
                }
            }
            catch (OpenApiService.ApiException e)
            {
                await Shell.Current.DisplayAlert("Błąd", e.Response + e.StatusCode + e.Message, "OK");
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

        public OpenApiService.Car SelectedItem
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
            await Shell.Current.GoToAsync(nameof(NewCarPage));
        }

        async void OnItemSelected(OpenApiService.Car item)
        {

            if (item == null)
                return;
            // This will push the ItemDetailPage onto the navigation stack
            var dialog = await Shell.Current.DisplayAlert("Usunięcie pojazdu", "Czy chcesz usunąć pojazd?", "tak", "nie");
            if (dialog)
            {
                try
                {
                    System.Net.Http.HttpClient _client = new System.Net.Http.HttpClient();
                    var authHeaderValue = await Xamarin.Essentials.SecureStorage.GetAsync("token");
                    _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authHeaderValue);
                    var client = new OpenApiService.OpenApiService("https://travelapi-app.azurewebsites.net/", _client);
                    await client.CarsDELETEAsync(item.Id);
                    await Shell.Current.DisplayAlert("Potwierdzenie", "Usunięto pojazd", "OK");
                }
                catch (OpenApiService.ApiException e)
                {
                    if (e.StatusCode != 200)
                        await Shell.Current.DisplayAlert("Błąd", e.Response + e.StatusCode + e.Message, "OK");
                    else
                        await Shell.Current.DisplayAlert("Potwierdzenie", "Usunięto pojazd", "OK");
                }
            }
        }
    }
}
