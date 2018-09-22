using System;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
namespace Food_Connecter
{
    public interface IAuthenticate
    {
        Task<bool> Authenticate(MobileServiceAuthenticationProvider provider);
        Task<bool> ReleaseAuth();
    }
}
