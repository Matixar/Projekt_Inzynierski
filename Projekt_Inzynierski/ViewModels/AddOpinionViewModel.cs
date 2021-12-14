﻿using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Projekt_Inzynierski.ViewModels
{
    [QueryProperty(nameof(UserId), nameof(UserId))]
    class AddOpinionViewModel:BaseViewModel
    {
        private string description;

        public string Description { get => description; set => description = value; }
        public string Selection { get => selection; set => selection = value; }

        private string selection;

        private int userId;

        public Command AddOpinionCommand { get; }
        public int UserId { get => userId; set => userId = value; }

        public AddOpinionViewModel()
        {
            AddOpinionCommand = new Command(AddOpinion);
        }

        private async void AddOpinion()
        {
            try
            {
                System.Net.Http.HttpClient _client = new System.Net.Http.HttpClient();
                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await Xamarin.Essentials.SecureStorage.GetAsync("token"));
                var client = new OpenApiService.OpenApiService("https://travelapi-app.azurewebsites.net/", _client);
                var opinion = new OpenApiService.OpinionDto();
                opinion.UserId = UserId;
                opinion.OpinionValue = Convert.ToInt32(Selection);
                opinion.OpinionDescription = Description;
                var send = await client.AddOpinionAsync(opinion);
                await Shell.Current.DisplayAlert("Potwierdzenie", "Dodano opinię.", "OK");
                await Shell.Current.Navigation.PopAsync();
            }
            catch (OpenApiService.ApiException e)
            {
                await Shell.Current.DisplayAlert("Błąd", e.StatusCode + e.Message, "OK");
                await Shell.Current.Navigation.PopAsync();
            }
        }
    }
}
