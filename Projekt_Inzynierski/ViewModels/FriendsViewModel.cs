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
    class FriendsViewModel: BaseViewModel
    {
        private OpenApiService.MemberDto _selectedItem;

        public ObservableCollection<OpenApiService.MemberDto> Items { get; }

        public Command<OpenApiService.MemberDto> ItemTapped { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }

        public FriendsViewModel()
        {
            Title = "Znajomi";
            Items = new ObservableCollection<OpenApiService.MemberDto>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            ItemTapped = new Command<OpenApiService.MemberDto>(OnItemSelected);

            AddItemCommand = new Command(OnAddItem);
        }
        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                System.Net.Http.HttpClient _client = new System.Net.Http.HttpClient();
                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(await Xamarin.Essentials.SecureStorage.GetAsync("token"));
                var client = new OpenApiService.OpenApiService("https://travelapi-app.azurewebsites.net/", _client);
                var users = client.UsersAllAsync().Result;
                foreach (var item in users)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
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

        public OpenApiService.MemberDto SelectedItem
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
            await Shell.Current.DisplayAlert("Uwaga", "Funkcjonalność rozwojowa", "OK");
        }

        async void OnItemSelected(OpenApiService.MemberDto item)
        {
            if (item == null)
                return;
            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(UserDetailsPage)}?{nameof(UserDetailsViewModel.UserId)}={item.UserHash}");
        }
    }
}

