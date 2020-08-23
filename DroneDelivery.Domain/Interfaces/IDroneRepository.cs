using DroneDelivery.Domain.Entidades;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DroneDelivery.Domain.Interfaces
{
    public interface IDroneRepository
    {
        Task<IEnumerable<Drone>> ObterAsync();

        Task<Drone> ObterAsync(Guid id);

        Task AdicionarAsync(Drone drone);

        Task RemoverAsync(Guid id);
        void Remover(Drone drone);

        Task AtualizaraAsync(Drone drone);
    }
}
