using UnityEngine;
using System.Collections;
[System.Serializable]
public class GameAchievement {

    public string Name { get; set; }
    public string Description { get; set; }

    public GameAchievement()
    {

    }

    public GameAchievement(string name, string description)
    {
        Name = name;
        Description = description;
    }
}
