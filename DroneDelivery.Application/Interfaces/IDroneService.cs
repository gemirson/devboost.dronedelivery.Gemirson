using DroneDelivery.Application.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DroneDelivery.Application.Interfaces
{
    public interface IDroneService
    {
        Task<IEnumerable<DroneSituacaoModel>> ListarDronesAsync();


        Task<IEnumerable<DroneModel>> ObterAsync();

        Task<DroneModel> ObterAsync(Guid id);

        Task AdicionarAsync(DroneModel drone);

        Task RemoverAsync(Guid id);
        Task Remover(DroneModel drone);

        Task AtualizarDrone();

    }
}
