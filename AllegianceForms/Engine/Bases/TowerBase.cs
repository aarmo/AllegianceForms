using System.Drawing;

namespace AllegianceForms.Engine.Bases
{
    class TowerBase : Base
    {
        public TowerBase(string image, EBaseType type, int width, int height, Color teamColor, int team, int health, int sectorId) 
            : base(image, type, width, height, teamColor, team, health, sectorId)
        {
        }
    }
}
