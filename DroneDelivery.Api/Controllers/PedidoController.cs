using DroneDelivery.Application.Interfaces;
using DroneDelivery.Application.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DroneDelivery.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;

        public PedidoController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        [HttpGet]
        public async Task<IEnumerable<PedidoModel>> Obter()
        {
            return await _pedidoService.ObterAsync();
        }


        /// <summary>
        /// Criar um pedido
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     peso: [g]
        ///     POST /pedido
        ///     {
        ///        "peso": 10,  
        ///        "latitude": -23.5753639,
        ///        "longitude": -46.692417
        ///     }
        ///
        /// </remarks>        
        /// <param name="pedidoModel"></param>  
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Adicionar(PedidoModel pedidoModel)
        {
            var response = await _pedidoService.AdicionarAsync(pedidoModel);

            if (!response)
                return BadRequest();

            return Ok();
        }


    }
}
