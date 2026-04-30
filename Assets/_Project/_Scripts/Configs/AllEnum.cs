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
    Safe,
    Encounter,
    Chest
}