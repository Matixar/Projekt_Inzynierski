using System;
using System.Collections.Generic;
using System.Text;

namespace Projekt_Inzynierski.Models
{
    public class Trip
    {
        public string Id { get; set; }
        public string DeparturePlace { get; set; }

        public string DestinationPlace { get; set; }

        public User Driver { get; set; }

        public List<User> Passengers { get; set; }

        public DateTime DepartureTime { get; set; }

        public DateTime DestinationTime { get; set; }

        public int AvalibleSeats { get; set; }

        public float Distance { get; set; }

        public Car Car { get; set; }

        public float TripPrice { get; set; }
    }
}
