using AllegianceForms.Engine.Weapons;
using System.Collections.Generic;
using System.Drawing;

namespace AllegianceForms.Engine.Ships
{
    public class CombatShip : Ship
    {
        public List<Weapon> Weapons { get; set; }
        
        public CombatShip(StrategyGame game, string imageFilename, int width, int height, Color teamColor, int team, int alliance, float health, int numPilots, EShipType type, int sectorId)
            : base(game, imageFilename, width, height, teamColor, team, alliance, health, numPilots, sectorId)
        {
            Type = type;
            Weapons = new List<Weapon>();
        }

        public override void Update()
        {
            if (!Active) return;
            base.Update();

            foreach (var wep in Weapons)
            {
                wep.Update();
            }            
        }

        public override void Draw(Graphics g, int currentSectorId)
        {
            if (!Active) return;
            base.Draw(g, currentSectorId);

            foreach (var wep in Weapons)
            {
                wep.Draw(g, currentSectorId);
            }
        }
        
        public override bool CanAttackBases()
        {
            return Type == EShipType.TroopTransport || (Weapons != null && Weapons.Exists(_ => _.GetType() == typeof(BaseLaserWeapon)));
        }

        public override bool CanAttackShips()
        {
            return Weapons != null && Weapons.Exists(_ => _.GetType() == typeof(ShipLaserWeapon));
        }
    }
}
