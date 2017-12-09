using System.Drawing;

namespace AllegianceForms.Engine.Map
{
    public class Wormhole
    {
        public MapSector Sector1 { get; set; }
        public MapSector Sector2 { get; set; }

        public GameEntity End1 { get; set; }
        public GameEntity End2 { get; set; }

        private StrategyGame _game;

        public Wormhole(StrategyGame game, MapSector s1, MapSector s2)
        {
            _game = game;
            Sector1 = s1;
            Sector2 = s2;
            End1 = new GameEntity(game, ".\\Art\\wormhole.png", 50, 50, s1.Id) { Name = s2.Name, TextBrush = Brushes.CornflowerBlue };
            End2 = new GameEntity(game, ".\\Art\\wormhole.png", 50, 50, s2.Id) { Name = s1.Name, TextBrush = Brushes.CornflowerBlue };
        }

        public void SetVisibleToTeam(int team, bool visible)
        {
            var t = team - 1;
            if (t < 0 || t >= _game.NumTeams) return;

            Sector1.SetVisibleToTeam(t, visible);
            Sector2.SetVisibleToTeam(t, visible);
            End1.SetVisibleToTeam(t, visible);
            End2.SetVisibleToTeam(t, visible);
        }

        public virtual void Draw(Graphics g, int sectorId)
        {
            if (sectorId == Sector1.Id)
            {
                End1.Draw(g, sectorId);
            }
            if (sectorId == Sector2.Id)
            {
                End2.Draw(g, sectorId);
            }
        }

        public bool LinksTo(MapSector sector)
        {
            if (sector == null) return false;

            return Sector1 == sector || Sector2 == sector;
        }
    }
}
