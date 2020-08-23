using DroneDelivery.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
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
               
        /// <summary>
        /// Liberar todos os drones em Checkout e AguardandoNovo
        /// </summary>
        /// <remarks>
        [HttpGet]
        public async Task<IActionResult> Atualizar()
        {
            await _droneService.AtualizarDrone();
            return Ok();
        }

       



    }
}
