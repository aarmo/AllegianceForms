using AllegianceForms.Engine.Bases;
using AllegianceForms.Orders;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AllegianceForms.Engine.Ships
{
    public class Ship : GameUnit
    {
        public delegate void ShipEventHandler(Ship sender, EShipEventType e);
        public event ShipEventHandler ShipEvent;

        public EShipType Type { get; set; }
        public int Alliance { get; protected set; }
        public Color Colour { get; protected set; }
        public bool Selected { get; set; }     
        public EVertDir VerticalDir { get; set; }
        public EHorDir HorizontalDir { get; set; }
        public float Speed { get; set; }
        public List<ShipOrder> Orders { get; private set; }
        public ShipOrder CurrentOrder { get; private set; }
        public int NumPilots { get; set; }
        public float ScanRange { get; set; }
        public bool Docked { get; private set; }
        public Base DockedAtBase { get; private set; }

        public Ship(StrategyGame game, string imageFilename, int width, int height, Color teamColor, int team, int alliance, float health, int numPilots, int sectorId)
            : base(game, imageFilename, width, height, health, sectorId, team)
        {
            if (Image != null) Utils.ReplaceColour((Bitmap)Image, teamColor);

            Active = true;
            Alliance = alliance;
            Colour = teamColor;
            NumPilots = numPilots;
            if (team > 0) VisibleToTeam[team - 1] = true;
            ScanRange = 100;
            MaxHealth = Health = health;

            VerticalDir = EVertDir.None;
            HorizontalDir = EHorDir.None;
            Speed = 0;

            Orders = new List<ShipOrder>();
        }

        public override void Update()
        {
            if (!Active) return;
            base.Update();

            if (CurrentOrder != null && CurrentOrder.OrderComplete)
            {
                CurrentOrder = null;
            }
            else if (CurrentOrder == null && Orders.Count > 0)
            {
                CurrentOrder = Orders.First();
                Orders.Remove(CurrentOrder);
            }
            else if (CurrentOrder != null)
            {
                CurrentOrder.Update(this);                
            }
            else if (CurrentOrder == null) StopMoving();

            if (Docked && DockedAtBase != null && (!DockedAtBase.Active || DockedAtBase.Team != Team))
            {
                Docked = false;
                DockedAtBase = null;
            }

            Move();
        }

        public override void Draw(Graphics g, int currentSectorId)
        {
            if (!Active || !VisibleToTeam[0] || SectorId != currentSectorId) return;
            base.Draw(g, currentSectorId);

            var t = Team - 1;
            var b = BoundsI;
            DrawHealthBar(g, b);

            if (t == 0 && Selected) g.DrawRectangle(_game.SelectedPens[t], b.Left - 1, b.Top - 1, b.Width + 2, b.Height + 2);
        }

        public void StopMoving()
        {
            VerticalDir = EVertDir.None;
            HorizontalDir = EHorDir.None;
        }

        public virtual void Stop()
        {
            StopMoving();
            CurrentOrder = null;
            Orders.Clear();
        }

        public virtual void Dock(Base dockedBase)
        {
            if (Docked) return;

            Health = MaxHealth;

            if (CanDock(Type) && Orders.Count == 0)
            {
                // we are done for now
                Active = false;
                _game.DockPilots(Team, NumPilots);
            }
            
            if (Orders.Count == 0)
            {
                Docked = true;
                DockedAtBase = dockedBase;
            }
        }

        private void Move()
        {
            if (Speed == 0) return;

            // Standard speed if just Hor/Ver, otherwise moving at a 45deg angle, so adjust with the Sqrt(2)
            var s = Speed;
            if (VerticalDir != EVertDir.None && HorizontalDir != EHorDir.None) s = (float)(Speed * StrategyGame.SqrtTwo / 2);

            if (Docked && VerticalDir != EVertDir.None || HorizontalDir != EHorDir.None) Docked = false;

            switch (VerticalDir)
            {
                case EVertDir.North:
                    Top -= s;
                    break;
                case EVertDir.South:
                    Top += s;
                    break;
            }

            switch (HorizontalDir)
            {
                case EHorDir.West:
                    Left -= s;
                    break;
                case EHorDir.East:
                    Left += s;
                    break;
            }
        }

        public override void Damage(float amount, Weapons.Weapon source)
        {
            if (!Active || Docked) return;
            base.Damage(amount, source);

            if (!Active && Health == 0)
            {
                // Dead!
                OnShipEvent(EShipEventType.ShipDestroyed);
            }
        }

        public virtual void InsertOrder(ShipOrder order)
        {
            if (CurrentOrder != null) Orders.Insert(0, CurrentOrder);
            CurrentOrder = order;
        }

        public virtual void OrderShip(ShipOrder order, bool append = false)
        {
            if (append)
            {
                // Can't append orders after a Patrol/Build!
                if (CurrentOrder != null 
                    && (CurrentOrder is PatrolOrder 
                        || CurrentOrder is BuildOrder 
                        || (Orders.Count > 0 && (Orders.Last() is PatrolOrder || Orders.Last() is BuildOrder))))
                    return;

                // Don't add an order that is already complete (dock replaced)
                if (!order.OrderComplete) Orders.Add(order);
            }
            else
            {
                if (CurrentOrder != null) CurrentOrder.Cancel(this);

                if (!order.OrderComplete)
                {
                    Orders.Clear();
                    CurrentOrder = order;
                }
            }
        }

        public virtual bool CanAttackBases()
        {
            return false;
        }

        public virtual bool CanAttackShips()
        {
            return false;
        }

        public static bool CanDock(EShipType type)
        {
            return type != EShipType.Miner && type != EShipType.Constructor && !IsCapitalShip(type) && !IsDroneShip(type) && !IsTower(type);
        }

        public static bool IsDroneShip(EShipType type)
        {
            return type == EShipType.DroneBomber || type == EShipType.DroneScout
                || type == EShipType.DroneFighter || type == EShipType.DroneInterceptor
                || type == EShipType.DroneGunship || type == EShipType.DroneStealthFighter
                || type == EShipType.DroneStealthBomber;
        }

        public static bool IsCapitalShip(EShipType type)
        {
            return type == EShipType.Battlecruiser || type == EShipType.Battleship
                || type == EShipType.Corvette || type == EShipType.Cruiser
                || type == EShipType.Destroyer || type == EShipType.Devastator
                || type == EShipType.Frigate || type == EShipType.Support
                || type == EShipType.AdvancedSupport || type == EShipType.HeavySupport;
        }

        public static bool IsNonCapitalShip(EShipType type)
        {
            return type == EShipType.Bomber || type == EShipType.Scout
                || type == EShipType.Fighter || type == EShipType.FighterBomber
                || type == EShipType.StealthBomber || type == EShipType.StealthFighter
                || type == EShipType.Interceptor || type == EShipType.Gunship
                || type == EShipType.TroopTransport;
        }

        public static bool IsTower(EShipType type)
        {
            return type == EShipType.Tower || type == EShipType.ShieldTower
                || type == EShipType.MissileTower || type == EShipType.RepairTower;
        }

        protected void OnShipEvent(EShipEventType e)
        {
            if (ShipEvent != null) ShipEvent(this, e);
        }
    }
}