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
            StrategyGame.LoadData();
            StrategyGame.ResetGame(GameSettings.Default());
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

                var factor = (1f * b.Health + b.ScanRange + b.Signature) / (con.Cost + con.DurationSec);
                results.Add(factor);
            }

            var diff = results.Max() - results.Min();

            diff.ShouldBeLessThan(0.1f);
        }

        [TestMethod]
        public void ShipStatsAreBalanced()
        {
            var ships = StrategyGame.Ships.Ships;
            var results = new List<double>();

            foreach (var s in ships)
            {
                if (s.Type == EShipType.Lifepod || s.Type == EShipType.Constructor || s.Weapons == null) continue;

                var reqTech = new List<TechItem>();
                AddAllReqTech(s, reqTech);

                var weaponFactor = Math.Abs((1f * s.Weapons.Sum(_ => _.WeaponDamage) + s.Weapons.Sum(_ => _.WeaponRange)) 
                    / (s.Weapons.Sum(_ => _.ShootingDelay.TotalMilliseconds) + s.Weapons.Sum(_ => _.ShootingDuration.TotalMilliseconds)));

                var factor = (1f * s.Health + s.ScanRange + s.Signature - s.NumPilots + s.Speed) 
                    / (reqTech.Sum(_ => _.Cost) + reqTech.Sum(_ => _.DurationSec));

                results.Add(factor + weaponFactor);
            }
            
            var diff = results.Max() - results.Min();
            diff.ShouldBeLessThan(0.5f);
        }

        private void AddAllReqTech(ShipSpec ship, List<TechItem> tech)
        {
            if (ship.DependsOnTechIds == null || ship.DependsOnTechIds.Length == 0)
                return;

            var allTech = StrategyGame.TechTree[0].TechItems;
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
    }
}
