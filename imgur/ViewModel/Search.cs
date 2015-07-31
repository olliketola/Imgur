using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using imgur.Model;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace imgur.ViewModel
{
    class Search 
    {
        

        public async Task SearchGallery(string query)
        {
            HttpClient hc = new HttpClient();
            string url = "https://api.imgur.com/3/gallery/search/time?q=" + query;
            var message = new HttpRequestMessage(HttpMethod.Get, url);

            message.Headers.Add("User-Agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_8_2) AppleWebKit/537.17 (KHTML, like Gecko) Chrome/24.0.1309.0 Safari/537.17");
            message.Headers.Add("Authorization", "Client-ID 9adfc8a3fd65c2b");
            var response = await hc.SendAsync(message);
            var data = await response.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine(data);



        }

    }
}
