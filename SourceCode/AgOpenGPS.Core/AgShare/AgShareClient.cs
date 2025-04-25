using System;
using System.Collections.Generic;
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
        public async Task<List<FieldDownloadDto>> GetOwnFieldsAsync()
        {
            if (string.IsNullOrWhiteSpace(ApiKey)) return new List<FieldDownloadDto>();

            _client.DefaultRequestHeaders.Remove("Authorization");
            _client.DefaultRequestHeaders.Add("Authorization", $"ApiKey {ApiKey}");

            try
            {
                var response = await _client.GetAsync("http://localhost:5000/api/isoxmlfields").ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"GetOwnFields failed: {response.StatusCode}");
                    return new List<FieldDownloadDto>();
                }

                var json = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrWhiteSpace(json))
                    return new List<FieldDownloadDto>();

                var rawFields = JsonSerializer.Deserialize<List<FieldDownloadDto>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (rawFields == null) return new List<FieldDownloadDto>();

                foreach (var field in rawFields)
                {
                    try
                    {
                        field.Boundary = string.IsNullOrWhiteSpace(field.BoundaryJson)
                            ? new List<List<double>>()
                            : JsonSerializer.Deserialize<List<List<double>>>(field.BoundaryJson);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"❌ Boundary parse fout: {ex.Message}");
                        field.Boundary = new List<List<double>>();
                    }

                    try
                    {
                        field.AbLines = string.IsNullOrWhiteSpace(field.ABLinesJson)
                            ? new List<AbLineUploadDto>()
                            : JsonSerializer.Deserialize<List<AbLineUploadDto>>(field.ABLinesJson);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"❌ ABLine parse fout: {ex.Message}");
                        field.AbLines = new List<AbLineUploadDto>();
                    }
                }


                return rawFields;

            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine("Connection error: " + ex.Message);
                return new List<FieldDownloadDto>();
            }
            catch (JsonException ex)
            {
                Debug.WriteLine("JSON parse error: " + ex.Message);
                return new List<FieldDownloadDto>();
            }
        }
    }
}
