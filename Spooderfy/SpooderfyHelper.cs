using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace Spooderfy
{
    public class SpooderfyHelper
    {
        public static ImageSource ImageFromUrl(string url)
        {
            try
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(url, UriKind.Absolute);
                bitmap.EndInit();
                return bitmap;
            }
            catch
            {
                return default;
            }
        }
        public static void CreateImageForStreaming(Visual Target, int Width, int Height)
        {
            try
            {
                RenderTargetBitmap mapBits = new RenderTargetBitmap((int)Width, (int)Height, 96, 96, PixelFormats.Pbgra32);
                mapBits.Render(Target);
                PngBitmapEncoder pngImage = new PngBitmapEncoder();
                pngImage.Frames.Add(BitmapFrame.Create(mapBits));
                using (Stream fileStream = File.Create("DATA\\StreamImage.png"))
                {
                    pngImage.Save(fileStream);
                }
            }
            catch { }
        }

        private static Dictionary<string, DateTime> Error = new Dictionary<string, DateTime>();

        public static void HandleError(string Message)
        {
            Directory.CreateDirectory("DATA");
            if (!File.Exists("DATA\\error.sfy"))
                File.Create("DATA\\error.sfy").Close();

            if(Error.ContainsKey(Message))
            {
                if (DateTime.UtcNow - Error[Message] < TimeSpan.FromSeconds(300))
                    return;
                Error.Remove(Message);
            }

            Error.Add(Message, DateTime.UtcNow);

            StringBuilder Builder = new StringBuilder();
            Builder.AppendLine($"{File.ReadAllText("DATA\\error.sfy")}");
            Builder.Append($"{DateTime.UtcNow.ToShortDateString()} : {DateTime.UtcNow.ToShortTimeString()} - Error: {Message}");


#if DEBUG
            MessageBox.Show(Builder.ToString());
#endif

            File.WriteAllText("DATA\\error.sfy", Builder.ToString());
        }
    }
}
