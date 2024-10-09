using YouTubeApiProject.Models;

    public class SearchViewModel
    {
        public string Query { get; set; }
        public List<YouTubeVideoModel> Videos { get; set; } // Using YouTubeVideoModel as the type for Videos list
    }
