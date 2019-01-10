using Bot_Builder_Echo_Bot_V4.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Bot_Builder_Echo_Bot_V4.Helpers
{
    public class TranslatorHelper
    {
        public static async Task<string> GetDesiredLanguageAsync(string content)
        {
            string requestBody = string.Format("[{{ \"Text\": \"{0}\" }}]", content);

            var client = new RestClient("https://api.cognitive.microsofttranslator.com/detect?api-version=3.0");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Postman-Token", "160eb738-664f-4a6a-92ec-0b233a16948c");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Ocp-Apim-Subscription-Key", "eeca801f6c6e4f2eaf5d3204f029f2b5");
            request.AddParameter("undefined", requestBody, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            string createdBody = "{ \"result\": " + response.Content + "} ";
            var detectedLanguage = JsonConvert.DeserializeObject<DetectedLang>(createdBody).result[0].language;

            return detectedLanguage;
        }

        public static async Task<string> TranslateSentenceAsync(string originalSentence)
        {
            string decodedString = WebUtility.HtmlDecode(originalSentence);
            Object[] body = new Object[] { new { Text = decodedString } };
            var requestBody = JsonConvert.SerializeObject(body);
            string convertedAnswer = string.Empty;
            //string languageCode = ConfigurationManager.AppSettings["DesiredLanguage"].ToString();

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(string.Format("https://api.cognitive.microsofttranslator.com/translate?api-version=3.0&to={0}", "en"));
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add("Ocp-Apim-Subscription-Key", "eeca801f6c6e4f2eaf5d3204f029f2b5");

                var response = await client.SendAsync(request);
                var responseBody = await response.Content.ReadAsStringAsync();
                convertedAnswer = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(responseBody), Formatting.Indented);
            }

            string createdBody = "{ \"documents\": [ { \"result\": " + convertedAnswer + "} ] }";
            //var message = JsonConvert.DeserializeObject<TranslationModel>(createdBody).documents[0].result[0].translations[0].text;

            return null;
        }
    }
}
