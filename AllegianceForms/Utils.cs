using AllegianceForms.Engine;
using Newtonsoft.Json;
using System.Drawing;
using System.IO;
using System.Text;

namespace AllegianceForms
{
    public class Utils
    {
        public static void SerialiseToFile(string filename, object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            File.WriteAllText(filename, json);
        }

        public static T DeserialiseFromFile<T>(string filename)
        {
            if (!File.Exists(filename)) return default(T);

            var json = File.ReadAllText(filename);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static string RandomString()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var sb = new StringBuilder();
            var length = StrategyGame.Random.Next(32) + 1;

            for (var l = 0; l < length; l++)
            {
                sb.Append(chars[StrategyGame.Random.Next(chars.Length)]);
            }

            return sb.ToString();
        }

        public static Bitmap GetAvatarImage(string key)
        {
            var hash = key.GetHashCode();
            var rnd = new System.Random(hash);
            
            const string basePath = ".\\Art\\Avatars\\full";

            var dirs = Directory.GetDirectories(basePath);
            var d = rnd.Next(dirs.Length);

            var imgs = Directory.GetFiles(dirs[d]);
            return (Bitmap)Image.FromFile(imgs[rnd.Next(imgs.Length)]);
        }

        public static void ReplaceColour(Bitmap bmp, Color newColour)
        {
            // Set the image's team color
            for (var x = 0; x < bmp.Width; x++)
            {
                for (var y = 0; y < bmp.Height; y++)
                {
                    var c = bmp.GetPixel(x, y);
                    if (c.A != 0) bmp.SetPixel(x, y, Color.FromArgb(c.A, newColour.R, newColour.G, newColour.B));
                }
            }
        }

        public static Bitmap CropImageToNonTransparent(Bitmap b)
        {
            var cropTo = new Rectangle(int.MaxValue, int.MaxValue, int.MinValue, int.MinValue);

            for (var x = 0; x < b.Width; x++ )
            {
                for (var y = 0; y < b.Height; y++)
                {
                    var p = b.GetPixel(x, y);
                    if (p.A != 0)
                    {
                        if (x < cropTo.X) cropTo.X = x;
                        if (x > cropTo.Width) cropTo.Width = x;
                        if (y < cropTo.Y) cropTo.Y = y;
                        if (y > cropTo.Height) cropTo.Height = y;
                    }
                }
            }
            cropTo.Width -= cropTo.X;
            cropTo.Height -= cropTo.Y;

            return CropImage(b, cropTo);
        }

        public static Bitmap CropImage(Bitmap b, Rectangle r)
        {
            var nb = new Bitmap(r.Width, r.Height);
            var g = Graphics.FromImage(nb);
            g.DrawImage(b, -r.X, -r.Y);
            return nb;
        }
    }
}
