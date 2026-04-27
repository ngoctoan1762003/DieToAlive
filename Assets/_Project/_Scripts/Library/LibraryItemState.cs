using System.Collections.Generic;

[System.Serializable]
public class LibraryItemState
{
    public string id;
    public bool isDiscovered;

    public List<SkillState> skills = new();
    public List<PassiveState> passives = new();
}

[System.Serializable]
public class SkillState
{
    public string id;
    public bool isUnlocked;
}

[System.Serializable]
public class PassiveState
{
    public string id;
    public bool isUnlocked;
}