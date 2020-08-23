using DroneDelivery.Data.Data;
using DroneDelivery.Data.Repositorios.IRepository;
using DroneDelivery.Domain.Entidades;
using DroneDelivery.Domain.Interfaces;
using System.Threading.Tasks;

namespace DroneDelivery.Data.Repositorios
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly DroneDbContext _context;

        public UnitOfWork(DroneDbContext context)
        {
            _context = context;
            Pedidos = new PedidoRepository(_context);
            Drones = new DroneRepository(_context);
            Intinerarios = new IntinerarioRepository(_context);
        }
        
        public IPedidoRepository Pedidos { get; private set; }

        public IDroneRepository Drones { get; private set; }

        public IIntinerarioRepository Intinerarios { get; private set; }

        public void Dispose()
        {
            _context.Dispose();
        }

       public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
