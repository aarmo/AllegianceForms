using System.Collections.Generic;

namespace AllegianceForms.Engine.Map
{
    public class MapDistanceComparer : IComparer<MapSector>
    {
        private MapSector _origin;

        public MapDistanceComparer(MapSector origin)
        {
            _origin = origin;
        }

        public int Compare(MapSector x, MapSector y)
        {
            var xDistance = StrategyGame.DistanceBetween(x.MapPosition, _origin.MapPosition);
            var yDistance = StrategyGame.DistanceBetween(y.MapPosition, _origin.MapPosition);

            if (xDistance < yDistance) return -1;
            if (xDistance > yDistance) return 1;

            return 0;

        }
    }
}
