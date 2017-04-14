using System;
using System.Drawing;
using System.Linq;

namespace AllegianceForms.Engine.Map
{
    public class MapSector : IComparable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Point MapPosition { get; set; }
        public bool[] VisibleToTeam { get; set; }
        public int StartingSector { get; set; }
        public Rectangle Bounds => new Rectangle(MapPosition.X * GameMaps.SectorSpacing + GameMaps.MapPadding, MapPosition.Y * GameMaps.SectorSpacing + GameMaps.MapPadding, GameMaps.SectorDiameter, GameMaps.SectorDiameter);
        public bool Conflict { get; set; }
        public bool CriticalAlert { get; set; }
        public bool Colour1Set { get; set; }
        public bool Colour2Set { get; set; }
        public Brush Colour1 { get; set; }
        public Brush Colour2 { get; set; }

        private Brush _originalColour;
        private StrategyGame _game;

        public MapSector(StrategyGame game, int id, string name, Point pos)
        {
            VisibleToTeam = new bool[game.NumTeams];
            Id = id;
            Name = name;
            MapPosition = pos;
            _game = game;

            Colour1Set = Colour2Set = false;
            Colour1 = Colour2 = _originalColour = new SolidBrush(Color.DimGray);
        }

        public override string ToString()
        {
            return Name;
        }

        public int CompareTo(object obj)
        {
            var o = (MapSector)obj;

            return Id.CompareTo(o.Id);
        }

        public void UpdateColours()
        {
            var visibleBases = _game.AllBases.Where(_ => _.SectorId == Id && _.VisibleToTeam[0] && _.CanLaunchShips());
            Colour1Set = Colour2Set = false;

            foreach (var b in visibleBases)
            {
                var t = b.Team - 1;
                if (!Colour1Set)
                {
                    Colour1 = _game.TeamBrushes[t];
                    Colour1Set = true;
                }
                else if (!Colour2Set)
                {
                    Colour2 = _game.TeamBrushes[t];
                    Colour2Set = true;
                }
                else
                {
                    break;
                }
            }

            if (!Colour1Set) Colour1 = _originalColour;
            if (!Colour2Set) Colour2 = _originalColour;

            CriticalAlert = _game.AllUnits.Any(_ => _.SectorId == Id && _.VisibleToTeam[0] && _.Alliance != _game.GameSettings.TeamAlliance[0] && _.CanAttackBases());
            Conflict = _game.AllUnits.Any(_ => _.SectorId == Id && _.VisibleToTeam[0] && _.Alliance != _game.GameSettings.TeamAlliance[0] && _.Type != EShipType.Lifepod);
        }
    }
}
