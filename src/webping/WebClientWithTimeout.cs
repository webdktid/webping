using System;
using System.Net;

namespace webping
{
    public class WebClientWithTimeout : WebClient
    {


        protected  override WebRequest GetWebRequest(Uri address)
        {
            var req = base.GetWebRequest(address);
            req.Timeout = 120000;
            return req;
        }
    }
}
