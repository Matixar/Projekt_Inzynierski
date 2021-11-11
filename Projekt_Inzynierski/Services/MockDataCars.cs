using Projekt_Inzynierski.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_Inzynierski.Services
{
    public class MockDataCars : IDataStore<Car>
    {
        readonly List<Car> cars;

        public MockDataCars()
        {
            cars = new List<Car>()
            {
                new Car {Id = "1", Brand = "Mercedes", Model = "CLS", RegistrationNumber = "LLE30620", Color = "Czarny", Seats = 3},
                new Car {Id = "2", Brand = "Jaguar", Model = "X308", RegistrationNumber = "LLE320320", Color = "Zielony", Seats = 3},
                new Car {Id = "3", Brand = "BMW", Model = "Seria 2", RegistrationNumber = "LLE31445", Color = "Czarny", Seats = 3},
                new Car {Id = "4", Brand = "Mercedes", Model = "A Klasa", RegistrationNumber = "LLE62534", Color = "Czarny", Seats = 3}
            };
        }
        public async Task<bool> AddItemAsync(Car item)
        {
            cars.Add(item);
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = cars.Where((Car arg) => arg.Id == id).FirstOrDefault();
            cars.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Car> GetItemAsync(string id)
        {
            return await Task.FromResult(cars.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Car>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(cars);
        }

        public async Task<bool> UpdateItemAsync(Car item)
        {
            var oldItem = cars.Where((Car arg) => arg.Id == item.Id).FirstOrDefault();
            cars.Remove(oldItem);
            cars.Add(item);

            return await Task.FromResult(true);
        }
    }
}
