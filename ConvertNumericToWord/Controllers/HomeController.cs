using Google.GData.Contacts;
using Google.GData.Client;
using Google.GData.Extensions;
using Google.Contacts;
using System.Diagnostics;
using Google.Apis.Auth.OAuth2;
using System.Threading;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ConvertNumericToWord.Models;

namespace ConvertNumericToWord.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            List<GmailContacts> list = new List<GmailContacts>();
            ViewBag.Message = "Your app description page.";
            list=auth();
            return View(list);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public List<GmailContacts>  auth()
        {

            string clientId = "33812891914-75btc6bs62gfchn5ljs4psnn4ga0phi8.apps.googleusercontent.com";
            string clientSecret = "WNCYctOqv8-AdoqoGnMKurjt";

            List<GmailContacts> list = new List<GmailContacts>();
            string[] scopes = new string[] { "https://www.googleapis.com/auth/contacts.readonly" };     // view your basic profile info.
            try
            {
                // Use the current Google .net client library to get the Oauth2 stuff.
                Google.Apis.Auth.OAuth2.UserCredential credential = GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets { ClientId = clientId, ClientSecret = clientSecret }
                                                                                             , scopes
                                                                                             , "Web client 1"
                                                                                             , System.Threading.CancellationToken.None
                                                                                             , new FileDataStore("Web client 1")).Result;

                // Translate the Oauth permissions to something the old client libray can read
                OAuth2Parameters parameters = new OAuth2Parameters();
                parameters.AccessToken = credential.Token.AccessToken;
                parameters.RefreshToken = credential.Token.RefreshToken;
                list= RunContactsSample(parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return list;
        }

        private List<GmailContacts> RunContactsSample(OAuth2Parameters parameters)
        {
            List<GmailContacts> modellist = new List<GmailContacts>();
            try
            {
               
                
                RequestSettings settings = new RequestSettings("Google contacts tutorial", parameters);
                ContactsRequest cr = new ContactsRequest(settings);
                var f = cr.GetContacts();
                foreach (Contact c in f.Entries)
                {
                    GmailContacts model = new GmailContacts();
                    model.EmailID = c.Name.FullName;
                    //Console.WriteLine();
                    modellist.Add(model);
                }
            }
            catch (Exception a)
            {
                Console.WriteLine("A Google Apps error occurred.");
                Console.WriteLine();
            }
            return modellist;
        }
    }
}
