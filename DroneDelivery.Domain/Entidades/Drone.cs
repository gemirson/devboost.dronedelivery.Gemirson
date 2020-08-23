using DroneDelivery.Domain.Core;
using DroneDelivery.Domain.Enum;
using DroneDelivery.Domain.Helpers;
using System;
using Geolocation;
using DroneDelivery.Utility;

namespace DroneDelivery.Domain.Entidades
{
    public class Drone : EntidadeBase<Guid>
    {

        public double Capacidade { get; private set; }

        public double Velocidade { get; private set; }

        public double Autonomia { get; private set; }

        public double Carga { get; private set; }

        public DateTime? HoraCarregamento { get; private set; }

        public Pedido Pedido { get; set; }

        public DroneStatus Status { get; set; }

        protected Drone() { }

        public Drone(Guid id, double capacidade, double velocidade, double autonomia, double carga, DroneStatus status)
        {
            Id = id;
            Capacidade = capacidade;
            Velocidade = velocidade;
            Autonomia = autonomia;
            Carga = carga;
            Status = status;
        }

        public void AtualizarStatusDrone(DroneStatus status)
        {
            Status = status;
        }

        public void AdicionarDroneParaCarregar()
        {
            HoraCarregamento = DateTime.Now;
        }

        public void LiberarDroneCarregado()
        {
            HoraCarregamento = null;
        }

        public bool VerificarDroneAceitaOPesoPedido(double pesoPedido)
        {
            return Capacidade > pesoPedido * 1000;
        }

        public bool TraceRotaDrone(Localizacao inicio, Localizacao fim, double autonomia) {
           

            double trecho_inicio = GeoCalculator.GetDistance(inicio.Latitude, inicio.Longitude,Utils.LATITUDE_INICIAL, Utils.LONGITUDE_INICIAL , 1, DistanceUnit.Meters);

            double trecho_fim = GeoCalculator.GetDistance(fim.Latitude, fim.Longitude, Utils.LATITUDE_INICIAL, Utils.LONGITUDE_INICIAL , 1, DistanceUnit.Meters);

            double autonomiaTotal = (trecho_inicio - trecho_fim)/(Velocidade*60);

            if (autonomiaTotal <= autonomia) return true;
            return false;


        }
               
    }
}
