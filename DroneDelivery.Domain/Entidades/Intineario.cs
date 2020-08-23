using DroneDelivery.Domain.Core;
using DroneDelivery.Domain.Enum;
using Geolocation;
using System;

namespace DroneDelivery.Domain.Entidades
{
    public class Intinerario : EntidadeBase<Guid>
    {
        public  Guid IdDrone { get; set; }
        public double PesoAtual { get; set; }
        public double AutonomiaAtual { get;  set; }
        public double Latitude { get;  set; }
        public double Longitude { get;  set; }

        public Intinerario(Guid idDrone, double pesoAtual, double autonomiaAtual, double latitude, double longitude)
        {
            IdDrone = idDrone;
            PesoAtual = pesoAtual;
            AutonomiaAtual = autonomiaAtual;
            Latitude = latitude;
            Longitude = longitude;
        }
    } 
}