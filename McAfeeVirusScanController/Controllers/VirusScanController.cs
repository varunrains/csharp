using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using McAfeeVirusScanController.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace McAfeeVirusScanController.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class VirusScanController : ControllerBase
    {
       
        private readonly ILogger<VirusScanController> _logger;
        private readonly VirusScanService _virusScanService;

        public VirusScanController(ILogger<VirusScanController> logger, VirusScanService virusScanService)
        {
            _logger = logger;
            _virusScanService = virusScanService;
        }

        [HttpGet]
        public async Task<string> Get()
        {
            return await Task.FromResult("Getting data!");
        }

        [HttpPost]
        public async Task ScanFilesAsync()
        {
            var files = Request.Form?.Files;
            if (files == null || files.All(file => file == null))
            {
                return;
            }

            try
            {
                await _virusScanService.ScanFilesAsync(files).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error scanning files");

            }
        }


    }
}
