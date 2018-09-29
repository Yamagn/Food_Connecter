using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.CurrentActivity;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.MobileServices;

namespace Food_Connecter.Droid
{
    [Activity(Label = "Food_Connecter", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IAuthenticate
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            App.Init((IAuthenticate)this);

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            CrossCurrentActivity.Current.Init(this, bundle);
            LoadApplication(new App());
        }

        // Define a authenticated user.
        private MobileServiceUser user;

        MobileServiceUser IAuthenticate.user { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public async Task<bool> Authenticate(MobileServiceAuthenticationProvider provider)
        {
            var success = false;
            var message = string.Empty;
            try{
                user = await osusowakeController.DefaultManager.CurrentClient.LoginAsync(this, provider, "foodconnecter");
                if (user != null)
                {
                    message = string.Format("you are now signed-in as {0}.", user.UserId);
                    success = true;
                }
            }
            catch (Exception e)
            {
                message = e.Message;
            }

            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetMessage(message);
            builder.SetTitle("Sign-in result");
            builder.Create().Show();

            return success;
        }

        public async Task<bool> ReleaseAuth()
        {
            var success = false;
            var message = string.Empty;
            try
            {
                if (user != null)
                {
                    message = string.Format("Logout as {0}.", user.UserId);
                    user = null;
                    success = true;
                }
            }
            catch (Exception e)
            {
                message = e.Message;
            }

            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetMessage(message);
            builder.SetTitle("Sign-in result");
            builder.Create().Show();

            return success;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

