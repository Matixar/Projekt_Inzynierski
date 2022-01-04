using OpenApiService;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Projekt_Inzynierski.ViewModels
{
    class NewCarViewModel : BaseViewModel
    {
        private string mark;

        private string model;

        private string registrationNumber;

        private int numberOfSeats;

        private string color;

        private int productionYear;

        private bool markValid;

        private bool modelValid;

        private bool regValid;

        private bool colorValid;



        public string Mark { get => mark; set => mark = value; }
        public string Model { get => model; set => model = value; }
        public string RegistrationNumber { get => registrationNumber; set => registrationNumber = value; }
        public int NumberOfSeats { get => numberOfSeats; set => numberOfSeats = value; }
        public string Color { get => color; set => color = value; }

        public NewCarViewModel()
        {
            SaveCommand = new Command(AddNewCar);
            CancelCommand = new Command(Cancel);
        }

        public Command SaveCommand { get; }

        public Command CancelCommand { get; }
        public int ProductionYear { get => productionYear; set { productionYear = value; OnPropertyChanged(); } }
        public bool MarkValid { get => markValid; set { markValid = value; OnPropertyChanged(); } }
        public bool ModelValid { get => modelValid; set { modelValid = value; OnPropertyChanged(); } }
        public bool RegValid { get => regValid; set { regValid = value; OnPropertyChanged(); } }
        public bool ColorValid { get => colorValid; set { colorValid = value; OnPropertyChanged(); } }

        private async void Cancel()
        {
            await Shell.Current.GoToAsync("..");
        }
        private async void AddNewCar()
        {
            if(MarkValid && ModelValid && RegValid && ColorValid)
            try
            {
                System.Net.Http.HttpClient _client = new System.Net.Http.HttpClient();
                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await Xamarin.Essentials.SecureStorage.GetAsync("token"));
                var client = new OpenApiService.OpenApiService("https://travelapi-app.azurewebsites.net/", _client);
                var car = new OpenApiService.CarDto();
                car.Mark = Mark;
                car.Model = Model;
                car.RegistrationNumber = RegistrationNumber;
                car.NumberOfSeats = NumberOfSeats;
                car.Color = Color;
                car.ProductionYear = ProductionYear;

                var result = await client.AddCarAsync(car);
                await AppShell.Current.DisplayAlert("Potwierdzenie","Dodano pojazd.", "OK");
                await Shell.Current.GoToAsync("..");

            }
            catch (ApiException e)
            {
                await Shell.Current.DisplayAlert("Błąd", e.StatusCode.ToString() + " " + e.Message, "OK");
            }
            else
                await AppShell.Current.DisplayAlert("Błąd", "Wpisz poprawne dane", "OK");
        }

    }
}
