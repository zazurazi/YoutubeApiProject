using System.Net.Http;
using System.Threading.Tasks;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Microsoft.Extensions.Configuration;
using YouTubeApiProject.Models;

namespace YouTubeApiProject.Services
{
    public class YouTubeApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public YouTubeApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            // Access the YouTube API key from appsettings.json
            _apiKey = configuration["YouTubeApiKey"]; // Ensure you're using the correct key name
            if (string.IsNullOrEmpty(_apiKey))
            {
                throw new InvalidOperationException("YouTube API key is not configured correctly in appsettings.json.");
            }
        }

        public async Task<(List<YouTubeVideoModel> Videos, string NextPageToken)> SearchVideosAsync(string query, string pageToken = null)
        {
            try
            {
                var youtubeService = new YouTubeService(new BaseClientService.Initializer
                {
                  
                    ApiKey = _apiKey,
                    ApplicationName = this.GetType().ToString()
                });

                var searchRequest = youtubeService.Search.List("snippet");
                searchRequest.Q = query;
                searchRequest.MaxResults = 10;
                searchRequest.PageToken = pageToken;

                var searchResponse = await searchRequest.ExecuteAsync();

                var videos = searchResponse.Items.Select(item => new YouTubeVideoModel
                {
                    Title = item.Snippet.Title,
                    Description = item.Snippet.Description,
                    ThumbnailUrl = item.Snippet.Thumbnails.Default__.Url,
                    VideoId = item.Id.VideoId
                }).ToList();

                return (videos, searchResponse.NextPageToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while calling YouTube API: {ex.Message}");
                return (new List<YouTubeVideoModel>(), null); // Return an empty list in case of error
            }
        }
    }
}
