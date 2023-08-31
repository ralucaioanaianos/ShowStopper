using Firebase.Database;
using Firebase.Database.Query;
using ShowStopper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowStopper.Services
{
    class FeedbacksService
    {
        public string webApiKey = "AIzaSyCBEbT1yT0WqRG6Rsts6dYdMz5OQ9dBHVM";
        private static string databaseUrl = "https://showstopper-71398-default-rtdb.europe-west1.firebasedatabase.app/";

        public static async Task AddFeedbackMessageToDatabase(string userEmail, string message)
        {
            FirebaseClient client = new FirebaseClient(databaseUrl);
            await client.Child("Feedbacks").PostAsync(new FeedbackMessage
            {
                UserEmail = userEmail,
                Message = message
            });
        }
    }
}
