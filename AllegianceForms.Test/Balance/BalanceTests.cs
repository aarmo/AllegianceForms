using AllegianceForms.Engine;
using AllegianceForms.Engine.Ships;
using AllegianceForms.Engine.Tech;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AllegianceForms.Test.Balance
{
    [TestClass]
    public class BalanceTests
    {
        [TestInitialize]
        public void Setup()
        {
            StrategyGame.SetupGame(GameSettings.Default());
            StrategyGame.LoadData();            
        }

        [TestMethod]
        public void BaseStatsAreBalanced()
        {
            var bases = StrategyGame.Bases.Bases;
            var tech = StrategyGame.TechTree[0].TechItems;
            var results = new List<float>();

            foreach (var b in bases)
            {
                var name = b.Type.ToString() + " Constructor";
                var con = tech.FirstOrDefault(_ => _.Type == ETechType.Construction && _.Name == name);
                if (con == null) continue; 

                var factor = (1f * b.Health + b.ScanRange + b.Signature) / (con.Cost + con.DurationTicks);
                results.Add(factor);
            }

            var diff = results.Max() - results.Min();

            diff.ShouldBeLessThan(0.1f);
        }

        [TestMethod]
        public void ShipStatsAreBalanced()
        {
            var settings = StrategyGame.GameSettings;
            var ships = StrategyGame.Ships.Ships;
            var results = new List<BalanceStat>();

            foreach (var s in ships)
            {
                if (s.Type == EShipType.Lifepod || s.Type == EShipType.Constructor || s.Weapons == null) continue;

                var reqTech = new List<TechItem>();
                AddAllReqTech(s, reqTech);
                
                var b = new BalanceStat()
                {
                    RequiredTech = reqTech,
                    Spec = s,
                    Type = s.Type,
                    TotalShootingDelay = s.Weapons.Sum(_ => _.ShootingDelayTicks) * 50f,
                    TotalShootingDuration = s.Weapons.Sum(_ => _.ShootingTicks) * 50f,
                    TotalTechCost = reqTech.Sum(_ => _.Cost),
                    TotalTechDuration = reqTech.Sum(_ => _.DurationTicks),
                    TotalWeaponDamage = s.Weapons.Sum(_ => _.WeaponDamage),
                    TotalWeaponRange = s.Weapons.Sum(_ => _.WeaponRange)
                };

                var weaponFactor = Math.Abs((b.TotalWeaponDamage + b.TotalWeaponRange)
                    / (b.TotalShootingDelay + b.TotalShootingDuration));

                var numbersAvailable = (Ship.IsCapitalShip(s.Type) ? settings.CapitalMaxDrones : (s.NumPilots > 0 ? settings.NumPilots / s.NumPilots : settings.ConstructorsMaxTowerDrones));

                b.Factor = weaponFactor + ((s.Health + s.ScanRange + s.Signature + s.Speed) * numbersAvailable / (b.TotalTechCost + b.TotalTechDuration));

                results.Add(b);
            }
            results = results.OrderByDescending(_ => _.Factor).ToList();
            var diff = results.Max(_ => _.Factor) - results.Min(_ => _.Factor);

            results.Count.ShouldBeGreaterThan(0);
            diff.ShouldBeLessThan(0.4f);
        }

        private void AddAllReqTech(ShipSpec ship, List<TechItem> tech)
        {
            if (ship.DependsOnTechIds == null || ship.DependsOnTechIds.Length == 0)
                return;
            var allTech = StrategyGame.TechTree[0].TechItems;
            if (Ship.IsCapitalShip(ship.Type))
            {
                var capTech = allTech.FirstOrDefault(_ => _.Type == ETechType.ShipyardConstruction && _.Name == ship.Type.ToString());
                if (capTech != null) tech.Add(capTech);
            }
            var shipTech = (from t in allTech
                            where ship.DependsOnTechIds.Contains(t.Id)
                            && !tech.Any(_ => _.Id == t.Id)
                            select t).ToList();

            tech.AddRange(shipTech);

            foreach (var t in shipTech)
            {
                AddAllReqTech(t, tech);
            }
        }

        private void AddAllReqTech(TechItem item, List<TechItem> tech)
        {
            var allTech = StrategyGame.TechTree[0].TechItems;
            if (item.Type == ETechType.Base)
            {
                var name = item.Name + " Constructor";
                var con = allTech.FirstOrDefault(_ => _.Type == ETechType.Construction && _.Name == name);
                tech.Add(con);
            }

            if (item.DependsOnIds == null || item.DependsOnIds.Length == 0)
                return;

            var newTech = (from t in allTech
                           where item.DependsOnIds.Contains(t.Id)
                           && !tech.Any(_ => _.Id == t.Id)
                           select t).ToList();

            tech.AddRange(newTech);

            foreach (var t in newTech)
            {
                AddAllReqTech(t, tech);
            }
        }
        
        private class BalanceStat
        {
            public double TotalWeaponDamage;
            public double TotalWeaponRange;
            public double TotalShootingDelay;
            public double TotalShootingDuration;
            public double TotalTechCost;
            public double TotalTechDuration;

            public ShipSpec Spec;
            public EShipType Type;
            public List<TechItem> RequiredTech;

            public double Factor;
        }
    }
}
