using System;
using System.Drawing;

namespace AllegianceForms.Engine.Rocks
{

    public class ResourceAsteroid : Asteroid
    {
        public bool BeingMined { get; set; }
        public static new string[] Images = new[] { "helium_1.png", "helium_2.png", "helium_3.png", "helium_4.png" };

        public int AvailableResources { get; set; }
        public const int MaxResources = 3000;

        public ResourceAsteroid(StrategyGame game, Random r, int width, int height, int sectorId)
            : base(game, StrategyGame.RockPicDir + Images[r.Next(0, Images.Length)], width, height, sectorId)
        {
            Type = EAsteroidType.Resource;
            AvailableResources = (int)(MaxResources * _game.GameSettings.ResourcesPerRockMultiplier);
        }

        public int Mine(int amount)
        {
            if (AvailableResources - amount < 0)
            {
                    amount = AvailableResources;
            }

            AvailableResources -= amount;
            return amount;
        }

        public void Regenerate(int amount)
        {
            if (AvailableResources == MaxResources) return;

            if (AvailableResources + amount < MaxResources)
            {
                AvailableResources += amount;
            }
            else
            {
                AvailableResources = MaxResources;
            }
        }

        public override void Draw(Graphics g, int currentSectorId)
        {
            if (!Active || !VisibleToTeam[0] || SectorId != currentSectorId) return;

            base.Draw(g, currentSectorId);            
            DrawResourceBar(g);
        }

        protected void DrawResourceBar(Graphics g)
        {
            var b = Bounds;
            var p = b.Width * (1f * AvailableResources / MaxResources);
            g.FillRectangle(StrategyGame.ResourceBrush, b.Left, b.Bottom + 3, p, 3);
            g.DrawRectangle(StrategyGame.HealthBorderPen, b.Left, b.Bottom + 3, b.Width, 3);
        }
    }
}
