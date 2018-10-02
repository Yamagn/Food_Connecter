using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Food_Connecter
{
    public partial class AddEventPage : ContentPage
    {
        public AddEventPage()
        {
            InitializeComponent();
        }

        async void manageButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new eventManagePage());
        }
    }
}
