using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DroneDelivery.Application.Interfaces;
using DroneDelivery.Application.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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


        [HttpPost]
        public async Task<IActionResult> Adicionar(DroneModel droneModel)
        {
            await _droneService.AdicionarAsync(droneModel);

            return Ok();
        }


    }
}
