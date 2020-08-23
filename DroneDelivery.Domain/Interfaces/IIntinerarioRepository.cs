using DroneDelivery.Domain.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DroneDelivery.Domain.Interfaces
{
    public interface IIntinerarioRepository
    {
        Task AdicionarAsync(Intinerario intinerario);
        Task<Intinerario> ObterAsync(Guid id);
        Task AtualizarAsync(Intinerario intinerario);
    }
}
