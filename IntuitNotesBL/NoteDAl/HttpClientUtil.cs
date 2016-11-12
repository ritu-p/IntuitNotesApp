using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace IntuitNotesBL.NoteDAl
{
    internal class HttpClientUtil
    {
        public static async Task<HttpUtilityOutput> PostHttpAsync(Uri url, string jsonContent)
        {
            var _httpClient = new HttpClient();
            var returnValue = new HttpUtilityOutput();
            try
            {
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var Value = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var httpResponseMessage = await _httpClient.PostAsync(url, Value);
                var response = await httpResponseMessage.Content.ReadAsStringAsync();
                returnValue.Content = response;
                returnValue.Status = httpResponseMessage.StatusCode;
            }
            catch
            {
                returnValue.Status = HttpStatusCode.InternalServerError;
            }

            return returnValue;
        }
    }
}