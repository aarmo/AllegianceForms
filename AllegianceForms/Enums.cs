using System.Collections.Generic;

namespace AllegianceForms
{
    public enum EShipEventType
    {
        ShipDestroyed,
        BuildingStarted,
        BuildingFinished
    }

    public enum EBaseEventType
    {
        BaseDamaged,
        BaseDestroyed,
        BaseCaptured
    }

    public enum EGameEventType
    {
        ResearchComplete,
        DroneBuilt,
        SectorLeftClicked,
        SectorRightClicked,
        ShipLaunched,
        ShipClicked,
        MissileHit
    }

    public enum EVertDir
    {
        None,
        North,
        South
    }

    public enum EHorDir
    {
        None,
        East,
        West
    }

    public enum EAsteroidType
    {
        None,
        Rock,
        Resource,
        TechCarbon,
        TechUranium,
        TechSilicon
    }

    public enum EShipType
    {
        None,
        Lifepod,
        Scout,
        Fighter,
        Interceptor,
        Bomber,
        Miner,
        Constructor,
        StealthFighter,
        StealthBomber,
        Gunship,
        FighterBomber,
        TroopTransport,
        Tower,
        MissileTower,
        RepairTower,
        Corvette,
        Destroyer,
        Frigate,
        Devastator,
        Cruiser,
        Battleship,
        Battlecruiser
    }

    public enum EBaseType
    {
        None,
        Outpost,
        Starbase,
        Refinery,
        Tactical,
        Supremacy,
        Expansion,
        Shipyard,
        Tower,
        MissileTower,
        RepairTower,
    }

    public enum EOrderType
    {
        None,
        Base,
        Ship
    }

    public enum ETechType
    {
        ShipyardConstruction = -2,
        Construction = -1,
        Starbase = 0,
        Supremacy = 1,
        Tactical = 2,
        Expansion = 3,
        Base = 4,
    }

    public enum EGlobalUpgrade
    {
        ScanRange,
        ShipSignature,

        RepairHealing,
        WeaponDamage,
        WeaponFireRate,
        MissileTracking,
        MissileSpeed,
        ShipHull,
        ShipSpeed,

        MinerCapacity,
        MinerEfficiency,
    }

    public enum ESounds
    {
        accept,
        criticalmessage,
        mousedown,
        mouseover,
        newtarget,
        newtargetenemy,
        newtargetneutral,
        noncriticalmessage,
        sshipint,
        static1,
        static2,
        text,
        vo_attack_miner,
        vo_builder_expansion,
        vo_builder_garrison,
        vo_builder_outpost,
        vo_builder_refinery,
        vo_builder_supremecy,
        vo_builder_tactical,
        vo_builder_teleport,
        vo_capture_enemyexpansion,
        vo_capture_enemygarrison,
        vo_capture_enemymine,
        vo_capture_enemyoutpost,
        vo_capture_enemysupremecy,
        vo_capture_enemytactical,
        vo_capture_expansion,
        vo_capture_garrison,
        vo_capture_mine,
        vo_capture_outpost,
        vo_capture_supremecy,
        vo_capture_tactical,
        vo_destroy_enemyexpansion,
        vo_destroy_enemygarrison,
        vo_destroy_enemyoutpost,
        vo_destroy_enemyrefinery,
        vo_destroy_enemysupremecy,
        vo_destroy_enemytactical,
        vo_destroy_enemyteleport,
        vo_destroy_expansion,
        vo_destroy_garrison,
        vo_destroy_miner,
        vo_destroy_outpost,
        vo_destroy_refinery,
        vo_destroy_supremecy,
        vo_destroy_tactical,
        vo_destroy_teleport,
        vo_miner_dontgetpaid,
        vo_miner_enemyonscope,
        vo_miner_intransit,
        vo_miner_report4duty,
        vo_miner_underattack,
        vo_miner_yougotit,
        vo_request_buildercarbon,
        vo_request_buildergeneric,
        vo_request_builderhelium,
        vo_request_buildersilicon,
        vo_request_builderuranium,
        vo_request_miner,
        vo_request_tower,
        vo_sal_bombersighted,
        vo_sal_minercritical,
        vo_sal_minerpartial,
        vo_sal_newequip,
        vo_sal_noripcord,
        vo_sal_researchcomplete,
        vo_sal_sectorlost,
        vo_sal_sectorsecured,
        vo_sal_shipcantripcord,
        vo_sal_shiptech,
        vo_sal_stationrisk,
        windowslides,
        plasmaac1,
        plasmamini1,
        missilelockonme,
        outofammo,
        payday,
        small_explosion,
        big_explosion,
        sniperlaser1pwrup,
        missilelock
    }
    
    public enum EAiCreditPriorities
    {
        Expansion,
        Offense,
        Defense,
    }
    
    public enum EAiPilotPriorities
    {
        Scout,
        BaseDefense,
        BaseOffense,
        MinerOffense,
        MinerDefense,
    }
    
    public enum EMapSize
    {
        Tiny,
        Small,
        Medium,
        Large,
        Massive
    };

    public static class KeyValuePairExtensions
    {
        public static bool IsDefault<T, TU>(this KeyValuePair<T, TU> pair)
        {
            return pair.Equals(new KeyValuePair<T, TU>());
        }
    }
}
