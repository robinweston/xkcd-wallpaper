using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace XkcdWallpaper.Console.Services
{
    public class WallpaperService
    {
        const int SpiSetDesktopWallpaper = 20;
        const int SpifUpdateIniFile = 0x01;
        const int SpifSendWinIniChange = 0x02;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        public void SetWallpaper(string imagePath, WallpaperStyle style)
        {
            var key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
            if (style == WallpaperStyle.Stretched)
            {
                key.SetValue(@"WallpaperStyle", 2.ToString());
                key.SetValue(@"TileWallpaper", 0.ToString());
            }

            if (style == WallpaperStyle.Centered)
            {
                key.SetValue(@"WallpaperStyle", 1.ToString());
                key.SetValue(@"TileWallpaper", 0.ToString());
            }

            if (style == WallpaperStyle.Tiled)
            {
                key.SetValue(@"WallpaperStyle", 1.ToString());
                key.SetValue(@"TileWallpaper", 1.ToString());
            }

            SystemParametersInfo(SpiSetDesktopWallpaper,
                0,
                imagePath,
                SpifUpdateIniFile | SpifSendWinIniChange);

            System.Console.WriteLine("Set desktop wallpaper to {0}", imagePath);
        }
    }
}
