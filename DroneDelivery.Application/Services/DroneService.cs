using AutoMapper;
using DroneDelivery.Application.Interfaces;
using DroneDelivery.Application.Models;
using DroneDelivery.Data.Repositorios.IRepository;
using DroneDelivery.Domain.Entidades;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DroneDelivery.Application.Services
{
    public class DroneService : IDroneService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DroneService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IMapper Mapper { get; }

        public async Task AdicionarAsync(DroneModel droneModel)
        {
            var drone = _mapper.Map<DroneModel, Drone>(droneModel);
            await _unitOfWork.Drones.AdicionarAsync(drone);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<DroneSituacaoModel>> ListarDronesAsync()
        {
            var drones = await _unitOfWork.Drones.ObterAsync();

            return _mapper.Map<IEnumerable<Drone>, IEnumerable<DroneSituacaoModel>>(drones);
        }

        public async Task<IEnumerable<DroneModel>> ObterAsync()
        {
            var drones = await _unitOfWork.Drones.ObterAsync();

            return _mapper.Map<IEnumerable<Drone>, IEnumerable<DroneModel>>(drones);
        }

        public async Task<DroneModel> ObterAsync(Guid id)
        {
            var drone = await _unitOfWork.Drones.ObterAsync(id);

            return _mapper.Map<Drone, DroneModel>(drone);
        }

        public async Task Remover(DroneModel droneModel)
        {
            var drone = _mapper.Map<DroneModel, Drone>(droneModel);

            _unitOfWork.Drones.Remover(drone);
            await _unitOfWork.SaveAsync();
        }

        public async Task RemoverAsync(Guid id)
        {
            await _unitOfWork.Drones.RemoverAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task AtualizarDrone()
        {
            var drones = await _unitOfWork.Drones.ObterAsync();

            foreach (var drone in drones)
            {
                if((drone.Status== Domain.Enum.DroneStatus.EmAguardandoNovo) ||(drone.Status == Domain.Enum.DroneStatus.EmCheckout))
                {
                    drone.Status = Domain.Enum.DroneStatus.EmEntrega;
                    await _unitOfWork.Drones.AtualizaraAsync(drone);

                }
            }
        }
    }
}
