using System;
using System.Drawing;

namespace AllegianceForms.Engine.Map
{
    public class MapSector
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Point MapPosition { get; set; }
        public bool[] VisibleToTeam { get; set; }
        public bool StartingSector { get; set; }
        public Rectangle Bounds => new Rectangle(MapPosition.X * GameMaps.SectorSpacing, MapPosition.Y * GameMaps.SectorSpacing, GameMaps.SectorDiameter, GameMaps.SectorDiameter);

        public MapSector(int id, string name, Point pos)
        {
            VisibleToTeam = new bool[StrategyGame.NumTeams];
            Id = id;
            Name = name;
            MapPosition = pos;
        }
    }
}
