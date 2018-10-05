﻿using System;
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
        class ImageList
        {
            [JsonProperty("photos")]
            public List<string> Photos = null;
        }

        public osusowakePage()
        {
            InitializeComponent();
        }
        

        async void LoadBitmapCollection()
        {
            int imageDimension = Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android ? 80 : 40;
            string urlSuffix = String.Format("?width={0}&height{0}&mode=max", imageDimension);

            using (WebClient webClient = new WebClient())
            {
                try
                {
                    Uri uri = new Uri(Constants.ApplicationURL + "/api/view?id=" + App.Authenticator.user.UserId + "&city=" + kanriPage.userInfo.City);
                    //Uri uri = new Uri("http://docs.xamarin.com/demo/stock.json");
                    var stream = App.client.GetStringAsync(uri).Result;
                    Console.WriteLine(stream);

                    var js = JsonConvert.DeserializeObject<List<osusowakeFood>>(stream);

                    foreach (var filepath in js)
                    {
                        var tgr = new TapGestureRecognizer();
                        tgr.Tapped += async (sender, e) =>
                        {
                            await Navigation.PushAsync(new OsusowakeDetailPage
                            {
                                BindingContext = filepath
                            });
                        };
                        Image image = new Image();
                        image.Source = ImageSource.FromUri(new Uri(filepath.imageUrl));
                        image.HeightRequest = imageDimension;
                        image.WidthRequest = imageDimension;
                        image.GestureRecognizers.Add(tgr);
                        flexLayout.Children.Add(image);
                    }

                    //var js = JsonConvert.DeserializeObject<ImageList>(stream);

                    //foreach (var filepath in js.Photos)
                    //{
                    //    var tgr = new TapGestureRecognizer();
                    //    tgr.Tapped += async (sender, e) =>
                    //    {
                    //        await Navigation.PushAsync(new OsusowakeDetailPage
                    //        {
                    //            //BindingContext = filepath
                    //        });
                    //    };
                    //    Image image = new Image();
                    //    image.Source = ImageSource.FromUri(new Uri(filepath));
                    //    image.GestureRecognizers.Add(tgr);
                    //    flexLayout.Children.Add(image);
                    //}
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
            else
            {
                flexLayout.Children.Clear();
                LoadBitmapCollection();
            }
        }

        async void manageButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new uketoriListPage());
        }
    }
}
