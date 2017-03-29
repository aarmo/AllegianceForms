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
        const string BuildImagePath = @".\Art\Animations\Build\";

        public EAsteroidType TargetRockType { get; set; }
        public EBaseType BaseType { get; set; }
        public bool Building { get; set; }
        public TimeSpan BuildingDuration { get; }
        public Image BuildImage { get; set; }
        public Asteroid Target { get; set; }

        private DateTime _buildingStart = DateTime.MinValue;
        private DateTime _buildingStop = DateTime.MaxValue;
        private int _callNext = 80;

        private int _currentBuildingGlowHalfSize = StartBuildingGlowSize / 2;

        public BuilderShip(string imageFilename, int width, int height, Color teamColor, int team, int alliance, float health, EBaseType baseType, int sectorId)
            : base(imageFilename, width, height, teamColor, team, alliance, health, 0, sectorId)
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
            var buildFilename = BuildImagePath;
            switch (TargetRockType)
            {
                case EAsteroidType.TechCarbon:
                    buildFilename += "CarbonBuildGlow.png";
                    break;
                case EAsteroidType.TechSilicon:
                    buildFilename += "SiliconBuildGlow.png";
                    break;
                case EAsteroidType.TechUranium:
                    buildFilename += "UraniumBuildGlow.png";
                    break;
                default:
                    buildFilename += "RockBuildGlow.png";
                    break;
            }
            BuildImage = Image.FromFile(buildFilename);
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

        public override void Update(int currentSectorId)
        {
            if (!Active) return;
            base.Update(currentSectorId);
            _callNext--;

            if (_buildingStop < DateTime.Now)
            {
                Active = Building = false;
                if (BaseType != EBaseType.Tower) OnShipEvent(EShipEventType.BuildingFinished);
            }
            else if (Building && _buildingStop == DateTime.MaxValue)
            {
                _buildingStart = DateTime.Now;
                _buildingStop = _buildingStart + BuildingDuration;
                OnShipEvent(EShipEventType.BuildingStarted);
            }
        }

        public override void Damage(float amount, int senderTeam)
        {
            if (Building) return;
            base.Damage(amount, senderTeam);

            if (Team == 1 && !Docked && _callNext <= 0)
            {
                _callNext = 80;
                StrategyGame.OnGameEvent(new GameAlert(SectorId, $"{Type} under attack in {StrategyGame.Map.Sectors[SectorId]}!"), EGameEventType.ImportantMessage);
                SoundEffect.Play(ESounds.vo_miner_underattack, true);
            }
        }

        public override void Draw(Graphics g, int currentSectorId)
        {
            if (!Active || !VisibleToTeam[0] || SectorId != currentSectorId) return;

            if (!Building || BaseType == EBaseType.Tower)
            {
                base.Draw(g, currentSectorId);
                return;
            }

            //Building!
            g.DrawImage(BuildImage, new Rectangle((int)_centerX - _currentBuildingGlowHalfSize, (int)_centerY - _currentBuildingGlowHalfSize, _currentBuildingGlowHalfSize * 2, _currentBuildingGlowHalfSize * 2));
            
            _currentBuildingGlowHalfSize = (int)(StrategyGame.Lerp(EndBuildingGlowSize, StartBuildingGlowSize, _buildingStart, BuildingDuration) / 2);
        }
    }
}
