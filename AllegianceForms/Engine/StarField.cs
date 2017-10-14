using IrrKlang;
using System.Drawing;

namespace AllegianceForms.Engine
{
    public class StarField
    {
        public Image Frame;
        private const int MaxDepth = 32;
        private Vector3D[] _stars = new Vector3D[256];

        private int _width;
        private int _height;
        private int _halfWidth;
        private int _halfHeight;

        public void Init(int width, int height)
        {
            Frame = new Bitmap(width, height);
            _width = width;
            _height = height;
            _halfWidth = width / 2;
            _halfHeight = height / 2;

            var r = StrategyGame.Random;
            for (var i = 0; i < _stars.Length; i++)
            {
                _stars[i] = new Vector3D(r.Next(-25, 25)
                                , r.Next(-25, 25)
                                , r.Next(1, MaxDepth));
            }
        }

        public void UpdateFrame()
        {
            var r = StrategyGame.Random;
            using (var g = Graphics.FromImage(Frame))
            {
                g.FillRectangle(Brushes.Black, 0, 0, _width, _height);

                for (var i = 0; i < _stars.Length; i++)
                {
                    _stars[i].Z -= 0.2f;

                    if (_stars[i].Z <= 0)
                    {
                        _stars[i].X = r.Next(-25, 25);
                        _stars[i].Y = r.Next(-25, 25);
                        _stars[i].Z = MaxDepth;
                    }

                    var k = 128.0f / _stars[i].Z;
                    var px = _stars[i].X * k + _halfWidth;
                    var py = _stars[i].Y * k + _halfHeight;

                    if (px >= 0 && px <= _width && py >= 0 && py <= _height)
                    {
                        var depth = (1 - _stars[i].Z / MaxDepth);
                        var size = depth * 5;
                        var shade = (int)(depth * 255);
                        var c = Color.FromArgb(shade, Color.White);
                        var b = new SolidBrush(c);
                        g.FillRectangle(b, px, py, size, size);
                    }
                    else
                    {
                        _stars[i].Z = 0;
                    }
                }
            }
        }
    }
}
