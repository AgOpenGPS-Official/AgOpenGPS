using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AgOpenGPS.Core.AgShare
{
    public class AgShareClient
    {
        private readonly HttpClient _client = new HttpClient();

        public string ApiKey { get; set; }

        public async Task<bool> TestApiKeyAsync()
        {
            if (string.IsNullOrWhiteSpace(ApiKey)) return false;

            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5000/api/isoxmlfields");

            _client.DefaultRequestHeaders.Remove("Authorization");
            _client.DefaultRequestHeaders.Add("Authorization", $"ApiKey {ApiKey}");

            try
            {
                var response = await _client.SendAsync(request).ConfigureAwait(false);
                Debug.WriteLine("Test status: " + response.StatusCode);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("API key test exception: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> UploadIsoXmlFieldAsync(string fieldId, object jsonPayload)
        {
            if (string.IsNullOrWhiteSpace(ApiKey)) return false;

            _client.DefaultRequestHeaders.Remove("Authorization");
            _client.DefaultRequestHeaders.Add("Authorization", $"ApiKey {ApiKey}");

            var content = new StringContent(JsonSerializer.Serialize(jsonPayload), Encoding.UTF8, "application/json");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            try
            {
                var response = await _client.PutAsync($"http://localhost:5000/api/isoxmlfields/{fieldId}", content).ConfigureAwait(false);
                string responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Upload exception: " + ex.Message);
                return false;
            }
        }
    }
}
