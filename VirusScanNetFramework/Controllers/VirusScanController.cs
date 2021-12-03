using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using VirusScanNetFramework.Services;

namespace VirusScanNetFramework.Controllers
{
    [AllowAnonymous]
    public class VirusScanController : ApiController
    {
       
        [HttpGet]
        public async Task<string> Get()
        {
            return await Task.FromResult("Getting data!");
        }

        [HttpPost]
        public async Task ScanFilesAsync()
        {
           // await Request.Content.LoadIntoBufferAsync();
          //  var bytes = await Request.Content.ReadAsByteArrayAsync();
            //Request.Content.
            var root = @"C:\AntiVirusTest\ScanFolder";
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }
            var provider = new MultipartFormDataStreamProvider(root);



            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);
                foreach (var fileData in provider.FileData)
                {
                    var name = fileData.Headers.ContentDisposition.FileName;
                    name = name.Trim('"');
                    var localFilename = fileData.LocalFileName;
                    var filePath = Path.Combine(root, name);
                    File.Move(localFilename, filePath);
                }

                
                VirusScanService vs = new VirusScanService();
                vs.ScanFilesAsync();
            }
            catch (Exception ex)
            {
                

            }
        }
    }
}
