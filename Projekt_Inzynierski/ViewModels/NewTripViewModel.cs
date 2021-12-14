using OpenApiService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Projekt_Inzynierski.ViewModels
{
    class NewTripViewModel : BaseViewModel
    {
        public TimeSpan Time { get => time; set => time = value; }
        public DateTimeOffset Date { get => date; set => date = value; }

        private DateTimeOffset date;

        private TimeSpan time;

        private string price;

        private string startFrom;

        private string endIn;

        private List<Car> carList;

        private Car selectedCar;

        private Position startPosition;

        private Position endPosition;

        private bool startFromMapVisible;

        private bool endInMapVisible;

        private int emptySeats;

        public Command StartFromMapCommand { get; }

        public Command EndInMapCommand { get; }

        public Command StartFromButtonCommand { get; }

        public Command EndInButtonCommand { get; }

        public Command RefreshCarPickerCommand { get; }



        public NewTripViewModel()
        {
            Date = DateTimeOffset.Now;
            Time = DateTimeOffset.Now.TimeOfDay;
            CarList = new List<Car>();
            SaveCommand = new Command(AddNewTrip);
            StartFromMapCommand = new Command(StartFromMapAsync);
            EndInMapCommand = new Command(EndInMap);
            StartFromButtonCommand = new Command(StartFromButton);
            EndInButtonCommand = new Command(EndInButton);
            RefreshCarPickerCommand = new Command(async () => await GetCarsFromApi());
            
        }

        private async void StartFromMapAsync(object args)
        {
            var arg = (MapClickedEventArgs)args;
            StartPosition = arg.Position;
            Geocoder geoCoder = new Geocoder();
            var possibleAddress = await geoCoder.GetAddressesForPositionAsync(StartPosition);
            StartFrom = possibleAddress.FirstOrDefault();
            Debug.WriteLine(StartFrom);
            StartFromMapVisible = false;
        }

        private async void EndInMap(object args)
        {
            var position = (MapClickedEventArgs)args;
            EndPosition = position.Position;
            Geocoder geoCoder = new Geocoder();
            var possibleAddress = await geoCoder.GetAddressesForPositionAsync(EndPosition);
            EndIn = possibleAddress.FirstOrDefault();
            Debug.WriteLine(EndIn);
            EndInMapVisible = false;
        }

        private void StartFromButton()
        {
            StartFromMapVisible = true;
        }

        private void EndInButton()
        {
            EndInMapVisible = true;
        }

        public async Task GetCarsFromApi()
        {
            IsBusy = true;
            try
            {
                CarList.Clear();
                System.Net.Http.HttpClient _client = new System.Net.Http.HttpClient();
                var authHeaderValue = await Xamarin.Essentials.SecureStorage.GetAsync("token");
                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authHeaderValue);
                var client = new OpenApiService.OpenApiService("https://travelapi-app.azurewebsites.net/", _client);
                var cars = await client.CarsAllAsync();
                foreach (var car in cars)
                {
                    CarList.Add(car);
                }
                
            }
            catch(ApiException e)
            {
                await Shell.Current.DisplayAlert("Błąd", e.StatusCode.ToString() + " " + e.Message, "OK");
            }
            finally
            {
                IsBusy = false;
                OnPropertyChanged();
            }
        }

        public async Task OnAppearing()
        {
            IsBusy = true;
            SelectedCar = null;
            await GetCarsFromApi();
        }
        public Command SaveCommand { get; }
        public string Price { get => price; set => price = value; }
        public string StartFrom { get => startFrom; set { startFrom = value; OnPropertyChanged(); } }
        public string EndIn { get => endIn; set { endIn = value; OnPropertyChanged(); } }

        public List<Car> CarList { get => carList;
            set
            {
                carList = value;
                OnPropertyChanged();
            }
        }

        public Car SelectedCar { get => selectedCar; set { selectedCar = value; OnPropertyChanged(); } }
        public Position StartPosition { get => startPosition; set => SetProperty(ref startPosition, value); }
        public Position EndPosition { get => endPosition; set => SetProperty(ref endPosition, value); }

        public bool StartFromMapVisible { get => startFromMapVisible; set { SetProperty(ref startFromMapVisible, value); OnPropertyChanged(); } }
        public bool EndInMapVisible { get => endInMapVisible; set { SetProperty(ref endInMapVisible, value); OnPropertyChanged();} }

        public int EmptySeats { get => emptySeats; set => emptySeats = value; }

        private bool AreFieldsNotEmpty()
        {
            return Price.Length > 0 && StartFrom.Length > 0 && EndIn.Length > 0 && SelectedCar != null;
        }

        async void AddNewTrip()
        {
            try
            {
                if (AreFieldsNotEmpty() && (EmptySeats < 1 || EmptySeats > SelectedCar.NumberOfSeats))
                    await AppShell.Current.DisplayAlert("Ostrzeżenie","Wpisano złą ilość wolnych miejsc. Samochód ma maksymalnie " + SelectedCar.NumberOfSeats + " wolnych miejsc.","OK");
                else
                {
                    System.Net.Http.HttpClient _client = new System.Net.Http.HttpClient();
                    _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await Xamarin.Essentials.SecureStorage.GetAsync("token"));
                    var client = new OpenApiService.OpenApiService("https://travelapi-app.azurewebsites.net/", _client);
                    var trip = new AddTripDto();
                    trip.Price = Convert.ToDouble(this.Price);
                    trip.StartFrom = this.StartFrom;
                    trip.EndIn = this.EndIn;
                    trip.StartTime = Date.Add(time);
                    trip.CarID = SelectedCar.Id;
                    trip.NumberOfSeats = EmptySeats;
                    var result = await client.AddTripAsync(trip);
                    await Shell.Current.DisplayAlert("Potwierdzenie", "Dodano przejazd.", "OK");
                    await Shell.Current.GoToAsync("..");
                }
            }
            catch (ApiException e)
            {
                await Shell.Current.DisplayAlert("Błąd", e.StatusCode.ToString() + " " + e.Message, "OK");
            }
            catch (NullReferenceException)
            {
                await Shell.Current.DisplayAlert("Błąd", "Uzupełnij brakujące pola", "OK");
            }
        }
    }
}
