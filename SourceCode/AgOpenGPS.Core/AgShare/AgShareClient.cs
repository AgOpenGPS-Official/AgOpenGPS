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

            var request = new HttpRequestMessage(HttpMethod.Get, new Uri(baseAddress, "/api/fields"));
            request.Headers.Authorization = new AuthenticationHeaderValue("ApiKey", apiKey);

            try
            {
                var response = await _client.SendAsync(request).ConfigureAwait(false);

                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    return (false, $"API Key error: {response.StatusCode}, {errorContent}");
                }
                return (response.IsSuccessStatusCode, content);
            }
            catch (HttpRequestException ex)
            {
                // Log de fout indien de request faalt
                return (false, $"Failed to connect to server: {ex.Message}");
            }
        }

        public async Task<bool> UploadAsync(string fieldId, object jsonPayload)
        {
            if (!_configured)
                return false;

            // Serialiseer het jsonPayload object naar JSON
            var content = new StringContent(JsonSerializer.Serialize(jsonPayload), Encoding.UTF8, "application/json");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            try
            {
                // Verstuur de PUT request naar de server
                var response = await _client.PutAsync($"/api/fields/{fieldId}", content).ConfigureAwait(false);

                // Log de response statuscode voor debugging
                Debug.WriteLine("Response Status Code: " + response.StatusCode);

                // Lees de response body als tekst voor debugging
                string responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                Debug.WriteLine("Response Body: " + responseBody);

                // Als de response succesvol is, return true, anders false
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Upload successful!");
                    return true;
                }
                else
                {
                    Debug.WriteLine("Upload failed: " + response.StatusCode);
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Log de uitzondering als er een fout optreedt
                Debug.WriteLine("Upload exception: " + ex.Message);
                return false;
            }
        }


        public async Task<List<FieldItem>> GetFieldsAsync()
        {
            if (!_configured)
                return null;

            try
            {
                var response = await _client.GetAsync("/api/fields").ConfigureAwait(false);
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                    return null;

                var fields = JsonSerializer.Deserialize<List<FieldItem>>(content);

                return fields;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("GetFieldsAsync exception: " + ex.Message);
                return null;
            }
        }

        public async Task<(bool Success, string Message, FieldDownloadDto Field)> DownloadFieldPreviewAsync(string fieldId)
        {
            if (!_configured)
                return (false, "AgShareClient is not configured.", null);

            try
            {
                var response = await _client.GetAsync($"/api/fields/" + fieldId).ConfigureAwait(false);
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                    return (false, $"Error downloading field: {content}", null);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var fieldDto = JsonSerializer.Deserialize<FieldDownloadDto>(content, options);

                if (fieldDto == null)
                    return (false, "Failed to parse field data.", null);

                // Parse the boundary and AB lines after downloading
                fieldDto.ParsedBoundary = AgShareFieldParser.ParseBoundary(fieldDto.Boundary);
                fieldDto.ParsedTracks = AgShareFieldParser.ParseAbLines(fieldDto.AbLines);

                return (true, "Field preview loaded successfully.", fieldDto);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("DownloadFieldPreviewAsync exception: " + ex.Message);
                return (false, "Exception occurred during field preview download.", null);
            }
        }



    }

}
