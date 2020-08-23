using DroneDelivery.Domain.Entidades;
using DroneDelivery.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
