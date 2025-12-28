using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AgOpenGPS.Core.AgShare.Models;
using Newtonsoft.Json;

namespace AgOpenGPS.Core.AgShare
{
    /// <summary>
    /// HTTP client for communicating with the AgShare API using API key authentication.
    /// Supports field upload, download, status checks, and querying both public and own fields.
    /// </summary>
    public class AgShareClient
    {
        private readonly HttpClient _client;

        /// <summary>
        /// Constructs client with base URL and API key
        /// </summary>
        public AgShareClient(string serverUrl, string apiKey)
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.Timeout = TimeSpan.FromSeconds(5);

            SetApiKey(apiKey);
            SetServerUrl(serverUrl);
        }

        /// <summary>
        /// Updates the API key
        /// </summary>
        public void SetApiKey(string apiKey)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("ApiKey", apiKey);
        }

        /// <summary>
        /// Updates the server URL
        /// </summary>
        public void SetServerUrl(string serverUrl)
        {
            _client.BaseAddress = new Uri(serverUrl);
        }

        /// <summary>
        /// Checks if the API key and connection are valid
        /// </summary>
        public static async Task<AgShareResult> CheckApiAsync(string baseUrl, string apiKey)
        {
            try
            {
                using (var tempClient = new HttpClient())
                {
                    tempClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    tempClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("ApiKey", apiKey);
                    tempClient.BaseAddress = new Uri(baseUrl);

                    var response = await tempClient.GetAsync("/api/fields");

                    if (response.IsSuccessStatusCode)
                    {
                        return AgShareResult.Success();
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        return AgShareResult.Failure(AgShareError.InvalidApiKey());
                    }
                    else
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        return AgShareResult.Failure(AgShareError.WrongStatusCode(response.StatusCode, responseBody));
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                return AgShareResult.Failure(AgShareError.HttpRequestException(ex));
            }
        }

        /// <summary>
        /// Uploads a field by ID
        /// </summary>
        public async Task<AgShareResult> UploadFieldAsync(Guid fieldId, UploadFieldDto fieldPayload)
        {
            try
            {
                var json = JsonConvert.SerializeObject(fieldPayload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.PutAsync($"/api/fields/{fieldId}", content);

                if (response.IsSuccessStatusCode)
                {
                    return AgShareResult.Success();
                }
                else
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return AgShareResult.Failure(AgShareError.WrongStatusCode(response.StatusCode, responseBody));
                }
            }
            catch (HttpRequestException ex)
            {
                return AgShareResult.Failure(AgShareError.HttpRequestException(ex));
            }
        }

        /// <summary>
        /// Retrieves a list of fields owned by the current user
        /// </summary>
        public async Task<AgShareResult<List<GetOwnFieldDto>>> GetOwnFieldsAsync()
        {
            try
            {
                var response = await _client.GetAsync("/api/fields");
                string json = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var data = JsonConvert.DeserializeObject<List<GetOwnFieldDto>>(json);
                    return AgShareResult<List<GetOwnFieldDto>>.Success(data);
                }
                else
                {
                    return AgShareResult<List<GetOwnFieldDto>>.Failure(AgShareError.WrongStatusCode(response.StatusCode, json));
                }
            }
            catch (JsonException ex)
            {
                return AgShareResult<List<GetOwnFieldDto>>.Failure(AgShareError.JsonException(ex));
            }
            catch (HttpRequestException ex)
            {
                return AgShareResult<List<GetOwnFieldDto>>.Failure(AgShareError.HttpRequestException(ex));
            }
        }

        /// <summary>
        /// Downloads a specific field
        /// </summary>
        public async Task<AgShareResult<GetFieldDto>> DownloadFieldAsync(Guid fieldId)
        {
            try
            {
                var response = await _client.GetAsync($"/api/fields/{fieldId}");
                string json = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var data = JsonConvert.DeserializeObject<GetFieldDto>(json);
                    return AgShareResult<GetFieldDto>.Success(data);
                }
                else
                {
                    return AgShareResult<GetFieldDto>.Failure(AgShareError.WrongStatusCode(response.StatusCode, json));
                }
            }
            catch (JsonException ex)
            {
                return AgShareResult<GetFieldDto>.Failure(AgShareError.JsonException(ex));
            }
            catch (HttpRequestException ex)
            {
                return AgShareResult<GetFieldDto>.Failure(AgShareError.HttpRequestException(ex));
            }
        }

        // !!! This is not implemented yet !!!
        /// <summary>
        /// Queries public fields within a given radius around a lat/lon
        /// </summary>
        public async Task<string> GetPublicFieldsAsync(double lat, double lon, double radius = 50)
        {
            var response = await _client.GetAsync($"/api/fields/public?lat={lat}&lon={lon}&radius={radius}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
