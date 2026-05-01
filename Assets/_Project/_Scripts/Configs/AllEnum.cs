public enum StatType
{
    DEF = 20,
    STR = 40,
    DEX = 50,
    CHA = 60,
    INT = 70,
    LUK = 80,
    MAG = 90,
    CRIT = 100,
    WEIGHT = 130
}

public enum UnitID
{
    None = 0,
    Main = 10,
    WolfLeader = 10000,
    Wolf = 10010,
    LittleWolf = 10020,
    
    Parasite = 10100,
    SmallParasite = 10110,
    Corpse = 10120,
    
    Lich = 10200,
    Skeleton = 10210,
    Turret = 10220,
    
    Yinyang = 10300,
    Soul = 10310
}

public enum CardID
{
    None = 0, 
    
    //Basic
    BasicEvade = 10,
    BasicAttack = 11,
    BasicBlock = 12,
    // ThrowWeapon = 13,
    BasicUse = 14,
    // BasicDash = 15,
    BasicEquip = 16,
    
    //Dagger
    SneakAttack = 100,
    ThrowDagger = 101,
    Puncture = 102,
    RetrieveDagger = 103,
    
    //Sword
    Slash = 200,
    SwordCounter = 201,
    Thrust = 202,
    ThrowSword = 203,
    RetrieveSword = 204,
    
    //Bow
    Shoot = 300,
    ThrowBow = 301,
    RetrieveBow = 302,
    RetrieveArrow = 310,
    
    //Claymore
    EarthShaker = 400,
    Cyclone = 401,
    ThrowClaymore = 402,
    RetrieveClaymore = 403,
    
    //Rapier
    Fleche = 500,
    RapierCounter = 501,
    Flurry = 502,
    ThrowRapier = 503,
    RetrieveRapier = 504,
    
    //Staff
    Fireball = 600,
    Healing = 601,
    HolyLight = 602,
    Bind = 603,
    ThrowStaff = 604,
    RetrieveStaff = 605,
    
    //Shield
    Bulwark = 700,
    ShieldBash = 701,
    ThrowShield = 702,
    RetrieveShield = 703,
    
    //Spear
    Piercing = 800,
    Sweep = 801,
    ThrowSpear = 802,
    RetrieveSpear = 803,
    
    //Enemy
    //Wolf Leader 
    BigClaw = 10010,
    DeepBite = 10020,
    WolfLeaderRoar = 10050,
    
    // Wolf
    Claw = 10100,
    Bite = 10110,
    WolfRoar = 10120,
    
    //Lich
    Sacrifice = 10200,
    Shared = 10210,
    
    //Skeleton
    Rebuild = 10250,
    
    //YinYang
    DimensionCut = 10300,
    
    //Parasite
    Roll = 10400,
    Evolve = 10410,
    Absorb = 10420,
    Throw = 10430,
    
    //Small Parasite
    QuickBite = 10450,
    
    CorpseBlock = 10460,
    
    // General
    WolfEvade = 100000,
    WolfBlock = 100010,
    WolfDash = 100020,
    
}

public enum Class
{
    Swordman,
    Sorcerer,
    Archer,
}

public enum PassiveID
{
    None = 0,
    QuickAction = 10,
    
    // Wolf  
    WolfEquipEvade = 100,
    WolfEquipBlock = 110,
    WolfEvadeSuccess = 120,
    WolfLeaderRoar = 130,
    WolfRoar = 140,
    
    // Lich
    Summon = 200,
    Protection = 210,
    Concentrate = 220,
    
    // Skeleton
    SkeletonBlock = 250,
    
    // Yin Yang
    OnChangeDimension = 300,
    DimensionSwitch = 320,
    YinDimension = 330,
    YangDimension = 340,
    
    // Soul
    DeadAndRevive = 350,
    
    // Parasite
    EnvironmentChange = 400,
    Evolve = 410,
    
    // Small Parasite
    NoHPCap = 450,
    
    // Corpse
    CorpeEquipBlock = 480,
    CorpeBlockSuccess = 490,
}

public enum StatusID
{
    None,
    Burn,
    Bleed,
    Wound,
    Fragile,
    Paralyze,
    Stun,
    Anger,
    Protection,
    Corrupt,
    Weaken,
    Poison,
    BuffStrength
}

public enum PassiveActivationType
{
    OnEndAction,
    OnStartAction,
    OnEvadeSuccess,
    OnBlockSuccess,
    OnClash,
    OnChangeStat,
    OnTakeDamage
}

public enum CardType
{
    Offensive,
    Defensive,
    UnInterruptable,
    UseItem,
    UseWeapon,
    ThrowWeapon,
}

public enum DamageType
{
    Slash,
    Pierce,
    Blunt
}

public enum InventoryItemType
{
    Weapon,
    Tool
}

public enum WeaponID
{
    None = 0,
    Bow = 10,
    Claymore = 20,
    Dagger = 30,
    Rapier = 40,
    Shield = 50,
    Spear = 60,
    Staff = 70,
    Sword = 80
}

public enum ToolID
{
    None = 0,
    Bomb = 10,
    Trap = 20,
    HealPotion = 30,
    Bandage = 40,
    Antidote = 50,
    Needle = 60,
    Cursed = 70,
    Energy = 80,
}

public enum LibraryItemType
{
    Enemy,
    Skill,
    Passive
}

public enum NodeType
{
    Combat,
    Rest,
    Shop,
    Chest
}

public enum LineState //For line node UI
{
    Locked,
    Unlocked,
    Visited
}