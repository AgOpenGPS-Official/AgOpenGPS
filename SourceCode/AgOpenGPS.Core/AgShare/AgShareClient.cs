using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AgLibrary.Logging;

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

        // Test connection to AgShare server with provided API key
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
                    return (false, $"API Key error: {response.StatusCode}, {content}");

                return (true, content);
            }
            catch (HttpRequestException ex)
            {
                Log.EventWriter("AgShare connection failed: " + ex.Message);
                return (false, $"Failed to connect to server: {ex.Message}");
            }
        }

        // Upload field JSON to the AgShare backend
        public async Task<bool> UploadAsync(string fieldId, object jsonPayload)
        {
            if (!_configured)
                return false;

            var content = new StringContent(JsonSerializer.Serialize(jsonPayload), Encoding.UTF8, "application/json");

            try
            {
                var response = await _client.PutAsync($"/api/fields/{fieldId}", content).ConfigureAwait(false);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Log.EventWriter("AgShare upload failed: " + ex.Message);
                return false;
            }
        }

        // Retrieve list of user fields
        public async Task<List<FieldItem>> GetFieldsAsync()
        {
            if (!_configured)
                return null;

            try
            {
                var response = await _client.GetAsync("/api/fields").ConfigureAwait(false);
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return null;

                var fields = JsonSerializer.Deserialize<List<FieldItem>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return fields;
            }
            catch (Exception ex)
            {
                Log.EventWriter("AgShare field list download failed: " + ex.Message);
                return null;
            }
        }

        // Download a field for preview including boundary and track data
        public async Task<(bool Success, string Message, FieldDownloadDto Field)> DownloadFieldPreviewAsync(string fieldId)
        {
            if (!_configured)
                return (false, "AgShareClient is not configured.", null);

            try
            {
                var response = await _client.GetAsync($"/api/fields/{fieldId}").ConfigureAwait(false);
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                    return (false, $"Error downloading field: {content}", null);

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var fieldDto = JsonSerializer.Deserialize<FieldDownloadDto>(content, options);

                if (fieldDto == null)
                    return (false, "Failed to parse field data.", null);

                fieldDto.ParsedBoundary = AgShareFieldParser.ParseBoundary(fieldDto.Boundary);
                fieldDto.ParsedTracks = AgShareFieldParser.ParseAbLines(fieldDto.AbLines);

                return (true, "Field preview loaded successfully.", fieldDto);
            }
            catch (Exception ex)
            {
                Log.EventWriter("AgShare preview download failed: " + ex.Message);
                return (false, "Exception occurred during field preview download: " + ex.Message, null);
            }
        }

        // Download field info by ID
        public async Task<FieldInfoDto> GetFieldByIdAsync(string fieldId)
        {
            if (!_configured)
                return null;

            try
            {
                var response = await _client.GetAsync($"/api/fields/{fieldId}").ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                    return null;

                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JsonSerializer.Deserialize<FieldInfoDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                Log.EventWriter("AgShare field info retrieval failed: " + ex.Message);
                return null;
            }
        }
    }

    public class FieldInfoDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsPublic { get; set; }
    }
}