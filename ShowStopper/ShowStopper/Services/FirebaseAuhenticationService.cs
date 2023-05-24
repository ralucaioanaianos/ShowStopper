using Firebase.Auth.Providers;
using Firebase.Auth;
using Microsoft.Maui.ApplicationModel.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowStopper.Services
{
    class FirebaseAuhenticationService
    {
        public static async Task CreateUserFirebase(string webApiKey, string authDomain, string email, string password)
        {
            FirebaseAuthConfig authConfig = new FirebaseAuthConfig
            {
                ApiKey = webApiKey,
                AuthDomain = authDomain,
                Providers = new FirebaseAuthProvider[]
                   {
                        new GoogleProvider().AddScopes("email"),
                        new EmailProvider(),
                   }
            };
            var client = new FirebaseAuthClient(authConfig);
            var auth = await client.CreateUserWithEmailAndPasswordAsync(email, password);
        }
    }
}
