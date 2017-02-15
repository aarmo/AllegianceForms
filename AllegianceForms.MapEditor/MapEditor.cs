using AllegianceForms.Engine.Generation;
using AllegianceForms.Engine.Map;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AllegianceForms.MapEditor
{
    public partial class MapEditor : Form
    {
        const int GridSize  = 10;
        private const string MapFilter = "Map Files (*.map)|*.map";
        private const string SectorNameData = ".\\Data\\Names-Sector.txt";

        private Bitmap _frame;
        private bool _modified;
        private GameMap _map;
        private MapSector _selectedSector;
        private readonly RandomString _sectorNames;
        private string _filename;

        private MapSector SelectedSector
        {
            get { return _selectedSector; }
            set
            {
                _selectedSector = value;

                if (_selectedSector == null)
                {
                    PropertiesPanel.Enabled = false;
                    SectorName.Text = string.Empty;
                    StartPos.Checked = false;
                }
                else
                {
                    PropertiesPanel.Enabled = true;
                    SectorName.Text = _selectedSector.Name;
                    StartPos.Checked = _selectedSector.StartingSector;
                }

            }
        }

        public MapEditor()
        {
            InitializeComponent();

            _frame = new Bitmap(Width, Height);
            _map = new GameMap();
            _sectorNames = new RandomString(SectorNameData);

            Random2Size.Items.Clear();
            Random2Size.Items.AddRange(Enum.GetNames(typeof(EMapSize)));
            Random2Size.SelectedIndex = 2;

            UpdateFrame();
        }

        private void UpdateFrame()
        {
            const int sectorDiam = 20;
            if (_map == null) return;

            var g = Graphics.FromImage(_frame);

            g.Clear(BackColor);

            // Draw Grid
            var width = _frame.Width;
            var height = _frame.Height;
            var gridPen = Pens.White;
            for (var x = 0; x < width; x += GridSize)
            {
                g.DrawLine(gridPen, x, 0, x, height);
            }
            for (var y = 0; y < height; y += GridSize)
            {
                g.DrawLine(gridPen, 0, y, width, y);
            }

            // Draw Lines
            var linePen = new Pen(Brushes.DarkBlue, 3);
            foreach (var w in _map.Wormholes)
            {
                var s1 = w.Sector1.MapPosition;
                var s2 = w.Sector2.MapPosition;
                g.DrawLine(linePen, GridSize * s1.X, GridSize * s1.Y, GridSize * s2.X, GridSize * s2.Y);
            }

            // Draw Sectors
            var sectorFill = Brushes.DimGray;
            var sectorFillSelected = Brushes.Orange;
            var sectorFillStart = Brushes.Black;

            foreach (var s in _map.Sectors)
            {
                var p = s.MapPosition;
                g.FillEllipse(s == _selectedSector ? sectorFillSelected : sectorFill, GridSize * p.X - sectorDiam / 2, GridSize * p.Y - sectorDiam / 2, sectorDiam, sectorDiam);

                if (s.StartingSector)
                {
                    g.FillEllipse(sectorFillStart, GridSize * p.X - 5 / 2, GridSize * p.Y - 5 / 2, 5, 5);
                }
            }

            Invalidate();
        }

        private void MapEditor_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            if (_frame != null) g.DrawImage(_frame, 0, 0);
        }

        private void MapEditor_SizeChanged(object sender, System.EventArgs e)
        {
            _frame = new Bitmap(Width, Height);

            UpdateFrame();
        }

        private void MapEditor_MouseClick(object sender, MouseEventArgs e)
        {
            SaveMap.Focus();

            var x = e.X / GridSize;
            var y = e.Y / GridSize;

            if (e.Button == MouseButtons.Right)
            {
                SelectedSector = null;
                foreach (var s in _map.Sectors)
                {
                    var p = s.MapPosition;
                    if (p.X >= x - 1 && p.Y >= y - 1
                        && p.X <= x + 1 && p.Y <= y + 1)
                    {
                        SelectedSector = s;
                        break;
                    }
                }
                UpdateFrame();
            }
            
            if (e.Button == MouseButtons.Left)
            {
                var lastSelected = SelectedSector;

                SelectedSector = null;
                foreach (var s in _map.Sectors)
                {
                    var p = s.MapPosition;
                    if (p.X >= x - 1 && p.Y >= y - 1
                        && p.X <= x + 1 && p.Y <= y + 1)
                    {
                        SelectedSector = s;
                        break;
                    }
                }

                if (SelectedSector == null)
                {
                    _map.Sectors.Add(new MapSector(0, _sectorNames.NextString, new Point(x, y)));
                    SetTitle(true, _filename);
                }
                else if (lastSelected != null && SelectedSector != null)
                {
                    var existing = _map.Wormholes.Where(w => w.LinksTo(lastSelected));
                    if (!existing.Any(w => w.LinksTo(SelectedSector)))
                    {
                        _map.Wormholes.Add(new Wormhole(lastSelected, SelectedSector));
                        SetTitle(true, _filename);
                    }
                }
                UpdateFrame();
            }

        }

        private void ClearMap_Click(object sender, System.EventArgs e)
        {
            if (!PromptToSave()) return;

            _map.Clear();
            _sectorNames.Reset();
            SelectedSector = null;
            SetTitle(false, string.Empty);

            UpdateFrame();
        }

        private void MapEditor_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && _selectedSector != null)
            {
                var x = e.X / GridSize;
                var y = e.Y / GridSize;

                // Move this Selected Sector!
                SelectedSector.MapPosition = new Point(x, y);
                SetTitle(true, _filename);

                UpdateFrame();
            }
        }

        private void MapEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (_selectedSector == null || SectorName.Focused) return;
            
            if (e.KeyCode == Keys.Delete)
            {
                RemoveSector(_map, SelectedSector);
                SetTitle(true, _filename);
                SelectedSector = null;
            }
            else if (e.KeyCode == Keys.Space)
            {
                SelectedSector = null;
                e.Handled = true;
            }
            else
            {
                return;
            }

            UpdateFrame();
        }

        private void RemoveSector(GameMap map, MapSector s)
        {
            map.Wormholes.RemoveAll(w => w.LinksTo(s));
            map.Sectors.Remove(s);
        }

        private void SectorName_TextChanged(object sender, System.EventArgs e)
        {
            if (SelectedSector == null)
            {
                SectorName.Text = string.Empty;
                return;
            }
            if (_selectedSector.Name == SectorName.Text) return;

            _selectedSector.Name = SectorName.Text;
            SetTitle(true, _filename);
        }

        private void StartPos_CheckedChanged(object sender, System.EventArgs e)
        {
            if (SelectedSector == null)
            {
                StartPos.Checked = false;
                return;
            }
            if (_selectedSector.StartingSector == StartPos.Checked) return;

            _selectedSector.StartingSector = StartPos.Checked;
            SetTitle(true, _filename);

            UpdateFrame();
        }

        private void LoadMap_Click(object sender, System.EventArgs e)
        {
            if (!PromptToSave()) return;

            var file = Dialogs.OpenFile("Open Map", MapFilter);

            if (file != string.Empty)
            {
                _map = Utils.LoadMapFromFile(file);
                SetTitle(false, file);
                UpdateFrame();
            }
        }

        private void SetTitle(bool modified, string filename)
        {
            _filename = filename;
            _modified = modified;
            Text = string.Format("Allegiance Forms - Map Editor [{0}]{1}", filename, modified ? "*" : string.Empty);
        }

        private bool PromptToSave()
        {
            if (!_modified) return true;

            var result = MessageBox.Show("Do you want to save your changes?", "Map Modified",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);

            switch (result)
            {
                case DialogResult.Cancel:
                    return false;
                case DialogResult.No:
                    return true;
            }

            var file = Dialogs.SaveFile("Save Map", MapFilter);
            if (file != string.Empty)
            {
                Utils.SerialiseToFile(file, _map);
                SetTitle(false, file);
                return true;
            }

            return false;
        }

        private void SaveMap_Click(object sender, System.EventArgs e)
        {
            var file = Dialogs.SaveFile("Save Map", MapFilter);
            if (file != string.Empty)
            {
                Utils.SerialiseToFile(file, _map);
                SetTitle(false, file);
            }
        }

        private void MapEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!PromptToSave())
            {
                e.Cancel = true;
            }
        }

        private void Random2_Click(object sender, System.EventArgs e)
        {
            if (!PromptToSave()) return;

            _map = RandomMap.Generate((EMapSize)Enum.Parse(typeof(EMapSize), Random2Size.SelectedItem.ToString()), Symmetrical.Checked, _sectorNames);

            SetTitle(false, "Random Map");
            UpdateFrame();
        }
    }
}
