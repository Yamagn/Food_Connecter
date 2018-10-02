using System;
using System.Net.Http;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Food_Connecter
{
    public partial class FoodItemPage : ContentPage
    {
        public static string Class { get; }
        public FoodItemPage()
        {
            InitializeComponent();
        }

        async void Go_Osusowake(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PostDataPage
            {
                BindingContext = this.BindingContext
            });
        }
    }
}
