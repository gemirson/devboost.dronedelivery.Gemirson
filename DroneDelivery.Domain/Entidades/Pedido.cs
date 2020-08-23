using DroneDelivery.Domain.Core;
using DroneDelivery.Domain.Enum;
using DroneDelivery.Domain.Helpers;
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
            double tempoEmMinutos = Helper_Utils.TempoDeslocamento(latitudeInicial, longitudeInicial, Latitude, Longitude,velocidadeDrone);
                      
            if (tempoEmMinutos > autonomiaDrone)
                return false;

            return true;
        }


        public double RestanteAutonomia(double latitudeInicial, double longitudeInicial, double velocidadeDrone, double autonomiaDrone)
        {
            double tempoEmMinutos = Helper_Utils.TempoDeslocamento(latitudeInicial, longitudeInicial, Latitude, Longitude, velocidadeDrone);

            tempoEmMinutos = autonomiaDrone - tempoEmMinutos;

            if (tempoEmMinutos > autonomiaDrone)
                return autonomiaDrone;
            return tempoEmMinutos;
                   
        }

        public bool RestantePeso(double PesoIntinerante)
        {
            return (Peso + PesoIntinerante)/1000 < Utility.Utils.CARGA_MAXIMA;

        }


    }
}
