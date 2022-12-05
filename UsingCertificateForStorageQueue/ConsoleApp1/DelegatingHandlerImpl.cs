using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class DelegatingHandlerImpl : DelegatingHandler
    {

        private HttpClientHandler _httpClientHandler;
        public DelegatingHandlerImpl() : base()
        {

        }

        public DelegatingHandlerImpl(HttpMessageHandler httpMessageHandler) : base(httpMessageHandler)
        {
          
        }

        public DelegatingHandlerImpl(HttpClientHandler httpClientHandler):base()
        {
           // this.InnerHandler = new HttpClientHandler();
            _httpClientHandler = httpClientHandler;
        }


        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {

            //HttpClientHandler inner = (HttpClientHandler)this.InnerHandler;
            ////base.
            //var inner = _httpClientHandler;
            // this.InnerHandler  = _httpClientHandler;
            request.Headers.Add("TestHeader", "TestValue");
            return base.SendAsync(request, cancellationToken);
        }
    }
}
