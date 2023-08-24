using Firebase.Database;
using ShowStopper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowStopper.Services
{
    class TicketsService
    {
        public string webApiKey = "AIzaSyCBEbT1yT0WqRG6Rsts6dYdMz5OQ9dBHVM";
        private static string databaseUrl = "https://showstopper-71398-default-rtdb.europe-west1.firebasedatabase.app/";
        
        public static async Task AddTicketToDatabase(AppEvent appEvent)
        {
            //try
            //{
            //    FirebaseClient firebaseClient = new FirebaseClient(databaseUrl);
            //    string email = FirebaseAuthenticationService.GetLoggedUserEmail();
            //    //var newEmail = await firebaseClient.Child()
            //}
        }
    }
}
