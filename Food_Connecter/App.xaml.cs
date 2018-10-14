using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Food_Connecter
{
    public partial class App : Application
    {
        private static FoodItemDatabase foodDatabase;
        public static System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        public static FoodItemDatabase FoodDatabase
        {
            get
            {
                if (foodDatabase == null)
                {
                    foodDatabase = new FoodItemDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FoodSQLite.db3"));
                }
                return foodDatabase;
            }
        }

        public int ResumeAtFoodId { get; set; }

        public static IAuthenticate Authenticator { get; private set; }

        public static void Init(IAuthenticate authenticator)
        {
            Authenticator = authenticator;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
