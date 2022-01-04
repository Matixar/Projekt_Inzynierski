using Projekt_Inzynierski.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace Projekt_Inzynierski.ViewModels
{
    [QueryProperty(nameof(UserId), nameof(UserId))]
    class UserDetailsViewModel : BaseViewModel
    {
        private int userId;

        private int id;

        private OpenApiService.MemberDto user;
        public OpenApiService.MemberDto User
        {
            get => user;
            set => SetProperty(ref user, value);
        }

        public UserDetailsViewModel()
        {
            AddOpinionCommand = new Command(AddOpinions);
        }

        public async void AddOpinions()
        {
            await Shell.Current.GoToAsync($"{nameof(AddOpinion)}?{nameof(AddOpinionViewModel.UserId)}={Id}&{nameof(AddOpinionViewModel.UserHash)}={UserId}");
        }

        public Command AddOpinionCommand { get; }
        public int UserId
        {
            get
            {
                return userId;
            }
            set
            {
                userId = value;
                LoadItemId(value);
            }
        }

        public int Id { get => id; set => id = value; }

        public async void LoadItemId(int userId)
        {
            try
            {
                System.Net.Http.HttpClient _client = new System.Net.Http.HttpClient();
                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await Xamarin.Essentials.SecureStorage.GetAsync("token"));
                var client = new OpenApiService.OpenApiService("https://travelapi-app.azurewebsites.net/", _client);
                
                User = await client.UserAsync(UserId.ToString());
                Id = User.Id;

            }
            catch (Exception e)
            {
                Debug.WriteLine("Failed to Load Item " + e.Message);
            }
        }
    }
}
