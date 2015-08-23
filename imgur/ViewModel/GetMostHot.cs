using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using imgur.Model;
using Newtonsoft.Json;
using Windows.UI.Popups;
using System.Net.Http.Headers;
using Windows.Web.Http.Filters;
using Newtonsoft.Json.Linq;
using Windows.UI.Xaml;
using System.Threading;
using Windows.Storage;
using Windows.Storage.Streams;
using System.IO;
using Windows.Web.Http;
using System.Collections.ObjectModel;
using imgur.Model;


namespace imgur.ViewModel
{
    class GetMostHot
    {

        public ObservableCollection<PictureList> lista {get; set;}
        public ObservableCollection<TopModel> toplista { get; set; }
        public ObservableCollection<SearchModel> searchlista { get; set; }
        public ObservableCollection<RandomModel> rlista { get; set; }
        HttpBaseProtocolFilter filter = new HttpBaseProtocolFilter();

        
  
        public async Task getdata(string url, int sivu)
        {

            try
            {
                var filter = new HttpBaseProtocolFilter();
                filter.CacheControl.ReadBehavior = Windows.Web.Http.Filters.HttpCacheReadBehavior.MostRecent;
                filter.CacheControl.WriteBehavior = Windows.Web.Http.Filters.HttpCacheWriteBehavior.NoCache;
                System.Net.Http.HttpClient hc = new System.Net.Http.HttpClient();
             
                string start = "{\"data\":";
                string end = ",\"success\":true,\"status\":200}";
                hc.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Client-ID 9adfc8a3fd65c2b");
                System.Diagnostics.Debug.WriteLine(hc.DefaultRequestHeaders.ToString());
        
                var data = await hc.GetStringAsync(new Uri(url));
                System.Diagnostics.Debug.WriteLine("DATA LADATTIIN WEBISTÄ");

                string x = data.Replace(start, "");
                string y = x.Replace(end, "");
                System.Diagnostics.Debug.WriteLine(y);

                if(sivu == 1)
                {

                        lista = JsonConvert.DeserializeObject<ObservableCollection<PictureList>>(y);
                        FixThumbLinks();
                  
                 }

                if (sivu == 2)
                {
                    toplista = JsonConvert.DeserializeObject<ObservableCollection<TopModel>>(y);

                    for (int i = 0; i < toplista.Count; i++)
                    {

                        toplista[i].thumb = "http://i.imgur.com/" + toplista[i].id + "s.jpg";


                        if (Boolean.Parse(toplista[i].is_album))
                        {
                            toplista[i].thumb = "http://i.imgur.com/" + toplista[i].cover + "s.jpg";

                        }
                    }

                } 
                if (sivu == 3)
                {
                    rlista = JsonConvert.DeserializeObject<ObservableCollection<RandomModel>>(y);

                    for (int i = 0; i < rlista.Count; i++)
                    {

                       rlista[i].thumb = "http://i.imgur.com/" + rlista[i].id + "s.jpg";


                        if (Boolean.Parse(rlista[i].is_album))
                        {
                           rlista[i].thumb = "http://i.imgur.com/" + rlista[i].cover + "s.jpg";

                        }
                    }

                }
                if (sivu == 4)
                {
                    lista = JsonConvert.DeserializeObject<ObservableCollection<PictureList>>(y);
                    for (int i = 0; i < lista.Count; i++)
                    {

                      lista[i].thumb = "http://i.imgur.com/" + lista[i].id + "s.jpg";


                        if (Boolean.Parse(lista[i].is_album))
                        {
                            lista[i].thumb = "http://i.imgur.com/" + lista[i].cover + "s.jpg";

                        }
                    }

                }
    
            }
            catch (System.Net.Http.HttpRequestException e)
            {

                System.Diagnostics.Debug.WriteLine(e.Message);

            }
        }


        // Thumbnailien asetus
        public void FixThumbLinks()
        {

            for (int i = 0; i < lista.Count; i++)
            {

                lista[i].thumb = "http://i.imgur.com/" + lista[i].id + "s.jpg";
           
            
                if (Boolean.Parse(lista[i].is_album))
                {
                    lista[i].thumb = "http://i.imgur.com/" + lista[i].cover + "s.jpg";
                  
                }
            }
        }


    
       
    }//class
}