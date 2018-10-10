using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;

using Foundation;
using UIKit;

namespace Food_Connecter
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IAuthenticate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //

        // Define a authenticated user.
        public MobileServiceUser user { get; set; }

        public async Task<bool> Authenticate(MobileServiceAuthenticationProvider provider)
        {
            var success = false;
            var message = string.Empty;
            try
            {
                if (user == null)
                {
                    user = await osusowakeController.DefaultManager.CurrentClient.LoginAsync(UIApplication.SharedApplication.KeyWindow.RootViewController, provider, "foodconnecter");
                    if (user != null)
                    {
                        user.UserId = user.UserId.Remove(0, 4);
                        message = string.Format("You are now signed-in as {0}.", user.UserId);
                        success = true;

                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                // Display the success or failure message.
                UIAlertView avAlert = new UIAlertView("Sign-in result", message, null, "OK", null);
                avAlert.Show();
            }

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
            catch(Exception e)
            {
                message = e.Message;
            }
            // Display the success or failure message.
            UIAlertView avAlert = new UIAlertView("Logout result", message, null, "OK", null);
            avAlert.Show();

            return success;
        }

        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            return osusowakeController.DefaultManager.CurrentClient.ResumeWithURL(url);
        }

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            App.Init(this);
            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}
