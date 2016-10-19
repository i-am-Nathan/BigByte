using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class NavigateShop : MonoBehaviour {

	public List<GameObject> ItemList;
	public GameObject[] Items;
	public int Index = 0;
	private Text _price;
	private Text _quantity;
	private Text _lore;
	private Text _effect;
	private Text _itemName;
	private Text _currentGold;
	public int[] ItemQuantity;
	public int[] Price;
	public AudioClip BuySound;
	private AudioSource _source;
	private GameData _gameData;
	private SubInventoryManager _subInventoryManager;

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
	// Use this for initialization
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
	void Update(){
			ItemList [Index].transform.Rotate (0, 0, 0.5f);
		if (Input.GetKeyDown (KeyCode.LeftArrow) || Input.GetKeyDown (KeyCode.A)) {
			Previous ();
		} else if (Input.GetKeyDown (KeyCode.RightArrow) || Input.GetKeyDown (KeyCode.D)) {
			Next ();
		} else if (Input.GetKeyDown (KeyCode.KeypadEnter)) {
			// Player 1
			if (ItemQuantity [Index] != 0 && _gameData.GetAmountOfGold() >= Price[Index]) {
				ItemQuantity [Index]--;
				_gameData.UpdateGold (0 - Price [Index]);
				_currentGold.text = _gameData.GetAmountOfGold() + "";
				_quantity.text= ItemQuantity [Index] +"";
				_source.PlayOneShot (BuySound);
				_subInventoryManager.AddItemQuantity (Items[Index].GetComponent<Item>().Name, true);
			}
		} else if (Input.GetKeyDown (KeyCode.J)) {
			// Player 2
			//Debug.Log(ItemQuantity [Index] + "");
			Debug.Log (_gameData.GetAmountOfGold () + "");
			if (ItemQuantity [Index] != 0 && _gameData.GetAmountOfGold() >= Price[Index]) {
				ItemQuantity [Index]--;
				_gameData.UpdateGold (0 - Price [Index]);
				_currentGold.text = _gameData.GetAmountOfGold() + "";
				_quantity.text= ItemQuantity [Index] +"";
				_source.PlayOneShot (BuySound);
				_subInventoryManager.AddItemQuantity (Items[Index].GetComponent<Item>().Name, false);
			}
		}

	}
	public void Next(){
		ItemList [Index].SetActive (false);
		if (Index == ItemList.Count - 1) {
			Index = 0;
		} else {
			Index++;
		}
		UpdateInfo ();

	}
	public void Previous(){
		ItemList [Index].SetActive (false);
		if (Index == 0) {
			Index = ItemList.Count-1;
		} else {
			Index--;
		}
		UpdateInfo ();
	}

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
