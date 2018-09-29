using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
namespace Food_Connecter
{
    public class osusowakeController
    {
        static osusowakeController defaultInstance = new osusowakeController();
        public static MobileServiceClient client = new MobileServiceClient(Constants.ApplicationURL);

        public static osusowakeController DefaultManager
        {
            get
            {
                return defaultInstance;
            }

            private set
            {
                defaultInstance = value;
            }
        }

        public MobileServiceClient CurrentClient
        {
            get { return client; }
        }
    }
}
