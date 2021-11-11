using Projekt_Inzynierski.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_Inzynierski.Services
{
    class MockDataTrips : IDataStore<Trip>
    {
        readonly List<Trip> trips;

        public MockDataTrips()
        {
            trips = new List<Trip>()
            {
                new Trip {Id="1", AvalibleSeats = 2, Car = new Car{ Brand = "Mercedes", Model = "CLS", Color = "Czarny", Id = "1", RegistrationNumber ="LLE04224", Seats=3 },
                    DepartureTime = DateTime.Now, DeparturePlace = "Turka", DestinationPlace = "Lublin", DestinationTime = DateTime.Now, 
                Distance = 10, Driver = null, Passengers = null, TripPrice = 4},
                new Trip {Id="2", AvalibleSeats = 2, Car = new Car{ Brand = "Mercedes", Model = "CLS", Color = "Czarny", Id = "1", RegistrationNumber ="LLE04224", Seats=3 }
                , DepartureTime = DateTime.Now, DeparturePlace = "Lublin", DestinationPlace = "Turka", DestinationTime = DateTime.Now,
                Distance = 10, Driver = null, Passengers = null, TripPrice = 4},
                new Trip {Id="3", AvalibleSeats = 2, Car = new Car{ Brand = "Mercedes", Model = "CLS", Color = "Czarny", Id = "1", RegistrationNumber ="LLE04224", Seats=3 }
                , DepartureTime = DateTime.Now, DeparturePlace = "Warszawa", DestinationPlace = "Lublin", DestinationTime = DateTime.Now,
                Distance = 80, Driver = null, Passengers = null, TripPrice = 40},
            };
        }
        public async Task<bool> AddItemAsync(Trip item)
        {
            trips.Add(item);
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = trips.Where((Trip arg) => arg.Id == id).FirstOrDefault();
            trips.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Trip> GetItemAsync(string id)
        {
            return await Task.FromResult(trips.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Trip>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(trips);
        }

        public async Task<bool> UpdateItemAsync(Trip item)
        {
            var oldItem = trips.Where((Trip arg) => arg.Id == item.Id).FirstOrDefault();
            trips.Remove(oldItem);
            trips.Add(item);

            return await Task.FromResult(true);
        }
    }
}
