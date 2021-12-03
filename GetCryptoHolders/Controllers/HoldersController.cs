using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GetCryptoHolders.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GetCryptoHolders.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoldersController : ControllerBase
    {
        private readonly CryptoHolderService _cryptoHolderService;
        public HoldersController(CryptoHolderService service)
        {
            _cryptoHolderService = service;
        }

        // GET: api/<HoldersController>
        [HttpGet]
        public async Task<string> Get()
        {

           await _cryptoHolderService.GetHolders().ConfigureAwait(false);

            return "done";



        }

        
    }
}
