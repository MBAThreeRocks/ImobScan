using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ImobScan.NetEngine
{
    public class Crawler
    {    
        public static async Task<string> Get(string url)
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
    }
}