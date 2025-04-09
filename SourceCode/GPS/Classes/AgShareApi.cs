using System;
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

        var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5000/api/users/me");
        request.Headers.Authorization = new AuthenticationHeaderValue("ApiKey", ApiKey);

        try
        {
            var response = await client.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public static async Task<bool> UploadIsoXmlFieldAsync(string fieldId, object jsonPayload)
    {
        if (string.IsNullOrWhiteSpace(ApiKey)) return false;

        using var client = new HttpClient();
        var content = new StringContent(JsonSerializer.Serialize(jsonPayload), Encoding.UTF8, "application/json");
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("ApiKey", ApiKey);

        var response = await client.PutAsync($"http://localhost:5000/api/isoxmlfields/{fieldId}", content);

        return response.IsSuccessStatusCode;
    }

}
