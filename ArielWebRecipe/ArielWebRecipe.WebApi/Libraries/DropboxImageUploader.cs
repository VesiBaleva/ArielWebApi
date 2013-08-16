using Spring.IO;
using Spring.Social.Dropbox.Api;
using Spring.Social.Dropbox.Connect;
using Spring.Social.OAuth1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

namespace ArielWebRecipe.WebApi.Libraries
{
    public class DropboxImageUploader
    {
        private const string DropboxAppKey = "05yz5fr8rr16fr7";
        private const string DropboxAppSecret = "dkg4rqm9o5sog67";
        private const string DropboxAuthKey = "nl5b0j3q75ts3a66";
        private const string DropboxAuthSecret = "1kfagfbn8huy7qg";
        private const string OAuthTokenFileName = "c:\\OAuthTokenFileName.txt";
        private const string FolderName = "data";

        public static string Upload(string filePath)
        {
            DropboxServiceProvider dropboxServiceProvider =
            new DropboxServiceProvider(DropboxAppKey, DropboxAppSecret, AccessLevel.AppFolder);

            // Login in Dropbox
            IDropbox dropbox = dropboxServiceProvider.GetApi(DropboxAuthKey, DropboxAuthSecret);

            // Display user name (from his profile)
            DropboxProfile profile = dropbox.GetUserProfileAsync().Result;
            Console.WriteLine("Hi " + profile.DisplayName + "!");


            // Upload a file
            Entry uploadFileEntry = dropbox.UploadFileAsync(new FileResource(filePath), "/pics/" + Guid.NewGuid().ToString() + ".jpg").Result;
            Console.WriteLine("Uploaded a file: {0}", uploadFileEntry.Path);

            // Share a file
            DropboxLink sharedUrl = dropbox.GetMediaLinkAsync(uploadFileEntry.Path).Result;

             
            return sharedUrl.Url;
        }

        private static OAuthToken LoadOAuthToken()
        {
            string[] lines = File.ReadAllLines(OAuthTokenFileName);
            OAuthToken oauthAccessToken = new OAuthToken(lines[0], lines[1]);
            return oauthAccessToken;
        }

        private static void AuthorizeAppOAuth(DropboxServiceProvider dropboxServiceProvider)
        {
            // Authorization without callback url
            OAuthToken oauthToken = dropboxServiceProvider.OAuthOperations.FetchRequestTokenAsync(null, null).Result;

            OAuth1Parameters parameters = new OAuth1Parameters();
            string authenticateUrl = dropboxServiceProvider.OAuthOperations.BuildAuthorizeUrl(
                oauthToken.Value, parameters);
            Process.Start(authenticateUrl);

            AuthorizedRequestToken requestToken = new AuthorizedRequestToken(oauthToken, null);
            OAuthToken oauthAccessToken =
                dropboxServiceProvider.OAuthOperations.ExchangeForAccessTokenAsync(requestToken, null).Result;

            string[] oauthData = new string[] { oauthAccessToken.Value, oauthAccessToken.Secret };
            File.WriteAllLines(OAuthTokenFileName, oauthData);
        }
    }
}