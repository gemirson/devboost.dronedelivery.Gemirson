using DroneDelivery.Application.Interfaces;
using DroneDelivery.Application.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DroneDelivery.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AtualizarStatusDroneController : ControllerBase
    {
        private readonly IDroneService _droneService;

        public AtualizarStatusDroneController(IDroneService droneService)
        {
            _droneService = droneService;
        }

        [HttpGet]
        public async Task<IEnumerable<DroneModel>> Atualizar()
        {
            return await _droneService.ObterAsync();
        }

       



    }
}
