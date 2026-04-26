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

public enum CardType
{
    Offensive,
    Defensive,
    UnInterruptable
}

public enum DamageType
{
    Slash,
    Pierce,
    Blunt
}