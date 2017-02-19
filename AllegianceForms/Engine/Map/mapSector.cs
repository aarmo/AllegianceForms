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
        public bool StartingSector { get; set; }
        public Rectangle Bounds => new Rectangle(MapPosition.X * GameMaps.SectorSpacing, MapPosition.Y * GameMaps.SectorSpacing, GameMaps.SectorDiameter, GameMaps.SectorDiameter);

        public bool Conflict { get; set; }
        public bool CriticalAlert { get; set; }
        public bool Colour1Set { get; set; }
        public bool Colour2Set { get; set; }
        public Brush Colour1 { get; set; }
        public Brush Colour2 { get; set; }

        public MapSector(int id, string name, Point pos)
        {
            VisibleToTeam = new bool[StrategyGame.NumTeams];
            Id = id;
            Name = name;
            MapPosition = pos;

            Colour1Set = Colour2Set = false;
            Colour1 = Colour2 = new SolidBrush(Color.DimGray);
        }

        public int CompareTo(object obj)
        {
            var o = (MapSector)obj;

            return Id.CompareTo(o.Id);
        }

        public void UpdateColours()
        {
            var visibleBases = StrategyGame.AllBases.Where(_ => _.SectorId == Id && _.CanLaunchShips() && _.VisibleToTeam[0]);
            Colour1Set = Colour2Set = false;
            foreach (var b in visibleBases)
            {
                if (!Colour1Set)
                {
                    Colour1 = b.TeamColor;
                    Colour1Set = true;
                }
                else if (!Colour2Set)
                {
                    Colour2 = b.TeamColor;
                    Colour2Set = true;
                }
                else
                {
                    break;
                }
            }

            CriticalAlert = StrategyGame.AllUnits.Any(_ => _.SectorId == Id && _.VisibleToTeam[0] && _.Team != 1 && _.CanAttackBases());
            Conflict = StrategyGame.AllUnits.Any(_ => _.SectorId == Id && _.VisibleToTeam[0] && _.Team != 1);
        }
    }
}
