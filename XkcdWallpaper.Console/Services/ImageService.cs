using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace XkcdWallpaper.Console.Services
{
    class ImageService
    {
        private readonly HttpClient _httpClient;

        public ImageService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> GetLocalImagePath(Comic comic)
        {
            var localFilePath = GetLocalFilePath(comic.ImageUrl);

            if (!File.Exists(localFilePath))
            {
                await SaveRemoteImage(comic.ImageUrl, localFilePath, comic.Title, comic.AltText);
            }
            else
            {
                System.Console.WriteLine("Returning {0} from cache", localFilePath);
            }

            return localFilePath;
        }

        private async Task SaveRemoteImage(string imageUrl, string localPath, string topText, string bottomText)
        {
            var imageBytes = await _httpClient.GetByteArrayAsync(imageUrl);

            using (var stream = new MemoryStream(imageBytes))
            {
                using (var image = Image.FromStream(stream))
                using (var imageWithText = AddTextToImage(image, topText, bottomText))
                {
                    imageWithText.Save(localPath, ImageFormat.Bmp);
                }

                System.Console.WriteLine("Saved {0} to {1}", imageUrl, localPath);
            }
        }

        private string GetLocalFilePath(string remoteFileUrl)
        {
            var uri = new Uri(remoteFileUrl);
            var filename = Path.GetFileName(uri.LocalPath);
            filename = Path.ChangeExtension(filename, "bmp");
            var localPath = Path.Combine(Environment.CurrentDirectory, "Images", filename);
            return localPath;
        }

        private Image AddTextToImage(Image image, string topText, string bottomText)
        {
            const int textHeight = 50;

            var newImage = new Bitmap(image.Width, image.Height + (textHeight * 2));
            using (var gr = Graphics.FromImage(newImage))
            {
                var sf = new StringFormat {LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center};

                gr.DrawImage(image, 0, textHeight, image.Width, image.Height);
                var topFont = new Font("Lucida", 12, FontStyle.Bold);
                var bottomFont = new Font("Lucida", 10);
                
                gr.DrawString(topText, topFont, Brushes.White, new RectangleF(0, 0, image.Width, textHeight), sf);
                gr.DrawString(bottomText, bottomFont, Brushes.White, new RectangleF(0, image.Height + textHeight, image.Width, textHeight), sf);
            }

            return newImage;
        }
    }
}
