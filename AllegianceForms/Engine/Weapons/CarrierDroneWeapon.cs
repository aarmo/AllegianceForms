using AllegianceForms.Engine.Ships;
using AllegianceForms.Orders;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AllegianceForms.Engine.Weapons
{
    public class CarrierDroneWeapon : ShipWeapon
    {
        private int _droneShipId;
        private int _droneCost;
        private bool _dronesLaunched = false;
        private GameEntity _droneTarget;
        private ShipSpec _droneShipSpec;
        private bool _droneSurroundShooter = false;
        private int _droneSurroundDistance = 100;

        private List<CombatShip> _drones = new List<CombatShip>();

        public CarrierDroneWeapon(StrategyGame game, int fireTicks, int refireTicks, float range, float damage, Ship shooter, PointF offset, int droneShipId, int droneCost)
            : base(game, fireTicks, refireTicks, range, damage, shooter, offset)
        {
            _droneShipId = droneShipId;
            _droneCost = droneCost;

            var race = game.Faction[Shooter.Team - 1].Race;
            _droneShipSpec = game.Ships.RaceShips[race].FirstOrDefault(_ => _.Id == _droneShipId);
            if (_droneShipSpec == null) throw new Exception($"Unknown Carrier Drone Ship Id: {_droneShipId} (Race: {race}, Team: {Shooter.Team})");

            if (_droneShipSpec.Weapons.Count > 0)
            {
                if (_droneShipSpec.Weapons[0] is ShipLaserWeapon w)
                {
                    _droneSurroundDistance = (int)(w.WeaponRange * 0.75f);
                    _droneSurroundShooter = w is NanLaserWeapon || w is ShieldChargeWeapon;
                }
            }

            for (var i = 0; i < damage; i++)
            {
                var d = _game.Ships.CreateShip(_droneShipSpec, Shooter.Team, Shooter.Colour, Shooter.SectorId);
                if (d != null) _drones.Add(d);
            }
        }

        public override void Draw(Graphics g, int currentSectorId, bool boosted)
        {
            // We take care of our drones here
            for (var i = 0; i < _drones.Count; i++)
            {
                var d = _drones[i];
                if (d.Health > 0)
                {
                    if (d.Active)
                    { 
                        d.Update();
                        d.Draw(g, currentSectorId);
                    }
                }
                else
                {
                    d.Active = false;
                    _drones.Remove(d);
                }
            }
        }

        public override void DamageTarget(float boostAmount)
        {
            // Instead of shooting, re-build our drones if we can afford them
            if (_drones.Count < WeaponDamage)
            {
                if (_game.Credits[Shooter.Team - 1] >= _droneCost)
                {
                    _game.SpendCredits(Shooter.Team, _droneCost);
                    var d = _game.Ships.CreateShip(_droneShipSpec, Shooter.Team, Shooter.Colour, Shooter.SectorId);
                    if (d != null)
                    {
                        d.CenterX = Shooter.CenterX + FireOffset.X;
                        d.CenterY = Shooter.CenterY + FireOffset.Y;
                        _drones.Add(d);

                        if (_dronesLaunched)
                        {
                            d.OrderShip(new SurroundOrder(_game, Shooter.SectorId, _droneSurroundShooter ? Shooter : Target, _droneSurroundDistance));
                        }
                        else
                        {
                            d.Active = false;
                        }
                    }
                }
            }
        }

        public override void CheckForANewTarget()
        {
            base.CheckForANewTarget();

            if (Target == null && _dronesLaunched)
            {
                DockDrones();
            }
            else if (!_dronesLaunched || _droneTarget != Target)
            {
                LaunchDrones();
            }
        }

        private void LaunchDrones()
        {
            _dronesLaunched = true;
            _droneTarget = Target;

            foreach (var d in _drones)
            {
                if (!d.Active)
                {
                    d.CenterX = Shooter.CenterX + FireOffset.X;
                    d.CenterY = Shooter.CenterY + FireOffset.Y;
                    d.Active = true;
                }
                d.OrderShip(new SurroundOrder(_game, Shooter.SectorId, _droneSurroundShooter ? Shooter : Target, _droneSurroundDistance));
            }
        }

        private void DockDrones()
        {
            _dronesLaunched = false;
            foreach (var d in _drones)
            {
                if (d.Active) d.OrderShip(new InterceptOrder(_game, Shooter, Shooter.SectorId));
            }
        }
    }
}