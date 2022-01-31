using System;
using TSDelivery.Deliverymans.SiloHost.Grains.Enums;

namespace TSDelivery.Deliverymans.SiloHost.Grains.Models
{
    public class Location
    {
        public string Identification { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public LocationStatus Status { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}