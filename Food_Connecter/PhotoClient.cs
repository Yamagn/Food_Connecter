using System;
using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;

namespace Food_Connecter
{
    public static class PhotoClient
    {
        public static async Task<string> TakePhotoAsync()
        {
            await CrossMedia.Current.Initialize();

            if(!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported) {
                throw new NotSupportedException("You Should set up Camera");
            }

            var photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions());
            if(photo == null)
            {
                return null;
            }
            return photo.Path;
        }
    }
}
