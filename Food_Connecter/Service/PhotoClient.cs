using System;
using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;


namespace Food_Connecter
{
    public static class PhotoClient
    {
        public static async Task<MediaFile> TakePhotoAsync()
        {
            await CrossMedia.Current.Initialize();
            if(!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported) {
                throw new NotSupportedException("You Should set up Camera");
            }

            var photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                PhotoSize = PhotoSize.Custom,
                CustomPhotoSize = 40,
                CompressionQuality = 50,
                DefaultCamera = CameraDevice.Rear
            });
            if(photo == null)
            {
                return null;
            }
            return photo;
        }
    }
}
