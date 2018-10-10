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

            Stack.IsVisible = true;
            listView.ItemsSource = await App.UketoriDatabase.GetItemsAsync();
            Stack.IsVisible = false;
        }

        async void OnListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            await Navigation.PushAsync(new ChatPage
            {
                BindingContext = ((osusowakeFood)e.SelectedItem).userName
            });
        }
    }
}
