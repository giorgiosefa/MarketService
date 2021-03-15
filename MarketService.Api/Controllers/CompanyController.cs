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
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService companyService;

        public CompanyController(ICompanyService companyService)
        {
            this.companyService = companyService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(CompanyModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var markets = await this.companyService.GetCompanies();
            return Ok(markets);
        }

        [HttpGet("{marketId}")]
        [ProducesResponseType(typeof(CompanyModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int marketId)
        {
            var market = await this.companyService.GetCompanyByMarketId(marketId);
            return Ok(market);
        }

        [HttpPost()]
        [Route("Add")]
        [ProducesResponseType(StatusCodes.Status200OK)]      
        public async Task<IActionResult> Add([FromBody] CompanyRequestModel request)
        {           
            await this.companyService.CreateCompany(request);
            return Ok();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Change(int id, [FromBody] CompanyRequestModel request)
        {
            await this.companyService.ChangeCompany(id, request);
            return Ok();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(int id)
        {
            await this.companyService.DeleteCompany(id);
            return Ok();
        }
    }
}