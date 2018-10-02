using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using System.Net;
using System.Runtime.Serialization.Json;
using System.IO;

namespace Food_Connecter
{
    public partial class osusowakePage : ContentPage
    {
        [DataContract]
        class ImageList
        {
            [DataMember(Name = "photos")]
            public List<string> Photos = null;
        }

        public osusowakePage()
        {
            InitializeComponent();

            LoadBitmapCollection();
        }

        async void LoadBitmapCollection()
        {
            int imageDimension = Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android ? 240 : 120;
            string urlSuffix = String.Format("?width={0}&height{0}&mode=max", imageDimension);

            using (WebClient webClient = new WebClient())
            {
                try
                {
                    Uri uri = new Uri("http://docs.xamarin.com/demo/stock.json");
                    byte[] data = await webClient.DownloadDataTaskAsync(uri);

                    using (Stream stream = new MemoryStream(data))
                    {
                        var jsonSerealizer = new DataContractJsonSerializer(typeof(ImageList));
                        ImageList imageList = (ImageList)jsonSerealizer.ReadObject(stream);

                        foreach(string filepath in imageList.Photos)
                        {
                            var tgr = new TapGestureRecognizer();
                            tgr.Tapped += (sender, e) => OnImageClicked();
                            Image image = new Image();
                            image.Source = ImageSource.FromUri(new Uri(filepath + urlSuffix));
                            image.GestureRecognizers.Add(tgr);
                            flexLayout.Children.Add(image);
                        }
                    }
                }
                catch
                {
                    flexLayout.Children.Add(new Label
                    {
                        Text = "Cannot Access list of bitmap files"
                    });
                }
            }
            activity_indicator.IsRunning = false;
            activity_indicator.IsVisible = false;
        }

        async void OnImageClicked()
        {
            await Navigation.PushAsync(new OsusowakeDetailPage());
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (App.Authenticator.user == null)
            {
                var res = await DisplayAlert("ログインしてください", "", "ログインする", "閉じる");
                if (res)
                {
                    kanriPage.authenticated = await App.Authenticator.Authenticate(MobileServiceAuthenticationProvider.Google);
                }
                else
                {
                    flexLayout.Children.Add(new Label
                    {
                        Text = "ログインしてください"
                    });

                    return;
                }
            }
        }
    }
}
