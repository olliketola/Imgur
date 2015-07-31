using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using imgur.Model;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using Windows.Web.Http.Filters;
using Newtonsoft.Json.Linq;
using Windows.UI.Popups;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;

namespace imgur.ViewModel
{
    class SinglePic 
    {


        public string kuva{get; set;}
        public string id{ get; set; }
        public string mp4 { get; set; }
        public string title { get; set; }
        public string is_album { get; set; }
        public string description{ get; set; }
        public string datetime{ get; set; }
        public string author { get; set; }
      
        public List<string> lista;
        public ObservableCollection<AlbumModel> album = new ObservableCollection<AlbumModel>();
        public ObservableCollection<SingleImageComments> comments;

        private string url = "https://api.imgur.com/3/gallery/album/";
        HttpBaseProtocolFilter filter = new HttpBaseProtocolFilter();
         
    

        //nämä ajetaan aina kun luodaan uusi ilmentymä tästä luokasta!
        public SinglePic()
        {
              lista = (List<string>)Application.Current.Resources["SinglePicInfo"];
              title = lista[2].ToString();
              is_album = lista[5].ToString();
              id = lista[4].ToString();
              if (lista[7] != null)
              {
                  description = lista[7].ToString();
              }
              datetime = "Added : "+UnixToDateTime(lista[8].ToString());
     
        }
     
        //jos valittu thumbnail sisältää albumin
        public async Task GetAlbumInfo()
        {
      

            if (Boolean.Parse(lista[5]))
            {

                try
                {

                    Windows.Web.Http.HttpClient hc = new Windows.Web.Http.HttpClient();
                    hc.DefaultRequestHeaders.Authorization = new Windows.Web.Http.Headers.HttpCredentialsHeaderValue("Authorization", "9adfc8a3fd65c2b");
                    var data = await hc.GetStringAsync(new Uri(url + lista[4].ToString()));
                    System.Diagnostics.Debug.WriteLine("ALBUMIN DATAN LATAAMINEN ONNISTUI" + data);

                    JObject json = JObject.Parse(data);

                  
                        for (int i = 0; i < json["data"]["images"].Count(); i++)
                        {
                            var item = new AlbumModel();

                            if (Boolean.Parse((string)json["data"]["images"][i]["animated"]))
                            {
                                 item = new AlbumModel
                                {

                                    title = (string)json["data"]["title"],
                                    description = (string)json["data"]["images"][i]["description"],
                                    id = (string)json["data"]["images"][i]["id"],
                                    mp4 = (string)json["data"]["images"][i]["mp4"]

                                };

                            }
                            else
                            {
                                item = new AlbumModel
                                {
                                    link = (string)json["data"]["images"][i]["link"],
                                    title = (string)json["data"]["title"],
                                    id = (string)json["data"]["images"][i]["id"],
                                    description = (string)json["data"]["images"][i]["description"]
                                   
                                };
                            }
                            album.Add(item);
                    }
                }

                catch (Exception e)
                {
                    ShowMessage("Virhe albumi datan lataamisessa");
                }

               
            }
        }
       
        public void SetPic()
        {
         

            if (Boolean.Parse(is_album) == false)
            {
                kuva = lista[0].ToString();

                if (lista[1] != null)
                {
                    mp4 = lista[1].ToString();
                }
            }
          
        }


        //kuvien ja albumien kommenttien lataaminen
        public async Task GetComments()
        {

            try
            {
                string url = "";
                if (Boolean.Parse(is_album) == false)
                {
                    url = "https://api.imgur.com/3/gallery/image/"+id+ "/comments/top";
                }
                else 
                {
                    url = "https://api.imgur.com/3/gallery/album/"+id+ "/comments/top";
                }

           /*   Windows.Web.Http.HttpClient hc = new Windows.Web.Http.HttpClient();
              hc.DefaultRequestHeaders.Authorization = new Windows.Web.Http.Headers.HttpCredentialsHeaderValue("Authorization", "c41e74ed5c02726");
              var data = await hc.GetStringAsync(new Uri(url));
            */

              var hc = new HttpClient();
              var message = new HttpRequestMessage(HttpMethod.Get, url);
               
              message.Headers.Add("User-Agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_8_2) AppleWebKit/537.17 (KHTML, like Gecko) Chrome/24.0.1309.0 Safari/537.17");
              message.Headers.Add("Authorization", "Client-ID 9adfc8a3fd65c2b");
              var response = await hc.SendAsync(message);
              var data = await response.Content.ReadAsStringAsync();
              System.Diagnostics.Debug.WriteLine(data);
             
              JObject json = JObject.Parse(data);
              comments = new ObservableCollection<SingleImageComments>();

             int count = 19;
                if (json["data"].Count() < count)
                {
                    count = json["data"].Count();
               }
            
                //Haetaan 20 parhaimmaksi valittua kommenttia
                for (int i = 0; i < json["data"].Count(); i++)
                {

                    var asd = new SingleImageComments(
                        (string)json["data"][i]["comment"],
                        (string)json["data"][i]["author"] + " :",
                       UnixToDateTime((string)json["data"][i]["datetime"])
                        );

                    comments.Add(asd);

                }
            }
            catch(Exception e)
            {
                ShowMessage("Virhe kommenttien lataamisessa"+e.Message);
            }

              
            }
        private async void ShowMessage(string error)
        {
            // using Windows.UI.Popups;
            MessageDialog messageDialog = new MessageDialog(error);
            await messageDialog.ShowAsync();
        }

        private string UnixToDateTime(string time)
        {
           var datetime = new DateTime(1970, 1, 1, 0, 0, 0);
           var date = datetime.AddSeconds(Double.Parse(time));
           return date.ToString();

        }
     }
 }

