using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {
	public string Name;
	public string Lore;
	public string Effect;

	public string GetName() {
		return Name;
	}
}
