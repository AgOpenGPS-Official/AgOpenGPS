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
        private bool _configured;

        public void SetServer(string server)
        {
            _client.BaseAddress = new Uri(server, UriKind.Absolute);
        }

        public void SetApiKey(string apiKey)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                _client.DefaultRequestHeaders.Authorization = null;
                _configured = false;
            }
            else
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("ApiKey", apiKey);
                _configured = true;
            }
        }

        public async Task<(bool Success, string Message)> TestConnectionAsync(string server, string apiKey)
        {
            if (!Uri.TryCreate(server, UriKind.Absolute, out Uri baseAddress))
                return (false, "Invalid server address");

            if (string.IsNullOrWhiteSpace(apiKey))
                return (false, "No API key");

            var request = new HttpRequestMessage(HttpMethod.Get, new Uri(baseAddress, "/api/isoxmlfields"));
            request.Headers.Authorization = new AuthenticationHeaderValue("ApiKey", apiKey);

            try
            {
                var response = await _client.SendAsync(request).ConfigureAwait(false);
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return (response.IsSuccessStatusCode, content);
            }
            catch (HttpRequestException)
            {
                return (false, "Failed to connect to server");
            }
        }

        public async Task<bool> UploadIsoXmlFieldAsync(string fieldId, object jsonPayload)
        {
            if (!_configured)
                return false;

            var content = new StringContent(JsonSerializer.Serialize(jsonPayload), Encoding.UTF8, "application/json");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            try
            {
                var response = await _client.PutAsync($"/api/isoxmlfields/{fieldId}", content).ConfigureAwait(false);
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
