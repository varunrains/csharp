using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace VirusScanNetFramework.Controllers
{
    public class TestController : ApiController
    {
        [HttpGet]
        public async Task<string> Get()
        {
            return await Task.FromResult("Getting data!");
        }
    }
}
