using DroneDelivery.Data.Data;
using DroneDelivery.Domain.Entidades;
using DroneDelivery.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DroneDelivery.Data.Repositorios
{
    public class IntinerarioRepository : IIntinerarioRepository
    {
        private readonly DroneDbContext _context;
        public IntinerarioRepository(DroneDbContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(Intinerario intinerario)
        {
            await _context.Intinerarios.AddAsync(intinerario);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Intinerario intinerario)
        {
            _context.Entry(intinerario).State = EntityState.Modified;
             await  _context.SaveChangesAsync() ;
           
        }

        public async Task<Intinerario> ObterAsync(Guid id)
        {
            return await _context.Intinerarios.FirstOrDefaultAsync(x => x.IdDrone == id);
        }
    }
}
