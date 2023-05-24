using Firebase.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowStopper.Services
{
    class FirebaseStorageService
    {
        private static string storageUrl = "showstopper-71398.appspot.com";

        public static async Task<string> UploadPhotoToStorage(FileResult photo)
        {
            // Upload the photo to Firebase Storage
            string fileName = Path.GetFileName(photo.FullPath);
            string storagePath = "photos/" + fileName;

            var storage = new FirebaseStorage(storageUrl);
            var photoStream = await photo.OpenReadAsync();
            var photoUrl = await storage.Child(storagePath).PutAsync(photoStream);
            return photoUrl;
        }
    }
}
