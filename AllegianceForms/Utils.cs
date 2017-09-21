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

        public static Bitmap GenerateAvatarImage(string key)
        {
            var hash = key.GetHashCode();
            var rnd = new System.Random(hash);
            const string basePath = ".\\Art\\Avatars\\generated";
            var sets = Directory.GetDirectories(basePath);
            var s = rnd.Next(sets.Length);

            var robot = DrawAllImagesinSubDir(rnd, sets[s], sets[s]);

            var hueShift = rnd.Next(180);

            for (var w = 0; w < robot.Width; w++)
            {
                for (var h = 0; h < robot.Height; h++)
                {
                    var c = robot.GetPixel(w, h);
                    var c2 = ColorHelper.ShiftHue(c, hueShift);
                    robot.SetPixel(w, h, c2);
                }
            }

            return robot;
        }

        private static Bitmap DrawAllImagesinSubDir(System.Random rnd, string dirPath, string imgPath)
        {
            var imgs = Directory.GetFiles(imgPath);

            if (imgs.Length > 0)
            {
                Graphics g = null;
                Bitmap img = null;

                // Draw one image from the parent's sub dirs                
                var dirs = Directory.GetDirectories(dirPath);
                
                foreach (var d in Directory.GetDirectories(dirPath))
                {
                    var imgs2 = Directory.GetFiles(d);

                    if (imgs2.Length > 0)
                    {
                        var i = Image.FromFile(imgs2[rnd.Next(imgs2.Length)]);
                        if (g == null)
                        {
                            img = (Bitmap)i;
                            g = Graphics.FromImage(img);
                        }
                        else
                        {
                            g.DrawImage(i, 0, 0);
                        }
                    }
                }

                return img;
            }
            else
            {
                // Recurse deeper to find the final sub dirs
                var dirs = Directory.GetDirectories(imgPath);
                if (dirs.Length > 0)
                    return DrawAllImagesinSubDir(rnd, imgPath, dirs[rnd.Next(dirs.Length)]);
                else
                    return null;
            }
        }
    }
}
