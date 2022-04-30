using System.Collections.Generic;
using System.Drawing;
using System.Linq;

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

        public bool WormholeExists(int id1, int id2)
        {
            return WormholeIds.Any(_ =>
                (_.ToSectorId == id1 || _.FromSectorId == id1) &&
                (_.ToSectorId == id2 || _.FromSectorId == id2));
        }
        
        public bool IsValid()
        {
            var startSectors = Sectors.FindAll(s => s.StartingSectorTeam != 0);
            if (startSectors.Count == 0) return false;

            return startSectors.TrueForAll(CanReachAllSectors);
        }

        public bool CanReachAllSectors(SimpleMapSector fromSector)
        {
            var visited = new List<SimpleMapSector>();

            var sectorsTravelled = TravelSector(fromSector, visited);

            return (Sectors.Count == sectorsTravelled);
        }

        private int TravelSector(SimpleMapSector start, ICollection<SimpleMapSector> visited)
        {
            if (visited.Contains(start)) return 0;

            visited.Add(start);
            var traveled = 1;

            var links = WormholeIds.FindAll(w => w.FromSectorId == start.Id || w.ToSectorId == start.Id);

            foreach (var l in links)
            {
                traveled += TravelSector(Sectors[l.ToSectorId], visited);
            }

            return traveled;
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
