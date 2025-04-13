using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AgOpenGPS;
using AgOpenGPS.Properties;

public static class AgShareApi
{
    private static readonly HttpClient client = new();

    public static string? ApiKey => Settings.Default.AgShareApiKey;

    public static void SaveApiKey(string key)
    {
        Settings.Default.AgShareApiKey = key;
        Settings.Default.Save();
    }

    public static async Task<bool> TestApiKeyAsync()
    {
        if (string.IsNullOrWhiteSpace(ApiKey)) return false;

        var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5000/api/isoxmlfields");
        request.Headers.Authorization = new AuthenticationHeaderValue("ApiKey", ApiKey);

        try
        {
            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                Debug.WriteLine($"API key test failed: {response.StatusCode}");
            }
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Debug.WriteLine("API key test exception: " + ex.Message);
            return false;
        }

    }

    public static async Task<bool> UploadIsoXmlFieldAsync(string fieldId, object jsonPayload)
    {
        if (string.IsNullOrWhiteSpace(ApiKey)) return false;

        var content = new StringContent(JsonSerializer.Serialize(jsonPayload), Encoding.UTF8, "application/json");
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        // ✅ gebruik gedeelde client
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("ApiKey", ApiKey);

        try
        {
            var response = await client.PutAsync($"http://localhost:5000/api/isoxmlfields/{fieldId}", content);

            Debug.WriteLine("=== Response ===");
            Debug.WriteLine("Status: " + response.StatusCode);
            string responseBody = await response.Content.ReadAsStringAsync();
            Debug.WriteLine("Body: " + responseBody);

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Upload exception: " + ex.Message);
            return false;
        }
    }


}
