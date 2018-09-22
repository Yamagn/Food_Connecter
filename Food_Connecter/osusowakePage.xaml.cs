using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Auth;
using Microsoft.WindowsAzure.MobileServices;

namespace Food_Connecter
{
    public partial class osusowakePage : ContentPage
    {
        // Track whether the user has authenticated.
        bool authenticated = false;

        public osusowakePage()
        {
            InitializeComponent();
        }

        async void loginButton_Clicked(object sender, EventArgs e)
        {
            if (App.Authenticator != null)
                authenticated = await App.Authenticator.Authenticate(MobileServiceAuthenticationProvider.Google);
        }

        async void logoutButton_Clicked(object sender, EventArgs e)
        {
            if (App.Authenticator != null)
            {
                authenticated = false;
                await App.Authenticator.ReleaseAuth();
            }
                
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Refresh items only when authenticated.
            if (authenticated == true)
            {
                // Hide the Sign-in button.
                this.loginButton.IsVisible = false;
            }
        }
    }
}
