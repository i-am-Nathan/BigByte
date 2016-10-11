using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NavigateShop : MonoBehaviour {

	public List<GameObject> ItemList;
	public GameObject[] Items;
	public int Index = 0;
	// Use this for initialization
	void Start () {
		Debug.Log (ItemList.Count);
		foreach (GameObject o in Items) {
			GameObject item = Instantiate (o) as GameObject;
			item.transform.SetParent (GameObject.Find ("ShopItem").transform);
			item.transform.position = GameObject.Find ("ShopItem").transform.position + new Vector3(0,4f,0);
			ItemList.Add (item);
			item.SetActive (false);
		}
		ItemList [Index].SetActive (true);
	}
	void Update(){
			ItemList [Index].transform.Rotate (0, 0.5f, 0);
			if (Input.GetKeyDown (KeyCode.LeftArrow) || Input.GetKeyDown (KeyCode.A)) {
				Previous ();
			} else if (Input.GetKeyDown (KeyCode.RightArrow) || Input.GetKeyDown (KeyCode.D)) {
				Next ();
			}

	}
	public void Next(){
		ItemList [Index].SetActive (false);
		if (Index == ItemList.Count - 1) {
			Index = 0;
		} else {
			Index++;
		}
		ItemList [Index].SetActive (true);
	}
	public void Previous(){
		ItemList [Index].SetActive (false);
		if (Index == 0) {
			Index = ItemList.Count-1;
		} else {
			Index--;
		}
		ItemList [Index].SetActive (true);
	}
		
}
