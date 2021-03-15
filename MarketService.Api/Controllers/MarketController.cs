using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketService.Domain.Model;
using MarketService.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MarketService.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MarketController : ControllerBase
    {
        private readonly IMarketsService marketsService;

        public MarketController(IMarketsService marketsService)
        {
            this.marketsService = marketsService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(MarketModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var markets = await this.marketsService.GetMarkets();
            return Ok(markets);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MarketModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int id)
        {
            var market = await this.marketsService.GetMarketById(id);
            return Ok(market);
        }

        [HttpPost()]
        [Route("Add")]
        [ProducesResponseType(StatusCodes.Status200OK)]      
        public async Task<IActionResult> Add([FromBody] MarketRequestModel request)
        {           
            await this.marketsService.CreateMarket(request);
            return Ok();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Change(int id, [FromBody] MarketRequestModel request)
        {
            await this.marketsService.ChangeMarket(id, request);
            return Ok();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(int id)
        {
            await this.marketsService.DeleteMarket(id);
            return Ok();
        }
    }
}