using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Food_Connecter
{
    public partial class FoodItemPage : ContentPage
    {
        public FoodItemPage()
        {
            InitializeComponent();
        }

        async void OnSaveClicked(object sender, EventArgs e)
        {
            var foodItem = (FoodItem)BindingContext;
            await App.Database.SaveItemAsync(foodItem);
            await Navigation.PopAsync();
        }

        async void OnDeleteClicked(object sender, EventArgs e)
        {
            var foodItem = (FoodItem)BindingContext;
            await App.Database.DeleteItemAsync(foodItem);
            await Navigation.PopAsync();
        }

        async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
