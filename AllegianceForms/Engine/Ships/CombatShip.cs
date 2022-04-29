using AllegianceForms.Engine.Weapons;
using System.Collections.Generic;
using System.Drawing;

namespace AllegianceForms.Engine.Ships
{
    public class CombatShip : Ship
    {
        public List<Weapon> Weapons { get; set; }
        public Dictionary<EAbilityType, Ability> Abilities { get; protected set; }
        public Pen TeamPen { get; protected set; }

        // Track abilities things
        private float _prevCenterX, _prevCenterY, _lastCenterX, _lastCenterY;
        private bool _boostWeapons;
        private float _boostWeaponsAmount = 1f;

        public CombatShip(StrategyGame game, string imageFilename, int width, int height, Color teamColor, int team, int alliance, float health, int numPilots, EShipType type, int sectorId)
            : base(game, imageFilename, width, height, teamColor, team, alliance, health, numPilots, sectorId)
        {
            Type = type;
            Weapons = new List<Weapon>();

            TeamPen = new Pen(Colour.AdjustAlpha(StrategyGame.AbilityPenAlpha), StrategyGame.AbilityPenWidth);

            // Prepare the abilities this ship can use, adjusted with the current upgrades
            Abilities = new Dictionary<EAbilityType, Ability>();

            var t = team - 1;
            var cooldown = game.TechTree[t].ResearchedUpgrades[EGlobalUpgrade.AbilityCooldown];
            var duration = game.TechTree[t].ResearchedUpgrades[EGlobalUpgrade.AbilityDuration];
            var effect = game.TechTree[t].ResearchedUpgrades[EGlobalUpgrade.AbilityEffect];

            foreach (var at in game.GetEnabledAbilities(team, type))
            {
                var ab = new Ability(at, cooldown, duration, effect);
                Abilities.Add(at, ab);

                if (at == EAbilityType.WeaponBoost) _boostWeaponsAmount = ab.AbilityEffectMultiplier;

                // Setup what happens to this ship when the ability is triggered & completes
                // (Rapid Fire, Weapon Boost, Engine Boost are handled while active, not toggled on/off)

                if (at == EAbilityType.EngineBoost) 
                {
                    ab.AbilityStarted += (Ability a) =>
                    {
                        Speed *= a.AbilityEffectMultiplier;
                    };
                    ab.AbilityFinished += (Ability a) =>
                    {
                        Speed /= a.AbilityEffectMultiplier;
                    };
                }
                else if (at == EAbilityType.ShieldBoost)
                {
                    ab.AbilityStarted += (Ability a) =>
                    {
                        Shield += MaxShield * (a.AbilityEffectMultiplier-1);
                        if (Shield > MaxShield) Shield = MaxShield;
                    };
                }
                else if (at == EAbilityType.ScanBoost)
                {
                    ab.AbilityStarted += (Ability a) =>
                    {
                        ScanRange *= a.AbilityEffectMultiplier;
                    };
                    ab.AbilityFinished += (Ability a) =>
                    {
                        ScanRange /= a.AbilityEffectMultiplier;
                    };
                }
                else if (at == EAbilityType.StealthBoost)
                {
                    ab.AbilityStarted += (Ability a) =>
                    {
                        Signature /= a.AbilityEffectMultiplier;
                    };
                    ab.AbilityFinished += (Ability a) =>
                    {
                        Signature *= a.AbilityEffectMultiplier;
                    };
                }
            }
        }

        public override void Update()
        {
            if (!Active) return;

            UpdateAbilities();

            base.Update();

            foreach (var wep in Weapons)
            {
                wep.Update(_boostWeapons ? _boostWeaponsAmount : 1f);
            }
        }

        public override void Draw(Graphics g, int currentSectorId)
        {
            if (!Active) return;

            base.Draw(g, currentSectorId);

            DrawAbilities(g, currentSectorId);

            foreach (var wep in Weapons)
            {
                wep.Draw(g, currentSectorId, _boostWeapons);
            }
        }

        private void UpdateAbilities()
        {
            _boostWeapons = AbilityIsActive(EAbilityType.WeaponBoost);

            foreach (var a in Abilities.Keys)
            {
                if (Abilities[a].IsActive())
                {
                    if (a == EAbilityType.HullRepair)
                    {
                        Damage(-MaxHealth / 100, Team);
                    }
                    else if (a == EAbilityType.EngineBoost)
                    {
                        _prevCenterX = _centerX;
                        _prevCenterY = _centerY;
                        _lastCenterX = _prevCenterX;
                        _lastCenterY = _prevCenterY;
                    }
                    else if (a == EAbilityType.RapidFire)
                    {
                        // Update weapons again to effectively reduce delay by half, which means it's not affected by the Ability Effect upgrades!
                        foreach (var wep in Weapons)
                        {
                            wep.Update(_boostWeapons ? _boostWeaponsAmount : 1f);
                        }
                    }
                }
            }
            
        }

        private void DrawAbilities(Graphics g, int currentSectorId)
        {
            if (!Active || !VisibleToTeam[0] || SectorId != currentSectorId) return;

            var currentOff = 0f;
            var currentOffTwo = 0f;
            var bounds = Rectangle.Empty;

            foreach (var a in Abilities.Keys)
            {
                if (Abilities[a].IsActive())
                {
                    if (a == EAbilityType.WeaponBoost || a == EAbilityType.ShieldBoost)
                    {
                        continue;
                    }

                    var pen = a == EAbilityType.RapidFire ? TeamPen : StrategyGame.AbilityPens[a];

                    if (a == EAbilityType.EngineBoost)
                    {
                        g.DrawLine(pen, _centerX, _centerY, _lastCenterX, _lastCenterY);
                    }
                    else
                    {
                        if (bounds == Rectangle.Empty) bounds = BoundsI;

                        // Draw smaller and smaller rects with a certain colour
                        g.DrawRectangle(pen, bounds.Left + currentOff, bounds.Top + currentOff, bounds.Width - currentOffTwo, bounds.Height - currentOffTwo);

                        currentOff += StrategyGame.AbilityPenWidth;
                        currentOffTwo = currentOff * 2;
                    }
                }
            }
        }

        public override bool CanAttackBases() => 
            Type == EShipType.TroopTransport || Type == EShipType.Bomber
            || Type == EShipType.FighterBomber || Type == EShipType.StealthBomber 
            || Type == EShipType.Frigate || Type == EShipType.Cruiser 
            || Type == EShipType.Battlecruiser;

        public override bool CanAttackShips() => 
            Type != EShipType.Lifepod && Type != EShipType.Constructor 
            && Type != EShipType.Miner && Type != EShipType.None;

        public bool CanUseAbility(EAbilityType type) => 
            Abilities.ContainsKey(type) && Abilities[type].IsReady();

        public bool UseAbility(EAbilityType type) => 
            Abilities.ContainsKey(type) && Abilities[type].Activate();

        public bool AbilityIsActive(EAbilityType type) => 
            Abilities.ContainsKey(type) && Abilities[type].IsActive();

    }
}
