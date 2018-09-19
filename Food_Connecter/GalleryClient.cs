using System;
using System.Threading.Tasks;
using Plugin.Media;

namespace Food_Connecter
{
    public static class GalleryClient
    {
        public static async Task<string> PickPhotoAsync()
        {
            var photo = await CrossMedia.Current.PickPhotoAsync();
            return photo.Path;
        }
    }
}
