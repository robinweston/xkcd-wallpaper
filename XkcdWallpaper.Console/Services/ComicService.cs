using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace XkcdWallpaper.Console.Services
{
    class ComicService
    {
        private const string LatestComicJsonUrl = "http://xkcd.com/info.0.json";
        private const string ComicJsonUrl = "http://xkcd.com/{0}/info.0.json";

        public async Task<int> GetTotalComics()
        {
            var httpClient = new HttpClient();

            var response = await httpClient.GetAsync(LatestComicJsonUrl);
            var jsonString = await response.Content.ReadAsStringAsync();
            dynamic json = await JsonConvert.DeserializeObjectAsync(jsonString);

            return json.num;
        }

        public async Task<Comic> GetRandomComic()
        {
            var totalComics = await GetTotalComics();

            var comicNumber = new Random().Next(totalComics) + 1;
            //comicNumber = 1;
            return await GetComic(comicNumber);
        }

        public async Task<Comic> GetComic(int number)
        {
            System.Console.WriteLine("Getting comic {0}", number);

            var httpClient = new HttpClient();

            var url = string.Format(ComicJsonUrl, number);
            var response = await httpClient.GetAsync(url);
            var jsonString = await response.Content.ReadAsStringAsync();
            return await JsonConvert.DeserializeObjectAsync<Comic>(jsonString);
        }
            
    }
}
