using AllegianceForms.Engine.Bases;
using AllegianceForms.Engine.Rocks;
using System;
using System.Drawing;
using AllegianceForms.Orders;

namespace AllegianceForms.Engine.Ships
{
    public class BuilderShip : Ship
    {
        public const int StartBuildingGlowSize = 32;
        public const int EndBuildingGlowSize = 128;

        public EAsteroidType TargetRockType { get; set; }
        public EBaseType BaseType { get; set; }
        public bool Building { get; set; }
        public TimeSpan BuildingDuration { get; }
        public Image BuildImage { get; set; }
        public Asteroid Target { get; set; }

        private DateTime _buildingStart = DateTime.MinValue;
        private DateTime _buildingStop = DateTime.MaxValue;
        private DateTime _callNext = DateTime.MinValue;

        private int _currentBuildingGlowHalfSize = StartBuildingGlowSize / 2;

        public BuilderShip(string imageFilename, int width, int height, Color teamColor, int team, int health, EBaseType baseType, int sectorId)
            : base(imageFilename, width, height, teamColor, team, health, 0, sectorId)
        {
            Type = EShipType.Constructor;
            BaseType = baseType;
            BuildingDuration = new TimeSpan(0, 0, 4);

            switch (baseType)
            {
                case EBaseType.Tactical:
                    TargetRockType = EAsteroidType.TechSilicon;
                    break;

                case EBaseType.Supremacy:
                    TargetRockType = EAsteroidType.TechCarbon;
                    break;

                case EBaseType.Expansion:
                    TargetRockType = EAsteroidType.TechUranium;
                    break;

                default:
                    TargetRockType = EAsteroidType.Rock;
                    break;
            }

            var buildImageFilename = @".\Art\Animations\Build\";
            switch (TargetRockType)
            {
                case EAsteroidType.TechCarbon:
                    buildImageFilename += "CarbonBuildGlow.png";
                    break;
                case EAsteroidType.TechSilicon:
                    buildImageFilename += "SiliconBuildGlow.png";
                    break;
                case EAsteroidType.TechUranium:
                    buildImageFilename += "UraniumBuildGlow.png";
                    break;
                default:
                    buildImageFilename += "RockBuildGlow.png";
                    break;
            }
            BuildImage = Image.FromFile(buildImageFilename);
        }

        public override void OrderShip(ShipOrder order, bool append = false)
        {
            if (Building) return;

            base.OrderShip(order, append);
        }

        public Base GetFinishedBase()
        {
            var bse = StrategyGame.Bases.CreateBase(BaseType, Team, Colour, SectorId);
            if (bse != null)
            {
                bse.CenterX = CenterX;
                bse.CenterY = CenterY;
            }

            return bse;
        }

        public override void Update()
        {
            if (!Active) return;
            base.Update();

            if (_buildingStop < DateTime.Now)
            {
                Active = Building = false;
                OnShipEvent(EShipEventType.BuildingFinished);
            }
            else if (Building && _buildingStop == DateTime.MaxValue)
            {
                _buildingStart = DateTime.Now;
                _buildingStop = _buildingStart + BuildingDuration;
                OnShipEvent(EShipEventType.BuildingStarted);
            }
        }

        public override void Damage(int amount)
        {
            if (Building) return;

            base.Damage(amount);

            if (Team == 1 && !Docked && DateTime.Now > _callNext)
            {
                SoundEffect.Play(ESounds.vo_miner_underattack);
                _callNext = DateTime.Now.AddSeconds(2);
            }
        }

        public override void Draw(Graphics g)
        {
            if (!Active) return;
            if (!Building)
            {
                base.Draw(g);
                return;
            }

            //Building!
            g.DrawImage(BuildImage, new Rectangle((int)_centerX - _currentBuildingGlowHalfSize, (int)_centerY - _currentBuildingGlowHalfSize, _currentBuildingGlowHalfSize * 2, _currentBuildingGlowHalfSize * 2));
            
            _currentBuildingGlowHalfSize = (int)(StrategyGame.Lerp(EndBuildingGlowSize, StartBuildingGlowSize, _buildingStart, BuildingDuration) / 2);
        }
    }
}
