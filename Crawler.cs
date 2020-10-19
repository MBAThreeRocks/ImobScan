using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace ImobScan.NetEngine
{
    public class Crawler
    {    
        private static List<string> lstBadProxy = new List<string>();
        private static List<string> lstGoodProxy = new List<string>();

        public static async Task<string> Get(string url)
        {            
            int tentativas = 0;

            while (true)
            {
                if(tentativas > 5)
                    break;

                try
                {
                    using (HttpClient client = new HttpClient())
                        
                    using (HttpResponseMessage res = await client.GetAsync(url))                
                        using (HttpContent content = res.Content)
                        {
                            var byteArray = await content.ReadAsByteArrayAsync();
                            var data = Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);
                            if (data != null)
                            {
                                return(data);
                            }
                            else
                            {
                                return null;
                            }
                        }
                }
                catch
                {
                    tentativas++;
                }
            }
            return null;
        }

        public static string GerProxy(string url)
        {
            var lstProxys = ProxySharp.Proxy.GetProxies(); 

            string proxyS = lstProxys[0];

            Random r = new Random();
            int rInt = 0;

            while(true)
            {
                r = new Random();
                rInt = r.Next(0, lstProxys.Count - 1);
                proxyS = lstProxys[rInt];

                while(lstBadProxy.Contains(proxyS))
                {
                    r = new Random();
                    rInt = r.Next(0, lstProxys.Count - 1);
                    proxyS = lstProxys[rInt];

                    if(lstBadProxy.Count + lstGoodProxy.Count >= lstProxys.Count)
                        return null;                    
                }

                try
                {                                    
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    WebProxy myproxy = new WebProxy(proxyS);
                    myproxy.BypassProxyOnLocal = false;
                    request.Proxy = myproxy;
                    request.Method = "GET";
                    request.Timeout = 30000;
                    HttpWebResponse response = (HttpWebResponse) request.GetResponse();

                    if(!lstGoodProxy.Contains(proxyS))
                        lstGoodProxy.Add(proxyS);

                    if(response.StatusCode == HttpStatusCode.OK)
                    {
                        var encoding = ASCIIEncoding.ASCII;
                        using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
                catch
                {
                    lstBadProxy.Add(proxyS);
                    continue;
                }
            }   

            return null;
        }
    }
}