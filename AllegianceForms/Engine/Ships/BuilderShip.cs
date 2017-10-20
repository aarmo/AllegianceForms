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

        public BuilderShip(StrategyGame game, string imageFilename, int width, int height, Color teamColor, int team, int alliance, float health, EBaseType baseType, int sectorId)
            : base(game, imageFilename, width, height, teamColor, team, alliance, health, 0, sectorId)
        {
            Type = EShipType.Constructor;
            BaseType = baseType;
            BuildingDuration = new TimeSpan(0, 0, 4);
            
            switch (baseType)
            {
                case EBaseType.Tactical:
                    TargetRockType = EAsteroidType.Silicon;
                    break;

                case EBaseType.Supremacy:
                    TargetRockType = EAsteroidType.Carbon;
                    break;

                case EBaseType.Expansion:
                    TargetRockType = EAsteroidType.Uranium;
                    break;

                case EBaseType.Resource:
                    TargetRockType = EAsteroidType.Resource;
                    break;

                default:
                    TargetRockType = EAsteroidType.Generic;
                    break;
            }
            var buildFilename = BuildImagePath;
            switch (TargetRockType)
            {
                case EAsteroidType.Carbon:
                    buildFilename += "CarbonBuildGlow.png";
                    break;
                case EAsteroidType.Silicon:
                    buildFilename += "SiliconBuildGlow.png";
                    break;
                case EAsteroidType.Uranium:
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
            var bse = _game.Bases.CreateBase(BaseType, Team, Colour, SectorId);
            if (bse != null)
            {
                bse.CenterX = CenterX;
                bse.CenterY = CenterY;
            }

            return bse;
        }

        public bool HasBuildSphere()
        {
            return BaseType != EBaseType.Tower && BaseType != EBaseType.Minefield;
        }

        public override void Update()
        {
            if (!Active) return;
            base.Update();
            _callNext--;

            if (_buildingStop < DateTime.Now)
            {
                Active = Building = false;
                if (HasBuildSphere()) OnShipEvent(EShipEventType.BuildingFinished);
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
                _game.OnGameEvent(new GameAlert(SectorId, $"{Type} under attack in {_game.Map.Sectors[SectorId]}!"), EGameEventType.ImportantMessage);
                SoundEffect.Play(ESounds.vo_miner_underattack, true);
            }
        }

        public override void Draw(Graphics g, int currentSectorId)
        {
            if (!Active || !VisibleToTeam[0] || SectorId != currentSectorId) return;

            if (!Building || !HasBuildSphere())
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