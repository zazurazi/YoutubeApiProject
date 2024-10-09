using Microsoft.AspNetCore.Mvc;
using YouTubeApiProject.Services;
using System.Threading.Tasks;

namespace YouTubeApiProject.Controllers
{
    public class YouTubeController : Controller
    {
        private readonly YouTubeApiService _youTubeApiService;

        public YouTubeController(YouTubeApiService youtubeApiService)
        {
            _youTubeApiService = youtubeApiService;
        }

        // Action for the Search Page
        [HttpGet]
        public IActionResult Search()
        {
            return View();
        }

        // Action for the Results Page (Handles Form Submission)
        [HttpPost]
        public async Task<IActionResult> Search(string query, string pageToken = null)
        {
            if (string.IsNullOrEmpty(query))
            {
                ModelState.AddModelError("", "Please enter a search query.");
                return View();
            }

            try
            {
                // Call the YouTubeApiService to search for videos
                var (videos, nextPageToken) = await _youTubeApiService.SearchVideosAsync(query, pageToken);

                // If successful, pass the videos and nextPageToken to the view
                ViewBag.NextPageToken = nextPageToken;
                ViewBag.Query = query;

                return View("Results", videos);
            }
            catch (Exception ex)
            {
                // Log the error and show a user-friendly message
                Console.WriteLine($"Error: {ex.Message}");
                ViewBag.Error = "There was a problem retrieving videos. Please try again later.";
                return View();
            }
        }
    }
}
