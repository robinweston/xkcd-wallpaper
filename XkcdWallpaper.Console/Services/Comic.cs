using Newtonsoft.Json;

namespace XkcdWallpaper.Console.Services
{
    class Comic
    {
        [JsonProperty(PropertyName = "alt")]
        public string AltText { get; set; }

        [JsonProperty(PropertyName = "img")]
        public string ImageUrl { get; set; }
        
        [JsonProperty(PropertyName = "num")]
        public string Number { get; set; }

        public string Title { get; set; }
    }
}
