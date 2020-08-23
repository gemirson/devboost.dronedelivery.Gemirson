using DroneDelivery.Data.Repositorios.IRepository;
using DroneDelivery.Domain.Entidades;
using DroneDelivery.Domain.Enum;
using DroneDelivery.Domain.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DroneDelivery.Application.Helpers
{
    public class Application_Helpers
    {

        private readonly IUnitOfWork _unitOfWork;

        public Application_Helpers(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Drone> DroneDisponivel(Pedido pedido,IEnumerable<Drone> drones)
        {
            // temos que procurar drones disponiveis
            Drone droneDisponivel = null;
            Intinerario intinerario = null;
           
            foreach (var drone in drones)
            {
                var droneTemAutonomia = pedido.ValidarDistanciaEntrega(Utility.Utils.LATITUDE_INICIAL, Utility.Utils.LONGITUDE_INICIAL, drone.Velocidade, drone.Autonomia);

                var droneAceitaPeso = drone.VerificarDroneAceitaOPesoPedido(pedido.Peso);

                if (!droneTemAutonomia || !droneAceitaPeso)
                    continue;

                intinerario = await _unitOfWork.Intinerarios.ObterAsync(drone.Id);

                if (drone.Status == DroneStatus.Livre)
                {
                    droneDisponivel = drone;
                    break;
                }

                if ((drone.Status == DroneStatus.EmAguardandoNovo) && pedido.RestantePeso(intinerario.PesoAtual) && drone.TraceRotaDrone(new Localizacao(pedido.Latitude, pedido.Longitude), new Localizacao(intinerario.Latitude, intinerario.Longitude), intinerario.AutonomiaAtual) && intinerario != null
                     )
                {
                    droneDisponivel = drone;
                    break;
                }
            }

            return droneDisponivel;
        }

        public static double AutonomiaRestanteDrone(Pedido pedido, IEnumerable<Drone> drones) {
            
            double restanteAutonomia = 0; ;

            foreach (var drone in drones)
            {
                if (drone.Status == DroneStatus.Livre)
                {
                    restanteAutonomia = pedido.RestanteAutonomia(Utility.Utils.LATITUDE_INICIAL, Utility.Utils.LONGITUDE_INICIAL, drone.Velocidade, drone.Autonomia);
                    break;
                }

              
            }

            return restanteAutonomia;
        }

        public async Task<Intinerario> GetIntinerarioAsync(Pedido pedido, IEnumerable<Drone> drones)
        {                     
            Intinerario intinerario = null;

            foreach (var drone in drones)
            {
               
                intinerario = await _unitOfWork.Intinerarios.ObterAsync(drone.Id);

                if (drone.Status == DroneStatus.Livre)
                {
                    break;
                }

                if ((drone.Status == DroneStatus.EmAguardandoNovo) && pedido.RestantePeso(intinerario.PesoAtual) && drone.TraceRotaDrone(new Localizacao(pedido.Latitude, pedido.Longitude), new Localizacao(intinerario.Latitude, intinerario.Longitude), intinerario.AutonomiaAtual) && intinerario != null
                     )
                {
                 
                    break;
                }
            }

            return intinerario;
        }

        public async Task GerenciarIntinerario(Drone drone, Intinerario intinerario, Pedido pedido,double autonomia) {

            if ((drone.Status == DroneStatus.EmAguardandoNovo || drone.Status == DroneStatus.EmCheckout) && intinerario != null)
            {
                intinerario.AutonomiaAtual += autonomia;
                intinerario.IdDrone = drone.Id;
                intinerario.PesoAtual += pedido.Peso;
                intinerario.Latitude = pedido.Latitude;
                intinerario.Longitude = pedido.Longitude;
                await _unitOfWork.Intinerarios.AtualizarAsync(intinerario);
            
            }
            else
            {
                await _unitOfWork.Intinerarios.AdicionarAsync(new Intinerario(drone.Id, pedido.Peso, autonomia, pedido.Latitude, pedido.Longitude));
            }

        }
    }
}
