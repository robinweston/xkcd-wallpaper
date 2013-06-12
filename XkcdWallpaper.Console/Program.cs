using System.Threading.Tasks;
using XkcdWallpaper.Console.Services;


namespace XkcdWallpaper.Console
{
    class Program
    {

        static void Main()
        {
            DoWork().Wait();
        }

        static async Task DoWork()
        {
            var comicService = new ComicService();
            var comic = await comicService.GetRandomComic();

            var imageService = new ImageService();
            var localImagePath = await imageService.GetLocalImagePath(comic);

            var wallPaperService = new WallpaperService();
            wallPaperService.SetWallpaper(localImagePath, WallpaperStyle.Centered);
        }
    }
}
