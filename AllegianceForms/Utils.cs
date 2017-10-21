using AllegianceForms.Engine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
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
            var rnd = new Random(hash);
            
            const string basePath = ".\\Art\\Avatars\\full";

            var dirs = Directory.GetDirectories(basePath);
            var d = rnd.Next(dirs.Length);

            var imgs = Directory.GetFiles(dirs[d]);
            return (Bitmap)Image.FromFile(imgs[rnd.Next(imgs.Length)]);
        }

        public static Bitmap ScaleColoursRandomly(Image img)
        {
            var rScale = (float)StrategyGame.Random.NextDouble();
            var gScale = (float)StrategyGame.Random.NextDouble();
            var bScale = (float)StrategyGame.Random.NextDouble();

            var b = new Bitmap(img.Width, img.Height, PixelFormat.Format32bppPArgb);
            using (var g = Graphics.FromImage(b))
            {
                float[][] colorMatrixElements = {
                   new float[] { rScale,  0,  0,  0, 0},
                   new float[] {0, gScale,  0,  0, 0},
                   new float[] {0,  0, bScale,  0, 0},
                   new float[] {0,  0,  0,  1f, 0},
                   new float[] {0, 0, 0, 0, 1f}
                };
                var colorMatrix = new ColorMatrix(colorMatrixElements);

                var imageAttributes = new ImageAttributes();
                imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                g.DrawImage(img,
                    new Rectangle(0, 0, img.Width, img.Height),
                    0, 0,
                    img.Width,
                    img.Height,
                    GraphicsUnit.Pixel,
                    imageAttributes);

                return b;
            }
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

        public static bool IsPointOnLine(Point p, Point a, Point b, float t = 1E-03f)
        {
            // ensure points are collinear
            var zero = (b.X - a.X) * (p.Y - a.Y) - (p.X - a.X) * (b.Y - a.Y);
            if (zero > t || zero < -t) return false;

            // check if x-coordinates are not equal
            if (a.X - b.X > t || b.X - a.X > t)
                // ensure x is between a.x & b.x (use tolerance)
                return a.X > b.X
                    ? p.X + t > b.X && p.X - t < a.X
                    : p.X + t > a.X && p.X - t < b.X;

            // ensure y is between a.y & b.y (use tolerance)
            return a.Y > b.Y
                ? p.Y + t > b.Y && p.Y - t < a.Y
                : p.Y + t > a.Y && p.Y - t < b.Y;
        }

        public static IEnumerable<T> Shuffle<T>(IEnumerable<T> source, Random rnd)
        {
            T[] elements = source.ToArray();
            for (int i = elements.Length - 1; i >= 0; i--)
            {
                var swapIndex = rnd.Next(i + 1);
                yield return elements[swapIndex];
                elements[swapIndex] = elements[i];
            }
        }

        public static bool IsDefault<T, TU>(KeyValuePair<T, TU> pair)
        {
            return pair.Equals(new KeyValuePair<T, TU>());
        }


        public static double AngleBetweenPoints(PointF from, PointF to)
        {
            var deltaX = to.X - from.X;
            var deltaY = to.Y - from.Y;

            return Math.Atan2(deltaY, deltaX) * (180 / Math.PI);
        }

        public static int PerceivedBrightness(Color c)
        {
            return (int)Math.Sqrt(
            c.R * c.R * .299 +
            c.G * c.G * .587 +
            c.B * c.B * .114);
        }

        public static bool WithinDistance(float x1, float y1, float x2, float y2, float d)
        {
            var dx = (x1 - x2);
            var dy = (y1 - y2);

            return (dx * dx + dy * dy) < d * d;
        }

        public static PointF GetNewPoint(PointF p, float d, float angle)
        {
            var rad = (Math.PI / 180) * angle;
            return new PointF((float)(p.X + d * Math.Cos(rad)), (float)(p.Y + d * Math.Sin(rad)));
        }

        public static double DistanceBetween(Point p1, Point p2)
        {
            var dx = (p1.X - p2.X);
            var dy = (p1.Y - p2.Y);

            return Math.Sqrt(dx * dx + dy * dy);
        }

        public static T ClosestDistance<T>(float x, float y, IEnumerable<T> check) where T : GameEntity
        {
            return check.OrderBy(_ => ((x - _.CenterX) * (x - _.CenterX) + (y - _.CenterY) * (y - _.CenterY))).FirstOrDefault();
        }

        public static float Lerp(float firstFloat, float secondFloat, DateTime startTime, TimeSpan duration)
        {
            var by = (float)((DateTime.Now - startTime).TotalMilliseconds / duration.TotalMilliseconds);

            return firstFloat * by + secondFloat * (1 - by);
        }
        
        private static StringFormat _centeredFormat = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        public static void DrawCenteredText(Graphics g, Brush brush, string text, Rectangle rect)
        {
            g.DrawString(text, SystemFonts.SmallCaptionFont, brush, rect, _centeredFormat);
        }

        public static Color NewAlphaColour(int A, Color color)
        {
            return Color.FromArgb(A, color.R, color.G, color.B);
        }
    }
}
