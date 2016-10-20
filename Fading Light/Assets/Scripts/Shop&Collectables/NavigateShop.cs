﻿// file:	Assets\Scripts\Shop&Collectables\NavigateShop.cs
//
// summary:	Implements the navigate shop class

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>   This is the class used to navigate the user through the shop. </summary>
///
/// <remarks>    . </remarks>

public class NavigateShop : MonoBehaviour {

    /// <summary>   List of items. </summary>
	public List<GameObject> ItemList;
    /// <summary>   The items. </summary>
	public GameObject[] Items;
    /// <summary>   Zero-based index of the. </summary>
	public int Index = 0;
    /// <summary>   The price. </summary>
	private Text _price;
    /// <summary>   The quantity. </summary>
	private Text _quantity;
    /// <summary>   The lore. </summary>
	private Text _lore;
    /// <summary>   The effect. </summary>
	private Text _effect;
    /// <summary>   Name of the item. </summary>
	private Text _itemName;
    /// <summary>   The current gold. </summary>
	private Text _currentGold;
    /// <summary>   The item quantity. </summary>
	public int[] ItemQuantity;
    /// <summary>   The price. </summary>
	public int[] Price;
    /// <summary>   The buy sound. </summary>
	public AudioClip BuySound;
    /// <summary>   Source for the. </summary>
	private AudioSource _source;
    /// <summary>   Information describing the game. </summary>
	private GameData _gameData;
    /// <summary>   Manager for sub inventory. </summary>
	private SubInventoryManager _subInventoryManager;

    /// <summary>   This will set the appropriate fields in my class. </summary>
    ///
 

	void Awake(){
		_price = GameObject.FindGameObjectWithTag ("Price").GetComponent<Text> ();
		_quantity = GameObject.FindGameObjectWithTag ("Quantity").GetComponent<Text>();
		_lore = GameObject.FindGameObjectWithTag("Lore").GetComponent<Text>();
		_effect = GameObject.FindGameObjectWithTag("Effect").GetComponent<Text>();
		_itemName = GameObject.FindGameObjectWithTag("ShopItemName").GetComponent<Text>();
		_source = GetComponent<AudioSource>();
		_subInventoryManager = GameObject.Find("SubInventoryManager").GetComponent<SubInventoryManager> ();
		_currentGold = GameObject.FindGameObjectWithTag ("Current Gold").GetComponent<Text> ();
	}

    /// <summary>   Grabs the approrpiate items and sets the shop up. </summary>
    ///
 

	void Start () {
		GameObject go = GameObject.FindGameObjectWithTag("Game Data");
		_gameData = (GameData)go.GetComponent(typeof(GameData));

		foreach (GameObject o in Items) {
			GameObject item = Instantiate (o) as GameObject;
			item.transform.SetParent (GameObject.Find ("ShopItem").transform);
			item.transform.position = GameObject.Find ("ShopItem").transform.position + new Vector3(0,4f,0);
			ItemList.Add (item);
			item.SetActive (false);
		}
		UpdateInfo ();
	}

    /// <summary>
    /// This will allow players to navigate the shop and acknowledges the keypress from players.
    /// Players can navigate with the A/D key for Player 2 and Left Arrow/Right Arrow for Player
    /// 1.Purchase button is L and C for player 1 and 2 respectivly.
    /// </summary>
    ///
 

	void Update(){
			ItemList [Index].transform.Rotate (0, 0, 0.5f);
		if (Input.GetKeyDown (KeyCode.LeftArrow) || Input.GetKeyDown (KeyCode.A)) {
			Previous ();
		} else if (Input.GetKeyDown (KeyCode.RightArrow) || Input.GetKeyDown (KeyCode.D)) {
			Next ();
		} else if (Input.GetKeyDown (KeyCode.L)) {
			// Player 1
			if (ItemQuantity [Index] != 0 && _gameData.GetAmountOfGold() >= Price[Index]) {
				ItemQuantity [Index]--;
				_gameData.UpdateGold (0 - Price [Index]);
				_currentGold.text = _gameData.GetAmountOfGold() + "";
				_quantity.text= ItemQuantity [Index] +"";
				_source.PlayOneShot (BuySound);

				Debug.Log ("buying: " + Items [Index].GetComponent<Item> ().GetName ());

				_subInventoryManager.AddItemQuantity (Items[Index].GetComponent<Item>().GetName(), true);
			}
		} else if (Input.GetKeyDown (KeyCode.C)) {
			// Player 2
			if (ItemQuantity [Index] != 0 && _gameData.GetAmountOfGold() >= Price[Index]) {
				ItemQuantity [Index]--;
				_gameData.UpdateGold (0 - Price [Index]);
				_currentGold.text = _gameData.GetAmountOfGold() + "";
				_quantity.text= ItemQuantity [Index] +"";
				_source.PlayOneShot (BuySound);

				Debug.Log ("buying: " + Items [Index].GetComponent<Item> ().GetName ());

				_subInventoryManager.AddItemQuantity (Items[Index].GetComponent<Item>().GetName(), false);
			}
		}

	}

    /// <summary>   Move to the next item in the shop i.e. go right. </summary>
    ///
 

	public void Next(){
		ItemList [Index].SetActive (false);
		if (Index == ItemList.Count - 1) {
			Index = 0;
		} else {
			Index++;
		}
		UpdateInfo ();

	}

    /// <summary>   Move to the previous item in the shop i.e. go left. </summary>
    ///
 

	public void Previous(){
		ItemList [Index].SetActive (false);
		if (Index == 0) {
			Index = ItemList.Count-1;
		} else {
			Index--;
		}
		UpdateInfo ();
	}

    /// <summary>
    /// This will update the information according to the current item. The information it updates
    /// would be its lore, price, effect, name and price.
    /// </summary>
    ///
 

	public void UpdateInfo(){
		ItemList [Index].SetActive (true);
		_price.text= Price [Index] + " Coins";
		_quantity.text = ItemQuantity [Index] + "";
		_lore.text = ItemList [Index].GetComponent<Item> ().Lore;
		_effect.text = ItemList [Index].GetComponent<Item> ().Effect;
		_itemName.text = ItemList [Index].GetComponent<Item> ().Name;
		_currentGold.text = _gameData.GetAmountOfGold() + "";
	}
		
}
