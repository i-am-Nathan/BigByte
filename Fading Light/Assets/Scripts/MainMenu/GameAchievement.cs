// file:	Assets\Scripts\MainMenu\GameAchievement.cs
//
// summary:	Implements the game achievement class

using UnityEngine;
using System.Collections;

/// <summary>   A game achievement. </summary>
///
/// <remarks>    . </remarks>

[System.Serializable]
public class GameAchievement {

    /// <summary>   Gets or sets the name. </summary>
    ///
    /// <value> The name. </value>

    public string Name { get; set; }

    /// <summary>   Gets or sets the description. </summary>
    ///
    /// <value> The description. </value>

    public string Description { get; set; }

    /// <summary>   Default constructor. </summary>
    ///
 

    public GameAchievement()
    {

    }

    /// <summary>   Constructor. </summary>
    ///
 
    ///
    /// <param name="name">         The name. </param>
    /// <param name="description">  The description. </param>

    public GameAchievement(string name, string description)
    {
        Name = name;
        Description = description;
    }
}
