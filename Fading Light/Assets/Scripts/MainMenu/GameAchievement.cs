using UnityEngine;
using System.Collections;

public class GameAchievement {

    public string Name { get; set; }
    public string Description { get; set; }
    public bool Achieved { get; set; }

    public GameAchievement()
    {

    }

    public GameAchievement(string name, string description, bool achieved)
    {
        Name = name;
        Description = description;
        Achieved = achieved;
    }
}
