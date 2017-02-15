using AllegianceForms.Engine.Ships;
using AllegianceForms.Orders;
using System;
using System.Collections.Generic;

namespace AllegianceForms.AI.Missions
{
    public class CommanderMission
    {
        public List<Ship> IncludedShips { get; set; }
        public CommanderAI AI { get; set; }
        public Sector UI { get; set; }

        public List<DateTime> RecentOrders { get; set; }

        protected bool _completed = false;
        protected int _recentOrderDelaySecs = 30;

        public CommanderMission(CommanderAI ai, Sector ui)
        {
            AI = ai;
            UI = ui;
            IncludedShips = new List<Ship>();
            RecentOrders = new List<DateTime>();
        }
        
        public virtual void UpdateMission()
        {
            IncludedShips.RemoveAll(_ => !_.Active);
            var cutOffTime = DateTime.Now.AddSeconds(_recentOrderDelaySecs);
            RecentOrders.RemoveAll(_ => _ > cutOffTime);

            var stillCompleted = MissionComplete();

            if (stillCompleted && !_completed)
            {
                _completed = true;
                IncludedShips.ForEach(_ => _.OrderShip(new DockOrder(_)));
                IncludedShips.Clear();
            }
            else if (_completed && !stillCompleted)
            {
                _completed = false;
            }

        }

        protected void LogOrder()
        {
            RecentOrders.Add(DateTime.Now);
        }

        public virtual bool RequireMorePilots()
        {
            return false;
        }

        public virtual void AddMorePilots()
        {
        }

        public virtual bool MissionComplete()
        {
            return false;
        }
    }
}
