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
        MissileHit,
        ImportantMessage,
        GameLost,
        GameWon
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
        Generic,
        Resource,
        Carbon,
        Uranium,
        Silicon
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
        Battlecruiser,
        Support,
        AdvancedSupport,
        HeavySupport,
        ShieldTower,

        DroneScout,
        DroneFighter,
        DroneInterceptor,
        DroneBomber,
        DroneGunship,
        DroneStealthFighter,
        DroneStealthBomber,
    }

    public enum EBaseType
    {
        None,
        Outpost,
        Starbase,
        Tactical,
        Supremacy,
        Expansion,
        Shipyard,
        Tower,
        MissileTower,
        RepairTower,
        Resource,
        Minefield,
        ShieldTower,
        Aliens
    }

    public enum EOrderType
    {
        None,
        Base,
        Ship,
        Ability,
        QuickChat
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

        ShieldRecharge,
        MaxShield,

        AbilityDuration,
        AbilityCooldown,
        AbilityEffect,
        
        LaserDamage,
        LaserFireRate,

        MissileDamage,
        MissileFireRate,
    }

    public enum ESounds
    {
        vo_sal_recruitsarrived,
        vo_request_minefield,
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
        vo_builder_shipyard,
        vo_capture_enemyexpansion,
        vo_capture_enemygarrison,
        vo_capture_enemymine,
        vo_capture_enemyoutpost,
        vo_capture_enemysupremecy,
        vo_capture_enemytactical,
        vo_capture_enemyshipyard,
        vo_capture_expansion,
        vo_capture_garrison,
        vo_capture_mine,
        vo_capture_outpost,
        vo_capture_supremecy,
        vo_capture_tactical,
        vo_capture_shipyard,
        vo_destroy_enemyexpansion,
        vo_destroy_enemygarrison,
        vo_destroy_enemyoutpost,
        vo_destroy_enemyrefinery,
        vo_destroy_enemysupremecy,
        vo_destroy_enemytactical,
        vo_destroy_enemyteleport,
        vo_destroy_enemyshipyard,
        vo_destroy_expansion,
        vo_destroy_garrison,
        vo_destroy_miner,
        vo_destroy_outpost,
        vo_destroy_refinery,
        vo_destroy_supremecy,
        vo_destroy_tactical,
        vo_destroy_teleport,
        vo_destroy_shipyard,
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
        vo_sal_capitalsighted,
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
        missilelock,
        sidewinder,
        final_explosion_small,
        final_explosion_medium,
        final_explosion_large,
        explosion_tiny,
        dropmine,

        vo_player_affirmative,
        vo_player_negative,
        vo_player_shoot,
        vo_player_yeeehaa,
        vo_player_attacktarget,
        vo_player_needhelp,
        vo_player_donateinvestor,
        vo_player_needturret,
        vo_player_joiningturret,
        vo_player_oops,
        vo_ma_needoffense,
        vo_player_attackbase,
        vo_player_attackconstuctor,
        vo_player_attackfighter,
        vo_player_attackinterceptor,
        vo_player_attackcapital,
        vo_player_attackminer,
        vo_player_attackbomber,
        vo_player_attackripcord,
        vo_player_attackstealth,
        vo_player_attacktransport,
        vo_player_attackscout,
        vo_player_attacktower,
        vo_ma_attackminefield,
        vo_ma_attacknanite,
        vo_ma_attackrescue,
        vo_ma_attackspecial,
        vo_ma_attackprobe,
        vo_player_scoutaleph,
        vo_player_foundenemyconstruct,
        vo_player_constructorlaunching,
        vo_player_findfreakinbase,
        vo_player_gogogo,
        vo_player_minerhunt,
        vo_player_staytogether,
        vo_player_ripcordhome,
        vo_player_laylow,
        vo_player_foundenemyminer,
        vo_player_baseunderattack,
        vo_player_deployprobes,
        vo_player_wait4signal,
        vo_player_retreat,
        vo_player_deploymines,
        vo_player_repairstation,
        vo_player_follow,
        vo_player_stayontarget,
        vo_player_gotcha,
        vo_ma_capturingbase,
        vo_ma_deployrescue,
        vo_ma_dontkillpods,
        vo_ma_offturret,
        vo_ma_stayoutofmiddle,
        vo_ma_pickuptech,
        vo_ma_needdefense,
        vo_player_defendbase,
        vo_player_defendconstuctor,
        vo_player_defendfighter,
        vo_player_defendinterceptor,
        vo_player_defendcapital,
        vo_player_defendminer,
        vo_player_defendbomber,
        vo_player_defendripcord,
        vo_player_defendstealth,
        vo_player_defendtransport,
        vo_player_defendscout,
        vo_player_defendtower,
        vo_ma_neednanonconst,
        vo_ma_defendspecial,
        vo_ma_cloaked,
        vo_ma_dontdefend,
        vo_player_inboundenemybomb,
        vo_player_inboundenemycap,
        vo_player_inboundenemyfight,
        vo_player_inboundenemytrans,
        vo_player_findalephs,
        vo_player_findcarbonacous,
        vo_player_findenemy,
        vo_player_findhelium,
        vo_player_findminer,
        vo_player_findsilicon,
        vo_player_finduranium,
        vo_player_findprobes,
        vo_ma_findcash,
        vo_ma_findscout,
        vo_ma_findass,
        vo_ma_findconst,
        vo_player_makeme,
        vo_player_ISPdrop,
        vo_player_everyoneready,
        vo_player_savingup,
        vo_player_startthegame,
        vo_player_didyoucopy,
        vo_player_coupleminutes,
        vo_player_justasec,
        vo_player_niceweather,
        vo_player_whocommander,
        vo_player_changethemap,
        vo_player_imclueless,
        vo_player_itslate,
        vo_player_pitstop,
        vo_player_rematch,
        vo_player_changedsettings,
        vo_player_gototeamonly,
        vo_player_shallwebegin,
        vo_player_checkwing,
        vo_player_misc17,
        vo_player_misc25,
        vo_player_misc3,
        vo_player_iquit,
        vo_player_heeheehee,
        vo_ma_pickteams,
        vo_ma_evenup,
        vo_ma_hello,
        vo_ma_settingssuck,
        vo_ma_stopspam,
        vo_ma_wakeup,
        vo_ma_mutiny,
        vo_player_donatecredits,
        vo_player_needammo,
        vo_player_needbase,
        vo_player_needconstructor,
        vo_player_needbetterfighters,
        vo_player_needfighter,
        vo_player_needfuel,
        vo_player_needfightersupport,
        vo_player_needinterceptor,
        vo_player_needobjective,
        vo_player_needcapital,
        vo_player_needminer,
        vo_player_needpickup,
        vo_player_needbomber,
        vo_player_needripcord,
        vo_player_needstealth,
        vo_player_needtransport,
        vo_player_needscout,
        vo_player_needtower,
        vo_player_needbetterwep,
        vo_player_needrepairs,
        vo_player_needmoney,
        vo_ma_needrescue,
        vo_ma_pickuppods,
        vo_ma_needmines,
        vo_ma_needtpscout,
        vo_player_acknowledged,
        vo_player_cool,
        vo_player_fiesty,
        vo_player_nomoney,
        vo_player_imonit,
        vo_player_onmyway,
        vo_player_takingittothem,
        vo_player_objectivecomplete,
        vo_player_surelyjoking,
        vo_player_hellyeah,
        vo_player_yourecrazy,
        vo_player_aintnothin,
        vo_player_holdurehorses,
        vo_player_roger,
        vo_player_scuseme,
        vo_player_whatnow,
        vo_player_imbusy,
        vo_player_misc9,
        vo_player_what,
        vo_ma_ready,
        vo_ma_cantpodded,
        vo_ma_dontthinkso,
        vo_ma_noImeanit,
        vo_ma_bugout,
        vo_ma_notime,
        vo_ma_asseskicked,
        vo_ma_harshlanguage,
        vo_player_almosthadyou,
        vo_player_comebackandfight,
        vo_player_deathbecomesyou,
        vo_player_howdthatfeel,
        vo_player_thathurt,
        vo_player_slapinvestor,
        vo_player_likelambs,
        vo_player_uhavenohonor,
        vo_player_ooohsorry,
        vo_player_wantapiece,
        vo_player_ripcordlastresort,
        vo_player_shootcommander,
        vo_player_udiedwithdignity,
        vo_player_ullbesorry,
        vo_player_yes,
        vo_player_comeondown,
        vo_player_howwasthat,
        vo_player_misc2,
        vo_player_misc20,
        vo_player_misc21,
        vo_player_misc27,
        vo_player_shootingfish,
        vo_player_yowie,
        vo_player_payloaddelivered,
        vo_player_urgoodbut,
        vo_ma_dogmeat,
        vo_ma_iqs,
        vo_ma_nowyouseeme,
        vo_ma_leavemark,
        vo_ma_stopgrin,
        vo_ma_wonttolerate,
        vo_player_argh,
        vo_player_bye,
        vo_player_dang,
        vo_player_foundenemybase,
        vo_player_gunnerready,
        vo_player_hi,
        vo_player_needrescue,
        vo_player_kinguniverse,
        vo_player_nicework,
        vo_player_nooo,
        vo_player_ohyeah,
        vo_player_pheww,
        vo_player_ripcording,
        vo_player_sweet,
        vo_player_thanks,
        vo_player_turretsattack,
        vo_player_thankslift,
        vo_player_watchfire,
        vo_player_doah,
        vo_player_awesome,
        vo_player_imissed,
        vo_player_ooooaaaa,
        vo_player_aaaaoooo,
        vo_player_readchat,
        vo_ma_gameover,
        vo_ma_gametime,
        vo_ma_letsgo,
        vo_ma_notenough,
        vo_ma_isalephmined,
        vo_player_alephmined,
        vo_ma_alephmineddroned,
        vo_ma_howfar,
        vo_ma_theyexp,
        vo_ma_theysup,
        vo_ma_theytac,
        vo_player_haveshipyard,
        vo_ma_pushop,
        vo_ma_podded,
        vo_ma_ripped,
        vo_ma_spotted,
        vo_ma_notseen,
        vo_player_meetatjump,
        vo_player_formonmywing,
        vo_player_holdup,
        vo_ma_rush,
        vo_player_headback,
        vo_ma_scoutahead,
        vo_player_getoffme,
        vo_ma_naniteslaunch,
        vo_ma_nanitesrip,
        vo_ma_campaleph,
        vo_ma_campbase,
        vo_ma_campteleport,
        vo_player_capshipwaiting,
        vo_player_bomberwaiting,
        vo_player_isourbaseclear,
        vo_player_minersaredead,
        vo_player_escortminer,
        vo_player_foundenemyships,
        vo_player_enemyhasflag,
        vo_player_inbound,
        vo_player_targetneeded,
        vo_player_complete,
        vo_player_escortbuilder,
        vo_player_needcruiser,
        vo_player_minershammered,
        vo_player_basecaptured,
        vo_player_havecapitols,
        vo_player_gimmesomething,
        vo_player_shieldsaredown,
        vo_player_yourmad,
        vo_player_coverme,
        vo_player_traitor,
        vo_player_outofammohmmm,
        vo_player_transportoutbound,
        vo_player_formate,
        vo_player_regroup,
        vo_player_cantholdem,
        vo_player_foundaleph,
        vo_player_foundhelium,
        vo_player_defend,
        vo_player_attack,
        vo_player_attackdefender,
        vo_player_defenddefender,
        vo_player_needdefender,
        squish_01,
        shieldcharge1
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

    public enum ELadderType
    {
        Ones = 1,
        Twos = 2
    }

    public enum ELadderTier
    {
        Unranked,
        Bronze,
        Silver,
        Gold,
        Platinum,
        Diamond,
        Master,
        Challenger
    }
    
    public enum EVariantAiStyle
    {
        Aggressive, Expanding, Defensive, Random
    }

    public enum EVariantAiTechFocus
    {
        CapitalsOnly, Singletech, DoubleTech, TripleTech, Random
    }

    public enum EVariantAiResearchFocus
    {
        EvenShip, EvenUpgrade, EvenEnd, SingleShip, SingleUpgrade, SingleEnd, Any, Random
    }

    public enum EMirrorType
    {
        Vertical, Horizontal
    }

    public enum EMapSize
    {
        Small, Normal, Large
    }

    public enum EGameType
    {
        Skirmish
    }

    public enum EWaveTargetType
    {
        None,
        Player,
        AI,
        Everyone
    }

    public enum EAbilityType
    {
        EngineBoost,
        ShieldBoost,
        HullRepair,
        WeaponBoost,
        RapidFire,
        ScanBoost,
        StealthBoost
    }

    public enum ERaceType
    {
        Amanni,
        Krael,
        Zoniqs
    }
}
