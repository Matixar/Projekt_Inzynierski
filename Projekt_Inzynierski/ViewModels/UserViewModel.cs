using OpenApiService;
using Projekt_Inzynierski.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace Projekt_Inzynierski.ViewModels
{
    class UserViewModel: BaseViewModel
    {
        private string name;

        private Boolean gender;

        private string description;

        public Command SaveChangesCommand { get; }

        public Command CancelChangesCommand { get; }

        public Command BackCommand { get; }
        public string Name { 
            get => name; 
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public bool Gender { 
            get => gender; 
            set
            {
                gender = value;
                OnPropertyChanged(nameof(Gender));
            }
        }
        public string Description { 
            get => description; 
            set
            {
                description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public UserViewModel()
        {
            SaveChangesCommand = new Command(saveChanges);
            BackCommand = new Command(GoBack);
            CancelChangesCommand = new Command(LoadUser);
            LoadUser();
        }

        private async void GoBack()
        {
            await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
        }

        private async void LoadUser()
        {
            try
            {
                System.Net.Http.HttpClient _client = new System.Net.Http.HttpClient();
                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await Xamarin.Essentials.SecureStorage.GetAsync("token"));
                var client = new OpenApiService.OpenApiService("https://travelapi-app.azurewebsites.net/", _client);
                var result = await client.User2Async();
                Name = result.Member.Name;
                Description = result.Member.Description;
                if (result.Member.Gender == "Mężczyzna")
                    Gender = false;
                else
                    Gender = true;
            }
            catch(ApiException e)
            {
                await Shell.Current.DisplayAlert("Błąd", e.StatusCode + e.Message, "OK");
            }
        }

        private async void saveChanges()
        {
            try
            {
                System.Net.Http.HttpClient _client = new System.Net.Http.HttpClient();
                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await Xamarin.Essentials.SecureStorage.GetAsync("token"));
                var client = new OpenApiService.OpenApiService("https://travelapi-app.azurewebsites.net/", _client);
                var body = new OpenApiService.EditMemberDto()
                {
                    Email = null,
                    Name = this.Name,
                    Description = this.Description,
                    Gender = this.Gender ? "Kobieta" : "Mężczyzna"
                };
                var result = await client.UsersAsync(body);
                Debug.WriteLine("test");
                await Shell.Current.DisplayAlert("Potwierdzenie", "Pomyślnie edytowano użytkownika", "OK");
                GoBack();
            
            }
            catch(ApiException e)
            {
                await Shell.Current.DisplayAlert("Błąd", e.StatusCode + e.Message, "OK");
            }
        }
    }
}
