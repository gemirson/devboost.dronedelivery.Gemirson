using DroneDelivery.Domain.Entidades;
using DroneDelivery.Domain.Enum;
using Geolocation;

namespace DroneDelivery.Domain.Helpers
{
    public static class Helper_Utils
    {
        public static bool ValidadePesoTraceRota(Pedido pedido, Drone drone, Intinerario intinerario) {
            if ((drone.Status == DroneStatus.EmAguardandoNovo) &&
                pedido.RestantePeso(intinerario.PesoAtual) &&
                drone.TraceRotaDrone(new Localizacao(pedido.Latitude, pedido.Longitude), new Localizacao(intinerario.Latitude, intinerario.Longitude), intinerario.AutonomiaAtual)
                  && intinerario != null
                       )
            {
                return true;
            }

            return false;
        }

        public static double TempoDeslocamento(double latitudeInicial, double longitudeInicial, double latitudeFinal,double longitudeFinal, double velocidadeDrone) 
       {
            double distance = GeoCalculator.GetDistance(latitudeInicial, longitudeInicial, latitudeFinal, longitudeFinal, 1, DistanceUnit.Meters);
            if (distance <= 0)
                return 0;

            //velocidade em m/s
            //T = d / v

             return (((distance * 2) / velocidadeDrone) / 60);


        }
    }
}
