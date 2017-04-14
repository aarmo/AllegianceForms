using AllegianceForms.Engine;
using AllegianceForms.Engine.Ships;
using System.Drawing;

namespace AllegianceForms.Orders
{
    public class ShipOrder
    {
        public int OrderSectorId { get; set; }
        public PointF OrderPosition { get; set; }
        public PointF Offset { get; set; }
        public bool OrderComplete { get; protected set; }

        protected StrategyGame _game;

        public ShipOrder(StrategyGame game, int sectorId) : this(game, sectorId, Point.Empty, Point.Empty)
        {
        }

        public ShipOrder(StrategyGame game, int sectorId, PointF pos, PointF offset)
        {
            OrderComplete = false;
            OrderSectorId = sectorId;
            OrderPosition = pos;
            Offset = offset;
            _game = game;
        }

        public virtual void Update(Ship ship)
        {
        }

        public virtual void Cancel(Ship ship)
        {
        }

        public virtual void Draw(Graphics g, PointF fromPos, int fromSectorId)
        {
        }
    }
}
