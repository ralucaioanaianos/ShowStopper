﻿using Firebase.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ShowStopper.Services
{
    class FirebaseStorageService
    {
        private static string storageUrl = "showstopper-71398.appspot.com";

        public static async Task<string> UploadPhotoToStorage(FileResult photo)
        {
            string fileName = Path.GetFileName(photo.FullPath);
            string storagePath = "photos/" + photo.FileName;
            var metadata = new Firebase.Storage.FirebaseMetaData
            {
                ContentType = "image/jpeg",
                FullPath = storagePath,
            };
            var storage = new FirebaseStorage(storageUrl);
                        var photoStream = await photo.OpenReadAsync();
            var photoUrl = await storage.Child(storagePath).PutAsync(photoStream);
            return photoUrl;
        }

        public static async Task<ImageSource> LoadImages()
        {
            var webClient = new WebClient();
            var stroageImage = await new FirebaseStorage(storageUrl)
                .Child("profile_images/")
                .Child("cat.jpg")
                .GetDownloadUrlAsync();
            string imgurl = stroageImage;
            byte[] imgBytes = webClient.DownloadData(imgurl);
            //string img = Convert.ToBase64String(imgBytes);
            var img = ImageSource.FromStream(() => new MemoryStream(imgBytes));
            return img;
        }
    }
}
