using AllegianceForms.Engine;
using AllegianceForms.Engine.Map;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AllegianceForms.Forms
{
    public partial class MapDesigner : Form
    {

        private readonly Bitmap _frame;
        private List<SectorPath> _paths = new List<SectorPath>();
        private Color _selectedColour = Color.Lime;
        private Color _normalColour = Color.CornflowerBlue;

        private Label _selectedSector = null;

        private bool _controlDown = false;
        private bool _shiftDown = false;
        
        public MapDesigner()
        {
            InitializeComponent();

            _selectedSector = SectorLabel;
            _frame = new Bitmap(MapPanel.Width, MapPanel.Height);

            StrategyGame.SetupGame(GameSettings.Default());
        }
        
        private void SectorLabel_Click(object sender, EventArgs e)
        {
            var lbl = sender as Label;
            if (lbl == null) return;

            MapPanel.Focus();
            if (_shiftDown && _selectedSector != null)
            {
                if (!_paths.Any(_ => (_.FromSector == lbl || _.FromSector == _selectedSector) && (_.ToSector == lbl || _.ToSector == _selectedSector)))
                {
                    _paths.Add(new SectorPath { FromSector = _selectedSector, ToSector = lbl });
                    DrawPaths();
                }
            }
            else
            {
                if (_selectedSector != null) _selectedSector.BackColor = _normalColour;
                _selectedSector = lbl;
                _selectedSector.BackColor = _selectedColour;
            }
        }

        private Label AddSector(int x, int y)
        {
            var l = new Label
            {
                ForeColor = SectorLabel.ForeColor,
                Width = SectorLabel.Width,
                Height = SectorLabel.Height,
                AutoSize = false,
                Text = string.Empty,
                BackColor = _selectedColour,
                Top = SectorLabel.Height * y,
                Left = SectorLabel.Width * x,
                BorderStyle = SectorLabel.BorderStyle,
                Cursor = SectorLabel.Cursor,
                TextAlign = SectorLabel.TextAlign
            };
            l.Click += SectorLabel_Click;
            MapPanel.Controls.Add(l);

            if (_selectedSector != null) _selectedSector.BackColor = _normalColour;
            _selectedSector = l;

            return l;
        }

        private void MapPanel_MouseClick(object sender, MouseEventArgs e)
        {
            MapPanel.Focus();

            if (_controlDown)
            {
                // Add a sector to the map
                AddSector(e.X / SectorLabel.Width, e.Y / SectorLabel.Height);
            }
            else
            {
                if (_selectedSector != null) _selectedSector.BackColor = _normalColour;
                _selectedSector = null;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (_selectedSector == null || MapName.Focused) return base.ProcessCmdKey(ref msg, keyData);

            switch (keyData)
            {
                case Keys.S:
                case Keys.Down:
                    _selectedSector.Top += SectorLabel.Height;
                    DrawPaths();
                    return true;
                    
                case Keys.D1:
                case Keys.D2:
                case Keys.D3:
                case Keys.D4:
                    _selectedSector.Text = (_selectedSector.Text == string.Empty ? keyData.ToString().Replace("D", string.Empty) : string.Empty);
                    return true;

                case Keys.Left:
                case Keys.A:
                    _selectedSector.Left -= SectorLabel.Width;
                    DrawPaths();
                    return true;

                case Keys.Right:
                case Keys.D:
                    _selectedSector.Left += SectorLabel.Width;
                    DrawPaths();
                    return true;

                case Keys.Up:
                case Keys.W:
                    _selectedSector.Top -= SectorLabel.Height;
                    DrawPaths();
                    return true;

                case Keys.Delete:
                    if (_selectedSector != null)
                    {
                        MapPanel.Controls.Remove(_selectedSector);
                        _paths.RemoveAll(_ => _.FromSector == _selectedSector || _.ToSector == _selectedSector);
                        DrawPaths();

                        if (MapPanel.Controls.Count > 0)
                        {
                            _selectedSector = (Label)MapPanel.Controls[MapPanel.Controls.Count - 1];
                            _selectedSector.BackColor = _selectedColour;
                        }
                        else
                        {
                            _selectedSector = null;
                        }
                        return true;
                    }
                    break;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void MapDesigner_KeyDown(object sender, KeyEventArgs e)
        {
            _controlDown = e.Control;
            _shiftDown = e.Shift;            
        }

        private void MapDesigner_KeyUp(object sender, KeyEventArgs e)
        {
            _controlDown = e.Control;
            _shiftDown = e.Shift;
        }

        private void Button_MouseEnter(object sender, EventArgs e)
        {
            SoundEffect.Play(ESounds.mouseover);
            var b = sender as Button;
            if (b != null) b.BackColor = Color.DarkGreen;
        }

        private void Button_MouseLeave(object sender, EventArgs e)
        {
            var b = sender as Button;
            if (b != null) b.BackColor = Color.Black;
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            MapName.Text = string.Empty;

            _selectedSector = null;
            SectorLabel.BackColor = _normalColour;
            MapPanel.Controls.Clear();

            _paths.Clear();
            DrawPaths();
        }

        private SimpleGameMap CreateMap()
        {
            var map = new SimpleGameMap(MapName.Text);

            for (var i = 0; i < MapPanel.Controls.Count; i++)
            {
                var lbl = MapPanel.Controls[i] as Label;
                if (lbl == null) continue;

                var x = lbl.Left / SectorLabel.Width;
                var y = lbl.Top / SectorLabel.Height;
                var s = new SimpleMapSector(i, new Point(x, y));
                s.StartingSectorTeam = lbl.Text != string.Empty ? Convert.ToInt16(lbl.Text) : 0;
                map.Sectors.Add(s);

                lbl.Tag = i;
            }

            foreach (var p in _paths)
            {
                var from = (int)p.FromSector.Tag;
                var to = (int)p.ToSector.Tag;
                var w = new WormholeId(from, to);
                map.WormholeIds.Add(w);
            }

            return map;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            var map = CreateMap();            

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Utils.SerialiseToFile(saveFileDialog.FileName, map);
            }
        }

        private void Open_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Clear_Click(sender, e);

                var map = Utils.DeserialiseFromFile<SimpleGameMap>(openFileDialog.FileName);
                if (map == null) return;

                LoadMap(map);
            }
        }
        
        private void LoadMap(SimpleGameMap map)
        {
            MapName.Text = map.Name;

            foreach (var s in map.Sectors)
            {
                var l = AddSector(s.MapPosition.X, s.MapPosition.Y);
                l.Text = s.StartingSectorTeam != 0  ? s.StartingSectorTeam.ToString() : string.Empty;
            }

            foreach (var w in map.WormholeIds)
            {
                var from = MapPanel.Controls[w.FromSectorId] as Label;
                var to = MapPanel.Controls[w.ToSectorId] as Label;

                _paths.Add(new SectorPath { FromSector = from, ToSector = to });
            }
            DrawPaths();
        }

        private void MapPanel_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            if (_frame != null) g.DrawImage(_frame, 0, 0);
        }

        private Pen _wormholePen = new Pen(Color.DarkGray, 4);
        private void DrawPaths()
        {
            var g = Graphics.FromImage(_frame);
            g.Clear(MapPanel.BackColor);

            foreach (var p in _paths)
            {
                var x1 = p.FromSector.Left + p.FromSector.Width / 2;
                var x2 = p.ToSector.Left + p.ToSector.Width / 2;
                var y1 = p.FromSector.Top + p.FromSector.Height / 2;
                var y2 = p.ToSector.Top + p.ToSector.Height / 2;

                g.DrawLine(_wormholePen, x1, y1, x2, y2);
            }

            Invalidate();
        }

        private void Preset_Click(object sender, EventArgs e)
        {
            Clear_Click(sender, e);

            var teams = StrategyGame.Random.Next(2, 5);
            var map = GameMaps.LoadMap(GameMaps.RandomName(teams)).ToSimpleMap();
            LoadMap(map);
        }

        private void Preview_Click(object sender, EventArgs e)
        {
            var map = CreateMap();
            StrategyGame.NumTeams = map.Sectors.Count(_ => _.StartingSectorTeam > 0);
            var gameMap = GameMap.FromSimpleMap(map, true);

            var i = new Bitmap(MapPicture.Width, MapPicture.Height);
            var g = Graphics.FromImage(i);
            gameMap.Draw(g, -1);

            MapPicture.Image = i;
            MapPicture.Visible = true;
            Preview.Visible = false;
            /*
            StrategyGame.Map = gameMap;
            var f = new Map();
            f.ShowDialog(this);
            */
        }

        private void MapPicture_Click(object sender, EventArgs e)
        {
            MapPicture.Visible = false;
            Preview.Visible = true;
            MapPicture.Image = null;
        }
    }

    public class SectorPath
    {
        public Label FromSector;
        public Label ToSector;
    }
}
