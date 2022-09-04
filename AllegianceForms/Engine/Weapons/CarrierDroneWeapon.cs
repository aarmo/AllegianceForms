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
        public int DroneShipId { get; set; }
        public int DroneCost { get; set; }

        private bool _dronesLaunched = false;
        private GameEntity _droneTarget = null;
        private ShipSpec _droneShipSpec;
        private bool _droneSurroundShooter = false;
        private int _droneSurroundDistance = 100;
        private bool _initialised = false;
        private List<CombatShip> _drones;

        public CarrierDroneWeapon(StrategyGame game, int fireTicks, int refireTicks, float range, float damage, Ship shooter, PointF offset, int droneShipId, int droneCost)
            : base(game, fireTicks, refireTicks, range, damage, shooter, offset)
        {
            DroneShipId = droneShipId;
            DroneCost = droneCost;
        }

        public override void Draw(Graphics g, int currentSectorId, bool boosted)
        {
            // Nothing to draw for now... TODO: Draw dots for available drones?
        }

        public override void DamageTarget(float boostAmount)
        {
            // Instead of shooting, re-build our drones if we can afford them
            if (_drones.Count < WeaponDamage)
            {
                if (_game.Credits[Shooter.Team - 1] >= DroneCost)
                {
                    _game.SpendCredits(Shooter.Team, DroneCost);
                    var d = _game.Ships.CreateShip(_droneShipSpec, Shooter.Team, Shooter.Colour, Shooter.SectorId);

                    if (d != null)
                    {
                        d.CenterX = Shooter.CenterX + FireOffset.X;
                        d.CenterY = Shooter.CenterY + FireOffset.Y;
                        var droneTarget = _droneSurroundShooter ? Shooter : Target;

                        if (_dronesLaunched && droneTarget != null)
                        {
                            d.OrderShip(new SurroundOrder(_game, Shooter.SectorId, droneTarget, _droneSurroundDistance));
                            _game.AddUnit(d);
                        }
                        else
                        {
                            d.Active = false;
                        }

                        _drones.Add(d);
                    }
                }
            }
        }

        public override void CheckForANewTarget()
        {
            base.CheckForANewTarget();

            if (!_initialised) InitialiseDrones();

            // We take care of our drone AI here
            for (var i = 0; i < _drones.Count; i++)
            {
                var d = _drones[i];
                if (d.Health <= 0)
                {
                    d.Active = false;
                    _drones.Remove(d);
                }
            }

            var newDroneTarget = _droneSurroundShooter ? Shooter : Target;

            if (_dronesLaunched && (newDroneTarget == null || Target == null || !IsThreatInSector()))
            {
                DockDrones();
                _droneTarget = null;
                _dronesLaunched = false;
            }
            else if (newDroneTarget != null && Target != null && ((_dronesLaunched && _droneTarget != newDroneTarget) || (!_dronesLaunched && IsThreatInSector())))
            {
                LaunchDrones(newDroneTarget);
                _droneTarget = newDroneTarget;
                _dronesLaunched = true;
            }
        }

        private bool IsThreatInSector() => _game.AllUnits.Any(_ => _.Active && _.Alliance != Shooter.Alliance && _.SectorId == Shooter.SectorId && _.Type != EShipType.Lifepod && _.IsVisibleToTeam(Shooter.Team-1));

        private void InitialiseDrones()
        {
            _initialised = true;

            var race = _game.Faction[Shooter.Team - 1].Race;
            _droneShipSpec = _game.Ships.RaceShips[race].FirstOrDefault(_ => _.Id == DroneShipId);
            if (_droneShipSpec == null) throw new Exception($"Unknown Carrier Drone Ship Id: {DroneShipId} (Race: {race}, Team: {Shooter.Team})");
            _droneShipSpec.Initialise(_game);

            if (_droneShipSpec.Weapons.Count > 0)
            {
                if (_droneShipSpec.Weapons[0] is ShipLaserWeapon w)
                {
                    _droneSurroundDistance = (int)w.WeaponRange;
                    _droneSurroundShooter = w is NanLaserWeapon || w is ShieldChargeWeapon;
                }
            }

            _drones = new List<CombatShip>();
            for (var i = 0; i < WeaponDamage; i++)
            {
                var d = _game.Ships.CreateShip(_droneShipSpec, Shooter.Team, Shooter.Colour, Shooter.SectorId);
                
                if (d != null) 
                {
                    d.Active = false;
                    _drones.Add(d);
                }
            }
        }

        private void LaunchDrones(GameEntity target)
        {
            for (var i = 0; i < _drones.Count; i++)
            {
                var d = _drones[i];
                if (!d.Active)
                {
                    d.SectorId = Shooter.SectorId;
                    d.CenterX = Shooter.CenterX + FireOffset.X;
                    d.CenterY = Shooter.CenterY + FireOffset.Y;
                    d.Active = true;
                    _game.AddUnit(d);
                }
                d.OrderShip(new SurroundOrder(_game, Shooter.SectorId, target, _droneSurroundDistance));
            }
        }

        private void DockDrones()
        {
            for (var i = 0; i < _drones.Count; i++)
            {
                var d = _drones[i];
                if (d.Active) d.OrderShip(new InterceptOrder(_game, Shooter, Shooter.SectorId));
            }
        }
    }
}