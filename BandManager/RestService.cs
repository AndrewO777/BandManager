using System.Text;
using System.Text.Json;
using BandManager.Models;

namespace BandManager;

public class RestService
{
    private static readonly HttpClient Client;
    private static readonly JsonSerializerOptions SerializerOptions;
    private static readonly string BaseUrl = "http://10.0.2.2:8080/";
    
    static RestService()
    {
        Client = new HttpClient();
        SerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
    }

    public static async Task<List<LearnedSong>> GetSongsAsync()
    {
        List<LearnedSong>? songs = new List<LearnedSong>();

        Uri uri = new Uri($"{BaseUrl}songs");
        try
        {
            HttpResponseMessage response = await Client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                songs = JsonSerializer.Deserialize<List<LearnedSong>>(content, SerializerOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
        
        return songs ?? [];
    }

    public static async Task UpdateSong(LearnedSong song)
    {
        Uri uri = new Uri($"{BaseUrl}songs/{song.Id}");
        try
        {
            string json = JsonSerializer.Serialize<LearnedSong>(song, SerializerOptions);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await Client.PutAsync(uri, content);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
}