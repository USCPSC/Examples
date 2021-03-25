using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Net.Http;
using System.Collections.Specialized;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace ConsoleNETFramework
{

    class Program
    {

        static HttpClient client = new HttpClient();
        static void Main(string[] args)
        {
            // 1.Get Access Token
            var clientId = Environment.GetEnvironmentVariable("azure_client_id");
            var tenantIdOrName = Environment.GetEnvironmentVariable("azure_tenant_id"); 
            var clientSecret = Environment.GetEnvironmentVariable("azure_client_secret");//TODO: Need to use certificate in PROD
            var tokenURL = $"https://login.microsoftonline.com/{tenantIdOrName}/oauth2/token?api-version=1.0";
            // Populate the form variable
            var formVariables = new Dictionary<string, string>();
            formVariables.Add("client_id", clientId);
            formVariables.Add("client_secret", clientSecret);
            formVariables.Add("resource", "https://graph.microsoft.com/");
            formVariables.Add("grant_type", "client_credentials");
            var urlFormData = new FormUrlEncodedContent(formVariables);
            urlFormData.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
            client.DefaultRequestHeaders.Clear();
            var response = Task.FromResult<Task<HttpResponseMessage>>(client.PostAsync(tokenURL, urlFormData)).Result;
            if (response.Result.IsSuccessStatusCode)
            {

                dynamic token = JsonConvert.DeserializeObject<dynamic>(response.Result.Content.ReadAsStringAsync().Result);
                Console.WriteLine("Successfully acquired token: " + token);
                Console.ReadKey();

                // 2. With the token make HTTP call
                string tokenData = token.access_token;
                var listDataUrl = "https://graph.microsoft.com/v1.0/sites/cpscgovdc.sharepoint.com,c0cefe40-beeb-41a9-b4f5-9960bcfa010b,fbb78c64-1220-42fe-a319-94c493a9a105/lists/6f53c37b-d6ba-46c3-91a9-2e942a984af9/items";
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenData);
                var data = client.GetAsync(listDataUrl, HttpCompletionOption.ResponseContentRead).Result;
                if (data.IsSuccessStatusCode)
                {
                    Console.WriteLine("Successfully acquired data from Sharepoint list: ");
                    Console.WriteLine(data.Content.ReadAsStringAsync().Result);
                    Console.ReadKey();
                }

            }
            else
            {
                Console.WriteLine("Error acquiring token: " + response.Result.ReasonPhrase);
                Console.WriteLine("Error acquiring token: " + response.Result.Content.ReadAsStringAsync().Result);
                Console.ReadKey();
            }





        }
    }
}
