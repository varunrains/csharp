using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GetCryptoHolders.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GetCryptoHolders.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeInflowOutflowController : ControllerBase
    {
        private readonly ExchangeInflowOutflowService _exchangeInflowOutflowService;
        public ExchangeInflowOutflowController(ExchangeInflowOutflowService exchangeInflowOutflowService)
        {
            _exchangeInflowOutflowService = exchangeInflowOutflowService;
        }

        [HttpGet]
        public async Task UpdateInflowOutflow()
        {
            await _exchangeInflowOutflowService.UpdateExchangeInflowOutFlow().ConfigureAwait(false);
        }
    }
}
