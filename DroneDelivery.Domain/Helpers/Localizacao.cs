using System;
using System.Collections.Generic;
using System.Text;

namespace DroneDelivery.Domain.Helpers
{
    public struct Localizacao
    {
        public Localizacao(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public double Latitude { get; }
        public double Longitude { get; }
    }
}
