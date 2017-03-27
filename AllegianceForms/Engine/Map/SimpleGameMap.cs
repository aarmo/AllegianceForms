using System.Collections.Generic;
using System.Drawing;

namespace AllegianceForms.Engine.Map
{
    public class SimpleGameMap
    {
        public string Name { get; set; }
        public List<SimpleMapSector> Sectors { get; set; }
        public List<WormholeId> WormholeIds { get; set; }

        public SimpleGameMap(string name)
        {
            Name = name;
            Sectors = new List<SimpleMapSector>();
            WormholeIds = new List<WormholeId>();
        }
    }

    public class WormholeId
    {
        public int FromSectorId;
        public int ToSectorId;

        public WormholeId(int from, int to)
        {
            FromSectorId = from;
            ToSectorId = to;
        }
    }

    public class SimpleMapSector
    {
        public int Id { get; set; }
        public Point MapPosition { get; set; }
        public int StartingSectorTeam { get; set; }

        public SimpleMapSector(int id, Point pos)
        {
            Id = id;
            MapPosition = pos;
        }
    }
}
