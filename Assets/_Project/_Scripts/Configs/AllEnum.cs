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
    Main = 10,
    WolfLeader = 10000,
    Wolf = 10010,
    LittleWolf = 10020,
    Parasite = 10030,
    Ann = 10040,
    Benn = 10050,
    Canis = 10060,
    Lich = 10070,
    Yinyang = 10080,
}

public enum CardID
{
    None = 0, 
    
    //Basic
    BasicEvade = 10,
    BasicAttack = 11,
    BasicBlock = 12,
    BasicRemoveWeapon = 13,
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
    RetrieveArrow = 310,
    
    //Claymore
    EarthShaker = 400,
    Cyclone = 401,
    Throw = 402,
    RetrieveClaymore = 403,
    
    //Rapier
    Fleche = 500,
    RapierCounter = 501,
    Flurry = 502,
    
    //Staff
    Fireball = 600,
    Healing = 601,
    HolyLight = 602,
    Bind = 603,
    
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
    QuickAction = 10,
    
    // Wolf  
    WolfEquipEvade = 100,
    WolfEquipBlock = 110,
    WolfEvadeSuccess = 120,
    WolfLeaderRoar = 130,
    WolfRoar = 140,
    
    
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
    OnClash,
    OnChangeStat
}

public enum CardType
{
    Offensive,
    Defensive,
    UnInterruptable,
    UseItem,
    UseWeapon
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
    Bomb = 0,
    HealPotion = 10,
    Smoke = 20,
    Trap = 30,
    ManaPotion = 40,
    Arrow = 50,
    Balo = 60,
    Bandage = 70,
    Antidote = 80,
    Needle = 90
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