using AllegianceForms.Engine;
using AllegianceForms.Engine.Bases;
using AllegianceForms.Engine.Ships;
using System.Drawing;

namespace AllegianceForms.Orders
{
    public class RefineOrder : MoveOrder
    {
        private Base _target;

        public RefineOrder(StrategyGame game, MinerShip ship, Base baseTarget) : base(game, ship.SectorId)
        {
            OrderPen.Color = Color.DarkGoldenrod;

            // Find the closest friendly station we can refine at
            _target = baseTarget;

            if (_target != null)
                OrderPosition = _target.CenterPoint;
        }

        public override void Update(Ship ship)
        {
            var miner = ship as MinerShip;
            if (miner == null)
            {
                OrderComplete = true;
                return;
            }

            base.Update(ship);

            if (OrderComplete && _target != null)
            {
                // Offload miner (no repair)
                miner.Refine();

                // Next Order will be followed!
            }
        }
    }
}
