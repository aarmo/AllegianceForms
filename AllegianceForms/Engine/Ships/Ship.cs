using AllegianceForms.Orders;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace AllegianceForms.Engine.Ships
{
    public class Ship : GameEntity
    {
        public delegate void ShipEventHandler(Ship sender, EShipEventType e);
        public event ShipEventHandler ShipEvent;

        public EShipType Type { get; set; }
        public int Team { get; protected set; }
        public Brush TeamColor { get; protected set; }
        public Color Colour { get; protected set; }
        public bool Selected { get; set; }
        public Pen SelectedPen { get; set; }        
        public EVertDir VerticalDir { get; set; }
        public EHorDir HorizontalDir { get; set; }
        public float Speed { get; set; }
        public int Health { get; set; }
        public List<ShipOrder> Orders { get; private set; }
        public ShipOrder CurrentOrder { get; private set; }
        public int NumPilots { get; set; }
        public int ScanRange { get; set; }
        public bool Docked { get; set; }
        public int MaxHealth { get; set; }
        

        public Ship(string imageFilename, int width, int height, Color teamColor, int team, int health, int numPilots, int sectorId)
            : base(imageFilename, width, height, sectorId)
        {
            if (Image != null)
            {
                var bmp = (Bitmap)Image;

                // Set the image's team color
                for (var x = 0; x < bmp.Width; x++)
                {
                    for (var y = 0; y < bmp.Height; y++)
                    {
                        var c = bmp.GetPixel(x, y);
                        if (c.A != 0) bmp.SetPixel(x, y, teamColor);
                    }
                }
            }

            Active = true;
            Team = team;
            Colour = teamColor;
            TeamColor = new SolidBrush(teamColor);
            SelectedPen = new Pen(teamColor, 1) { DashStyle = DashStyle.Dot };
            NumPilots = numPilots;
            VisibleToTeam[team - 1] = true;
            ScanRange = 100;
            MaxHealth = Health = health;

            VerticalDir = EVertDir.None;
            HorizontalDir = EHorDir.None;
            Speed = 0;

            Orders = new List<ShipOrder>();
        }

        public virtual void Update()
        {
            if (!Active) return;

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

            if (CurrentOrder == null) StopMoving();

            Move();
        }

        public override void Draw(Graphics g)
        {
            base.Draw(g);
            if (Selected) g.DrawRectangle(SelectedPen, _left - 2, _top - 2, Image.Width + 4, Image.Height + 4);
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

        public virtual void Dock()
        {
            if (Docked) return;
            
            Health = MaxHealth;

            if (Type != EShipType.Miner && Type != EShipType.Constructor && Orders.Count == 0)
            {
                // we are done for now
                Active = false;
                StrategyGame.DockShip(Team, NumPilots);
            }
            
            if (Orders.Count == 0)
            {
                Docked = true;
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

        public virtual void Damage(int amount)
        {
            if (!Active || Docked) return;

            if (Health - amount <= 0)
            {
                // Dead!
                Health = 0;
                Active = false;
                OnShipEvent(EShipEventType.ShipDestroyed);
            }
            else if (Health - amount >= MaxHealth)
            {
                Health = MaxHealth;
            }
            else
            {
                Health -= amount;
            }
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

        protected void OnShipEvent(EShipEventType e)
        {
            if (ShipEvent != null) ShipEvent(this, e);
        }
    }
}