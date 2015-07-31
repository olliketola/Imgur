using imgur.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Diagnostics;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace imgur.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Pictures : Page
    {


        private int get = 0;
       
        GetMostHot data = new GetMostHot();
        Search search = new Search();
      
     

        public Pictures()
        {
            this.InitializeComponent();
            Haku();
            
          
          
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
        }

        public async void Haku()
        {
            
             await data.getdata("https://api.imgur.com/3/gallery/hot/viral/0.json",1);
             photosGrid.ItemsSource = data.lista;

             await data.getdata("https://api.imgur.com/3/gallery/top/top/0.json",2);
             photosGrid2.ItemsSource = data.toplista;

             await data.getdata("https://api.imgur.com/3/gallery/random/random/0.json",3);
             photosGrid3.ItemsSource = data.rlista;
         
        }

        private void photosGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NavigateToSingle(1);

        }
        private void photosGrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        
            NavigateToSingle(2);

        }
        private void photosGrid3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            NavigateToSingle(3);

        }

        private void photosGrid4_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            NavigateToSingle(4);

        }



        private void myPivot_Loaded(object sender, RoutedEventArgs e)
        {
          

        }

        public void NavigateToSingle(int page)
        {

            Application.Current.Resources.Clear();
            get = 0;


            if (page == 1)
            {

                if (photosGrid.SelectedIndex == -1)
                    return; 
               
                 get = photosGrid.SelectedIndex;
                 List<string> lista = new List<string>();
                 lista.Add(data.lista.ElementAt(get).link);
                 lista.Add(data.lista.ElementAt(get).mp4);
                 lista.Add(data.lista.ElementAt(get).title);
                 lista.Add(data.lista.ElementAt(get).thumb);
                 lista.Add(data.lista.ElementAt(get).id);
                 lista.Add(data.lista.ElementAt(get).is_album);
                 lista.Add(data.lista.ElementAt(get).cover);
                 lista.Add(data.lista.ElementAt(get).description);
                 lista.Add(data.lista.ElementAt(get).datetime);
               
                 Application.Current.Resources.Add("SinglePicInfo", lista);
                 Frame.Navigate(typeof(Single));
        
            }
            if (page == 2)
            {

                if (photosGrid2.SelectedIndex == -1)
                    return;  
                get = photosGrid2.SelectedIndex;
                List<string> lista = new List<string>();
                lista.Add(data.toplista.ElementAt(get).link);
                lista.Add(data.toplista.ElementAt(get).mp4);
                lista.Add(data.toplista.ElementAt(get).title);
                lista.Add(data.toplista.ElementAt(get).thumb);
                lista.Add(data.toplista.ElementAt(get).id);
                lista.Add(data.toplista.ElementAt(get).is_album);
                lista.Add(data.toplista.ElementAt(get).cover);
                lista.Add(data.toplista.ElementAt(get).description);
                lista.Add(data.toplista.ElementAt(get).datetime);
               
                Application.Current.Resources.Add("SinglePicInfo", lista);
                Frame.Navigate(typeof(Single));
        
            }
            if (page == 3)
            {

                if (photosGrid3.SelectedIndex == -1)
                    return;  
                get = photosGrid3.SelectedIndex;
                List<string> lista = new List<string>();
                lista.Add(data.rlista.ElementAt(get).link);
                lista.Add(data.rlista.ElementAt(get).mp4);
                lista.Add(data.rlista.ElementAt(get).title);
                lista.Add(data.rlista.ElementAt(get).thumb);
                lista.Add(data.rlista.ElementAt(get).id);
                lista.Add(data.rlista.ElementAt(get).is_album);
                lista.Add(data.rlista.ElementAt(get).cover);
                lista.Add(data.rlista.ElementAt(get).description);
                lista.Add(data.rlista.ElementAt(get).datetime);
             
                Application.Current.Resources.Add("SinglePicInfo", lista);
                Frame.Navigate(typeof(Single));

            }

            if (page == 4)
            {

                if (photosGrid4.SelectedIndex == -1)
                    return;
                get = photosGrid4.SelectedIndex;
                List<string> lista = new List<string>();
                lista.Add(data.lista.ElementAt(get).link);
                lista.Add(data.lista.ElementAt(get).mp4);
                lista.Add(data.lista.ElementAt(get).title);
                lista.Add(data.lista.ElementAt(get).thumb);
                lista.Add(data.lista.ElementAt(get).id);
                lista.Add(data.lista.ElementAt(get).is_album);
                lista.Add(data.lista.ElementAt(get).cover);
                lista.Add(data.lista.ElementAt(get).description);
                lista.Add(data.lista.ElementAt(get).datetime);

                Application.Current.Resources.Add("SinglePicInfo", lista);
                Frame.Navigate(typeof(Single));

            }

         
        
        }

        private void pivot_item2_Loaded(object sender, RoutedEventArgs e)
        {
       
        }

        private async void b1_Click(object sender, RoutedEventArgs e)
        {


          Random r = new Random();

          await data.getdata("https://api.imgur.com/3/gallery/random/random/"+r.Next(0,50)+".json", 3);
          photosGrid3.ItemsSource = data.rlista;
           

        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            photosGrid.SelectedIndex = -1;
            photosGrid2.SelectedIndex = -1;
            photosGrid3.SelectedIndex = -1;
            photosGrid4.SelectedIndex = -1;
        }


        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            
            if (tb1.Text != null)  
            {
                string url = "https://api.imgur.com/3/gallery/search/time?q="+tb1.Text;
                await data.getdata(url, 4);
                photosGrid4.ItemsSource = data.lista;
            }
        }

     
        }

}
