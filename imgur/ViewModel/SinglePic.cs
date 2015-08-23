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
using System.Text.RegularExpressions;

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
              JArray a = (JArray)json["data"];
         
              comments = new ObservableCollection<SingleImageComments>();
              GetSubcomments(a);
              
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

        public List<string> MakeLink(string txt)
        {
            List<string> lista = new List<string>();
            Regex regx = new Regex(@"((http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?)");
            MatchCollection matches = regx.Matches(txt);
          

            foreach (Match match in matches)
            {
                lista.Add(match.Value);
            }

            return lista;
        }

        public void GetSubcomments(JArray x)
        {
         
            for (int i = 0; i < x.Count(); i++)
            {

                var asd = new SingleImageComments(
                    (string)x[i]["id"],
                    (string)x[i]["comment"],
                    (string)x[i]["author"] + " :",
                   UnixToDateTime((string)x[i]["datetime"]),false

                    );

              comments.Add(asd);

              if (x[i]["children"].Count() > 0)
                {

                    JArray y = (JArray)x[i]["children"];
                    GetSubcomments(y);
               }
             

            }
        }

     }
 }

