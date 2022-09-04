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
            // Nothing to draw for now...
            // TODO: Draw dots to visualise available drones?
        }

        public override void DamageTarget(float boostAmount)
        {
            // Instead of shooting, this weapon can re-build a drone for credits, if required
            if (_drones.Count < WeaponDamage)
            {
                if (_game.Credits[Shooter.Team - 1] >= DroneCost)
                {
                    _game.SpendCredits(Shooter.Team, DroneCost);
                    var d = _game.Ships.CreateShip(_droneShipSpec, Shooter.Team, Shooter.Colour, Shooter.SectorId);

                    if (d != null)
                    {
                        _drones.Add(d);

                        var droneTarget = _droneSurroundShooter ? Shooter : Target;

                        if (_dronesLaunched && droneTarget != null)
                        {
                            // Prepare to launch drone
                            d.CenterX = Shooter.CenterX + FireOffset.X;
                            d.CenterY = Shooter.CenterY + FireOffset.Y;

                            // Add to the game so it can fly!
                            _game.AddUnit(d);

                            d.OrderShip(new SurroundOrder(_game, Shooter.SectorId, droneTarget, _droneSurroundDistance));
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

            // We take care of our drone collection here...
            if (!_initialised) InitialiseDrones();

            // Remove dead drones
            for (var i = 0; i < _drones.Count; i++)
            {
                var d = _drones[i];
                if (d.Health <= 0)
                {
                    d.Active = false;
                    _drones.Remove(d);
                }
            }

            // Drone target selection & docking controls
            var newDroneTarget = _droneSurroundShooter ? Shooter : Target;

            if (_dronesLaunched && (newDroneTarget == null || Target == null || !IsThreatInSector()))
            {
                // Dock drones if launched and there is no target in range or threat detected
                DockDrones();
                _droneTarget = null;
                _dronesLaunched = false;
            }
            else if (newDroneTarget != null && Target != null && ((_dronesLaunched && _droneTarget != newDroneTarget) || (!_dronesLaunched && IsThreatInSector())))
            {
                // Order drones if not launched and we have a target, or if the target has changed
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
                    // Prepare to launch drone
                    d.SectorId = Shooter.SectorId;
                    d.CenterX = Shooter.CenterX + FireOffset.X;
                    d.CenterY = Shooter.CenterY + FireOffset.Y;
                    d.Active = true;

                    // Add to the game so it can fly!
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