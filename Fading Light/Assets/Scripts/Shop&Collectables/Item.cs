// file:	Assets\Scripts\Shop&Collectables\Item.cs
//
// summary:	Implements the item class

using UnityEngine;
using System.Collections;

/// <summary>   This is a basic item class which is used for the items in our game. </summary>
///
/// <remarks>    . </remarks>

public class Item : MonoBehaviour {
    /// <summary>   The name. </summary>
	public string Name;
    /// <summary>   The lore. </summary>
	public string Lore;
    /// <summary>   The effect. </summary>
	public string Effect;

    /// <summary>   Get method to obtain the name. </summary>
    ///
 
    ///
    /// <returns>   The name. </returns>

	public string GetName() {
		return Name;
	}
}
