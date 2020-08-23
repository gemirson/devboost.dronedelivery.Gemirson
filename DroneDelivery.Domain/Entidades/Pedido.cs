using DroneDelivery.Domain.Core;
using DroneDelivery.Domain.Enum;
using Geolocation;
using System;

namespace DroneDelivery.Domain.Entidades
{
    public class Pedido : EntidadeBase<Guid>
    {
        public double Peso { get; private set; }

        public DateTime DataPedido { get; private set; }

        public double Latitude { get; private set; }
        public double Longitude { get; private set; }
        public PedidoStatus Status { get; private set; }

        public Guid? DroneId { get; set; }
        public Drone Drone { get; set; }



        protected Pedido() { }

        public Pedido(double peso, DateTime dataPedido, double latitude, double longitude, PedidoStatus status)
        {
            Peso = peso;
            DataPedido = dataPedido;
            Longitude = longitude;
            Latitude = latitude;
            Status = status;
        }

        public void AtualizarStatusPedido(PedidoStatus status)
        {
            Status = status;
        }


        public bool ValidarPesoPedido(double carga)
        {
            return Peso*0.001 < carga;
        }

        public bool ValidarDistanciaEntrega(double latitudeInicial, double longitudeInicial, double velocidadeDrone, double autonomiaDrone)
        {
            double distance = GeoCalculator.GetDistance(latitudeInicial, longitudeInicial, Latitude, Longitude, 1, DistanceUnit.Meters);
            if (distance <= 0)
                return false;

            //velocidade em m/s
            //T = d / v
            var tempoEmMinutos = ((distance * 2) / velocidadeDrone) / 60;

            if (tempoEmMinutos > autonomiaDrone)
                return false;

            return true;
        }


        public double RestanteAutonomia(double latitudeInicial, double longitudeInicial, double velocidadeDrone, double autonomiaDrone)
        {
            double distance = GeoCalculator.GetDistance(latitudeInicial, longitudeInicial, Latitude, Longitude, 1, DistanceUnit.Meters);
            if (distance <= 0)
                return 0;
           
            distance = autonomiaDrone -(((distance * 2) / velocidadeDrone) / 60);

            if (distance > autonomiaDrone)
                return autonomiaDrone;

            return distance;

                   
        }

        public bool RestantePeso(double PesoIntinerante)
        {
            return (Peso + PesoIntinerante)/1000 < Utility.Utils.CARGA_MAXIMA;

        }


    }
}
