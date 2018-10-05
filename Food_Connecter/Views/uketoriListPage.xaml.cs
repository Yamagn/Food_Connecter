using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Food_Connecter
{
    public partial class uketoriListPage : ContentPage
    {
        public uketoriListPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            listView.ItemsSource = await App.uketoriDatabase.GetItemsAsync();
        }
    }
}
