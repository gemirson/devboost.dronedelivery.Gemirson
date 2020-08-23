using DroneDelivery.Data.Data;
using DroneDelivery.Domain.Entidades;
using DroneDelivery.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DroneDelivery.Data.Repositorios
{
    public class DroneRepository : IDroneRepository
    {
        private readonly DroneDbContext _context;

        public DroneRepository(DroneDbContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(Drone drone)
        {
            await _context.Drones.AddAsync(drone);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Drone>> ObterAsync()
        {
            return await _context.Drones.Include(x => x.Pedido).ToListAsync();
        }

        public async Task<Drone> ObterAsync(Guid id)
        {
            return await _context.Drones.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Drone> AtualizarAsync()
        {
            return await _context.Drones.FirstOrDefaultAsync(x => x.Status == Domain.Enum.DroneStatus.EmCheckout || x.Status == Domain.Enum.DroneStatus.EmAguardandoNovo);
        }

        public async Task RemoverAsync(Guid id)
        {
            var drone = await _context.Drones.FirstOrDefaultAsync(x => x.Id == id);
            _context.Remove(drone);
        }

        public void Remover(Drone drone)
        {
            _context.Remove(drone);
        }

        public async Task AtualizaraAsync(Drone drone)
        {
                 _context.Entry(drone).State = EntityState.Modified;
                await _context.SaveChangesAsync();

            
        }
    }
}
