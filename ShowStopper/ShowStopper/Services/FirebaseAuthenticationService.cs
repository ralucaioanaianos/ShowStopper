using Firebase.Auth.Providers;
using Firebase.Auth;
using Microsoft.Maui.ApplicationModel.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Auth.Repository;
using Firebase.Auth.Requests;

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
                    new EmailProvider()
                },
            };

            FirebaseAuthClient client = new FirebaseAuthClient(authConfig);
            await client.CreateUserWithEmailAndPasswordAsync(email, password);
        }

        public static string GetLoggedUserEmail()
        {
            FirebaseAuthConfig authConfig = new FirebaseAuthConfig
            {
                ApiKey = webApiKey,
                AuthDomain = authDomain,
                Providers = new FirebaseAuthProvider[]
                {
                    new GoogleProvider().AddScopes("email"),
                    new EmailProvider()
                },
            };

            FirebaseAuthClient client = new FirebaseAuthClient(authConfig);
            var userEmail = client.User.Info.Email;
            return userEmail;
        }

        public static async Task<User> LoginUserFirebase(string userName, string userPassword, INavigation navigation)
        {
            try
            {
                FirebaseAuthConfig authConfig = new FirebaseAuthConfig
                {
                    ApiKey = webApiKey,
                    AuthDomain = authDomain,
                    Providers = new FirebaseAuthProvider[]
                {
                    new GoogleProvider().AddScopes("email"),
                    new EmailProvider()
                },
                };

                FirebaseAuthClient client = new FirebaseAuthClient(authConfig);
                User user = null;
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
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return null;
            }
            }
            

        public static void LogoutUser()
        {
            FirebaseAuthConfig authConfig = new FirebaseAuthConfig
            {
                ApiKey = webApiKey,
                AuthDomain = authDomain,
                Providers = new FirebaseAuthProvider[]
                {
                    new GoogleProvider().AddScopes("email"),
                    new EmailProvider()
                },
            };

            FirebaseAuthClient client = new FirebaseAuthClient(authConfig);
            client.SignOut();
        }

        public static async Task ResetPassword()
        {
            FirebaseAuthConfig authConfig = new FirebaseAuthConfig
            {
                ApiKey = webApiKey,
                AuthDomain = authDomain,
                Providers = new FirebaseAuthProvider[]
                {
                    new GoogleProvider().AddScopes("email"),
                    new EmailProvider()
                },
            };

            FirebaseAuthClient client = new FirebaseAuthClient(authConfig);
            await client.ResetEmailPasswordAsync(client.User.Info.Email);
        }

        public static async Task ResetPasswordWithEmail(string email)
        {
            FirebaseAuthConfig authConfig = new FirebaseAuthConfig
            {
                ApiKey = webApiKey,
                AuthDomain = authDomain,
                Providers = new FirebaseAuthProvider[]
                {
                    new GoogleProvider().AddScopes("email"),
                    new EmailProvider()
                },
            };

            FirebaseAuthClient client = new FirebaseAuthClient(authConfig);
            await client.ResetEmailPasswordAsync(email);
        }
    }
}
