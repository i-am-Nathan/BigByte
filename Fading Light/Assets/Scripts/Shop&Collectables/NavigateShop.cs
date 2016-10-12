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
	public int[] ItemQuantity;
	public int[] Price;
	public AudioClip BuySound;
	private AudioSource _source;

	void Awake(){
		_price = GameObject.FindGameObjectWithTag ("Price").GetComponent<Text> ();
		_quantity = GameObject.FindGameObjectWithTag ("Quantity").GetComponent<Text>();
		_lore = GameObject.FindGameObjectWithTag("Lore").GetComponent<Text>();
		_effect = GameObject.FindGameObjectWithTag("Effect").GetComponent<Text>();
		_itemName = GameObject.FindGameObjectWithTag("ShopItemName").GetComponent<Text>();
		_source = GetComponent<AudioSource>();


	}
	// Use this for initialization
	void Start () {
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
			if (ItemQuantity [Index] != 0) {
				ItemQuantity [Index]--;
				_quantity.text= ItemQuantity [Index] +"";
				_source.PlayOneShot (BuySound);
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
	}
		
}
