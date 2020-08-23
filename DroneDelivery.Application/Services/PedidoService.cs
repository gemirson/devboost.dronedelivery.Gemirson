using AutoMapper;
using DroneDelivery.Application.Interfaces;
using DroneDelivery.Application.Models;
using DroneDelivery.Data.Repositorios.IRepository;
using DroneDelivery.Domain.Entidades;
using DroneDelivery.Domain.Enum;
using DroneDelivery.Domain.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DroneDelivery.Application.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PedidoService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IMapper Mapper { get; }

        public async Task<bool> AdicionarAsync(PedidoModel pedidoModel)
        {
            var pedido = _mapper.Map<PedidoModel, Pedido>(pedidoModel);

            if (!pedido.ValidarPesoPedido(Utility.Utils.CARGA_MAXIMA))
                return false;

            // temos que procurar drones disponiveis
            Drone droneDisponivel = null;
            Intinerario intinerario = null;
            double RestanteAutonomia = 0;

            var drones = await _unitOfWork.Drones.ObterAsync();

            foreach (var drone in drones)
            {
                var droneTemAutonomia = pedido.ValidarDistanciaEntrega(Utility.Utils.LATITUDE_INICIAL, Utility.Utils.LONGITUDE_INICIAL, drone.Velocidade, drone.Autonomia);

                var droneAceitaPeso = drone.VerificarDroneAceitaOPesoPedido(pedido.Peso);

                if (!droneTemAutonomia || !droneAceitaPeso)
                    continue;

                intinerario = await _unitOfWork.Intinerarios.ObterAsync(drone.Id);
                  

                if (drone.Status == DroneStatus.Livre) {
                    RestanteAutonomia = pedido.RestanteAutonomia(Utility.Utils.LATITUDE_INICIAL, Utility.Utils.LONGITUDE_INICIAL, drone.Velocidade, drone.Autonomia);
                    droneDisponivel = drone;
                    break;
                }

               if ((drone.Status == DroneStatus.EmAguardandoNovo) && pedido.RestantePeso(intinerario.PesoAtual) && drone.TraceRotaDrone(new Localizacao(pedido.Latitude, pedido.Longitude), new Localizacao(intinerario.Latitude, intinerario.Longitude), intinerario.AutonomiaAtual) && intinerario !=null
                    )
                {
                    droneDisponivel = drone;
                    break;
                }
            }

            if (droneDisponivel == null)
            {
                pedido.AtualizarStatusPedido(PedidoStatus.AguardandoEntrega);
                
            }
            else
            {
                pedido.DroneId = droneDisponivel.Id;
                pedido.AtualizarStatusPedido(PedidoStatus.EmEntrega);
                droneDisponivel.AtualizarStatusDrone(droneDisponivel.Status == DroneStatus.Livre ? DroneStatus.EmAguardandoNovo: DroneStatus.EmCheckout);

                if ((droneDisponivel.Status == DroneStatus.EmAguardandoNovo || droneDisponivel.Status == DroneStatus.EmCheckout) && intinerario != null)
                {
                    intinerario.AutonomiaAtual += RestanteAutonomia;
                    intinerario.IdDrone = droneDisponivel.Id;
                    intinerario.PesoAtual += pedido.Peso;
                    intinerario.Latitude = pedido.Latitude;
                    intinerario.Longitude = pedido.Longitude;
                    await _unitOfWork.Intinerarios.AtualizarAsync(intinerario);

                }
                else
                {
                    await _unitOfWork.Intinerarios.AdicionarAsync(new Intinerario(droneDisponivel.Id, pedido.Peso, RestanteAutonomia, pedido.Latitude, pedido.Longitude));
                }

            }
                               
            await _unitOfWork.Pedidos.AdicionarAsync(pedido);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<PedidoModel>> ObterAsync()
        {
            var pedidos = await _unitOfWork.Pedidos.ObterAsync();

            return _mapper.Map<IEnumerable<Pedido>, IEnumerable<PedidoModel>>(pedidos);
        }

        public async Task<PedidoModel> ObterAsync(Guid id)
        {
            var pedido = await _unitOfWork.Pedidos.ObterAsync(id);

            return _mapper.Map<Pedido, PedidoModel>(pedido);
        }

        public async Task Remover(PedidoModel pedidoModel)
        {
            var pedido = _mapper.Map<PedidoModel, Pedido>(pedidoModel);

            _unitOfWork.Pedidos.Remover(pedido);
            await _unitOfWork.SaveAsync();
        }

        public async Task RemoverAsync(Guid id)
        {
            await _unitOfWork.Pedidos.RemoverAsync(id);
            await _unitOfWork.SaveAsync();
        }
    }
}
