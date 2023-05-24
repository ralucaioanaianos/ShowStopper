using Firebase.Auth.Providers;
using Firebase.Auth;
using Microsoft.Maui.ApplicationModel.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Auth.Repository;

namespace ShowStopper.Services
{
    class FirebaseAuthenticationService
    {
        public static string webApiKey = "AIzaSyCBEbT1yT0WqRG6Rsts6dYdMz5OQ9dBHVM";

        public static string authDomain = "showstopper-71398.firebaseapp.com";

        public static async Task CreateUserFirebase(string email, string password)
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

        public static async Task<User> LoginUserFirebase(string userName, string userPassword, INavigation navigation)
        {
            User user = null;
            FirebaseAuthConfig authConfig = new FirebaseAuthConfig
            {
                ApiKey = webApiKey,
                AuthDomain = authDomain,
                Providers = new FirebaseAuthProvider[]
                {
                    new GoogleProvider().AddScopes("email"),
                    new EmailProvider()
                },
                UserRepository = new FileUserRepository("FirebaseSample"),

            };
            var client = new FirebaseAuthClient(authConfig);
            var result = await client.FetchSignInMethodsForEmailAsync(userName);
            if (result == null || !result.UserExists)
            {
                await navigation.PushAsync(new LoginPage("True"));

            }
            else
            {
                var userCredential = await client.SignInWithEmailAndPasswordAsync(userName, userPassword);
                user = userCredential.User;
            }
            return user;
        }
    }
}
