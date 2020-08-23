using DroneDelivery.Application.Interfaces;
using DroneDelivery.Application.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DroneDelivery.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DroneController : ControllerBase
    {
        private readonly IDroneService _droneService;

        public DroneController(IDroneService droneService)
        {
            _droneService = droneService;
        }

        [HttpGet]
        public async Task<IEnumerable<DroneModel>> Obter()
        {
            return await _droneService.ObterAsync();
        }

        [HttpGet("situacao")]
        public async Task<IEnumerable<DroneSituacaoModel>> ListarDrones()
        {
            return await _droneService.ListarDronesAsync();
        }


        /// <summary>
        /// Criar um drone
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     capacidade [gramas]
        ///     velocidade [m/s]
        ///     autonomia  [m]
        ///     carga: [minutos]
        ///     status : [Livre = 1, EmManutencao = 2,EmEntrega = 3,EmCarga = 4, EmAguardandoNovo=5,EmCheckout=6]
        /// 
        ///     POST /drone
        ///     {
        ///        "capacidade": 1200,        
        ///        "velocidade":3, 
        ///        "autonomia": 30,
        ///        "carga: 10, 
        ///        "status":1 
        ///     }
        ///
        /// </remarks>        
        /// <param name="droneModel"></param>  
        [HttpPost]
        public async Task<IActionResult> Adicionar(DroneModel droneModel)
        {
            await _droneService.AdicionarAsync(droneModel);

            return Ok();
        }


    }
}
