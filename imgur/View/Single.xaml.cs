using imgur.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using imgur.ViewModel;
using imgur.Model;
using Windows.UI.Xaml.Media.Imaging;
using System.Diagnostics;
using Windows.Storage;
using System.Net.Http;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using System.Text.RegularExpressions;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace imgur.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Single : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        SinglePic s = new SinglePic();
        private const string UrlPattern = @"(http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?";


        private int val = 0;
       
       public Single()
       {
           this.InitializeComponent();
           this.navigationHelper = new NavigationHelper(this);
           this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
           this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
      
       }


       public async void getComments()
       {
           await s.GetComments();
           comments.ItemsSource = s.comments;

        

       }
       public async void getAlbum()
       {
           await s.GetAlbumInfo();
        
       }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// 
      

        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {

          
             
            if (Boolean.Parse(s.is_album))
            {
               getAlbum();
               root.DataContext = s;
               album.ItemsSource = s.album;
                
            }
            else
            {
                s.SetPic();

                if (s.mp4 != null)
                {

                    kuva.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    media.Visibility = Windows.UI.Xaml.Visibility.Visible;

                }
                else
                {
                    kuva.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    media.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

                }
                root.DataContext = s;
             
            }
            getComments();
           
        }

      
        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StorageFolder picsFolder = KnownFolders.SavedPictures;
                StorageFolder vidsFolder = KnownFolders.VideosLibrary;
                HttpClient client = new HttpClient();

                string url = "";
                StorageFile file = null;
                if (s.mp4 == null)
                {
                    if (Boolean.Parse(s.is_album))
                    {
                         
                         url = s.album.ElementAt(val).link;
                    }
                    else
                    {
                        url = s.kuva;
                    }

                    file = await picsFolder.CreateFileAsync("myImage.jpg", CreationCollisionOption.GenerateUniqueName);
  
                }
                else
                {
                    url = s.mp4;
                    file = await vidsFolder.CreateFileAsync("myVideo.mp4", CreationCollisionOption.GenerateUniqueName);
                }

                var stream = await file.OpenAsync(FileAccessMode.ReadWrite);
                byte[] responseBytes = await client.GetByteArrayAsync(url);
                using (var outputStream = stream.GetOutputStreamAt(0))
                {
                    DataWriter writer = new DataWriter(outputStream);
                   writer.WriteBytes(responseBytes);
                   await writer.StoreAsync();
                   await outputStream.FlushAsync();
                }
                Msg("Tallenus onnistui");
            }
            catch (Exception err)
            {
                Debug.WriteLine(err.Message);
                Msg("Virhe"+err.Message);
            }
              
 
            }

        private void StackPanel_Holding(object sender, HoldingRoutedEventArgs e)
        {
            FrameworkElement senderElement = sender as FrameworkElement;
            FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);
            flyoutBase.ShowAt(senderElement);

        }

        private async void Msg(string viesti)
        {
            MessageDialog msg = new MessageDialog(viesti);
            await msg.ShowAsync();
        }

        private void root_Holding(object sender, HoldingRoutedEventArgs e)
        {
            FrameworkElement senderElement = sender as FrameworkElement;
            FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);
            flyoutBase.ShowAt(senderElement);
        }

        private void album_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            val = album.SelectedIndex;
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void comments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

   

      

    }

}
