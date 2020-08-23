using AutoMapper;
using DroneDelivery.Application.Helpers;
using DroneDelivery.Application.Interfaces;
using DroneDelivery.Application.Models;
using DroneDelivery.Data.Repositorios.IRepository;
using DroneDelivery.Domain.Entidades;
using DroneDelivery.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DroneDelivery.Application.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly Application_Helpers application_Helpers;

        public PedidoService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            application_Helpers = new Application_Helpers(unitOfWork);
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

            ///-------------------------------Get drone------------------------------////

            droneDisponivel  = await application_Helpers.DroneDisponivel(pedido, drones);

            intinerario = await application_Helpers.GetIntinerarioAsync(pedido, drones);

            RestanteAutonomia = Application_Helpers.AutonomiaRestanteDrone(pedido, drones);

           
            if (droneDisponivel == null)
            {
                pedido.AtualizarStatusPedido(PedidoStatus.AguardandoEntrega);
                
            }
            else
            {
                pedido.Drone = droneDisponivel;
                pedido.AtualizarStatusPedido(PedidoStatus.EmEntrega);
                droneDisponivel.AtualizarStatusDrone(droneDisponivel.Status == DroneStatus.Livre ? DroneStatus.EmAguardandoNovo: DroneStatus.EmCheckout);

                await  application_Helpers.GerenciarIntinerario(droneDisponivel, intinerario, pedido, RestanteAutonomia);
                           

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
