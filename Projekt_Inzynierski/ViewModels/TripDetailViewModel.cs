using Projekt_Inzynierski.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace Projekt_Inzynierski.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class TripDetailViewModel : BaseViewModel
    {
        private string itemId;
        private string departurePlace;
        private string destinationPlace;
        private string driver;
        private List<User> passengers;
        private DateTime departureTime;
        private DateTime destinationTime;
        private int avalibleSeats;
        private string distance;
        private string car;
        private string tripPrice;
        public string Id { get; set; }
        public string DeparturePlace
        {
            get => departurePlace;
            set => SetProperty(ref departurePlace, value);
        }
        public string DestinationPlace
        {
            get => destinationPlace;
            set => SetProperty(ref destinationPlace, value);
        }

        public string Driver
        {
            get => driver;
            set => SetProperty(ref driver, value);
        }

        public List<User> Passengers
        {
            get => passengers;
            set => SetProperty(ref passengers, value);
        }
        public DateTime DepartureTime
        {
            get => departureTime;
            set => SetProperty(ref departureTime, value);
        }

        public string ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
                LoadItemId(value);
            }
        }

        public DateTime DestinationTime { get => destinationTime; set => SetProperty(ref destinationTime, value); }
        public int AvalibleSeats { get => avalibleSeats; set => SetProperty(ref avalibleSeats, value); }
        public string Distance { get => distance; set => SetProperty(ref distance, value); }
        public string Car { get => car; set => SetProperty(ref car, value); }
        public string TripPrice { get => tripPrice; set => SetProperty(ref tripPrice, value); }

        public async void LoadItemId(string itemId)
        {
            try
            {
                var item = await DataStoreTrip.GetItemAsync(itemId);
                Id = item.Id;
                DeparturePlace = item.DeparturePlace;
                DepartureTime = item.DepartureTime;
                DestinationPlace = item.DestinationPlace;
                DestinationTime = item.DestinationTime;
                AvalibleSeats = item.AvalibleSeats;
                Distance = item.Distance.ToString() + "km";
                Car = item.Car.Brand + " " + item.Car.Model;
                TripPrice = item.TripPrice.ToString() + "zł";
                Driver = item.Driver.Name;
                Passengers = item.Passengers;
                
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
    }
}
