// file:	Assets\DownloadedContent\AllStarCharacterLibrary\Scripts\Outfitter.cs
//
// summary:	Implements the outfitter class

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

/// <summary>   (Serializable) an outfitter. </summary>
///
/// <remarks>    . </remarks>

[Serializable]
public class Outfitter : MonoBehaviour 
{
	
    /// <summary>   The AC. </summary>
	CharacterDemoController ac;
    /// <summary>   Zero-based index of the old weapon. </summary>
	int oldWeaponIndex;
    /// <summary>   The weapons. </summary>
	[SerializeField]
	public List<WeaponSlot> weapons;
	
	// Use this for initialization

    /// <summary>   Starts this object. </summary>
    ///
 

	void Start () 
	{
		ac = GetComponentInChildren<CharacterDemoController>();
		for(int i = 0;i<weapons.Count;i++)
		{
			for(int model=0;model<weapons[i].models.Count;model++)
			{
				weapons[i].models[model].enabled = false;
			}
		}
		for(int model=0;model<weapons[ac.WeaponState].models.Count;model++)
		{
			weapons[ac.WeaponState].models[model].enabled = true;
		}
		oldWeaponIndex=ac.WeaponState;
	}
	
	// Update is called once per frame

    /// <summary>   Updates this object. </summary>
    ///
 

	void Update () 
	{
		if(ac.WeaponState!=oldWeaponIndex)
		{
			for(int model=0;model<weapons[oldWeaponIndex].models.Count;model++)
			{
				weapons[oldWeaponIndex].models[model].enabled = false;
			}
			for(int model=0;model<weapons[ac.WeaponState].models.Count;model++)
			{
				weapons[ac.WeaponState].models[model].enabled = true;
			}
			oldWeaponIndex=ac.WeaponState;
		}
	}
}

/// <summary>   (Serializable) a weapon slot. </summary>
///
/// <remarks>    . </remarks>

[Serializable]
public class WeaponSlot
{
    /// <summary>   The models. </summary>
	[SerializeField]
	public List<Renderer> models = new List<Renderer>();
}
