using UnityEngine;
using System.Collections;

/// <summary>
/// This is a basic item class which is used for the items in our game.
/// </summary>
public class Item : MonoBehaviour {
	public string Name;
	public string Lore;
	public string Effect;

	/// <summary>
	/// Get method to obtain the name
	/// </summary>
	public string GetName() {
		return Name;
	}
}
