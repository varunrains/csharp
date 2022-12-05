using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.IdentityModel;

namespace ocelot_gateway.Handlers
{
    public class AuthenticationHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                var r = request;
                var token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6IjJaUXBKM1VwYmpBWVhZR2FYRUpsOGxWMFRPSSIsImtpZCI6IjJaUXBKM1VwYmpBWVhZR2FYRUpsOGxWMFRPSSJ9.eyJhdWQiOiJhcGk6Ly9lbGltc2JwdGNvcmVzZXJ2aWNlcyIsImlzcyI6Imh0dHBzOi8vc3RzLndpbmRvd3MubmV0L2UzYTRlODc0LWQ1ZWEtNDI5My1iOTU5LWQ2MzBmZmFiODkxYy8iLCJpYXQiOjE2NTYzOTY1NDEsIm5iZiI6MTY1NjM5NjU0MSwiZXhwIjoxNjU2NDAwNDQxLCJhaW8iOiJFMlpnWUdpN3ZFN2t1dVRPNXVzUlo4NVZTOTJPQUFBPSIsImFwcGlkIjoiODIxYWJhYjYtM2M0YS00NDc0LTlmMTAtOTE4ZmY2MjYwNWQ0IiwiYXBwaWRhY3IiOiIxIiwiaWRwIjoiaHR0cHM6Ly9zdHMud2luZG93cy5uZXQvZTNhNGU4NzQtZDVlYS00MjkzLWI5NTktZDYzMGZmYWI4OTFjLyIsIm9pZCI6IjdmZWYyYjA0LTRlZTctNDJlZi05OTg2LWFkODgwNDk2M2Q0MiIsInJoIjoiMC5BVGNBZE9pazQtclZrMEs1V2RZd182dUpITGE2R29KS1BIUkVueENSal9ZbUJkUTNBQUEuIiwic3ViIjoiN2ZlZjJiMDQtNGVlNy00MmVmLTk5ODYtYWQ4ODA0OTYzZDQyIiwidGlkIjoiZTNhNGU4NzQtZDVlYS00MjkzLWI5NTktZDYzMGZmYWI4OTFjIiwidXRpIjoiTWlfN0FvcTE2a3E0RXZRdHhLZlFBQSIsInZlciI6IjEuMCJ9.Li4mQ_lAHDCXuSxEBzuhzY0OKPSNDOuO57RzWOhwgwdJCTlDfX5ZBQe-Hg_v2RcXlcwUJ8yclcr6WiRDSv114H8vw_-GHvgv4-lNgna_t0hvbu_sWxRWLebQP4iKDkT4CDfVAp4Q11yUX4IXIOB90H994n0iEflPkFzrh9AdPVNluxbGc2SPQGRpxC0jy4W_0xFtLaGXUKKb_-JhPjXuQmzcQ_w5tib252qSwi7fEyNtiD2wXWlgyt73zjZThkoW9ZwpnIrgH3GvBYjVI4j8lOb9TNuvzJqqxtzQfIc6mTN_khTgNzKYdNo9qhdYcS9_JLEN_AMwe9gvum1TfGwyzA";

                request.Headers.Remove("Authorization");
                request.Headers.Add("Authorization", $"Bearer {token}");
                request.Headers.Add("X-User-Puid", "WCE4");
                //do stuff and optionally call the base handler..
               
            }catch(Exception e)
            {
                Console.WriteLine("");
            }
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
