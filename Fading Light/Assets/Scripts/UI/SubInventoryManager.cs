using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SubInventoryManager : MonoBehaviour {

	public Sprite HealthImage;
	public Sprite DefenseImage;
	public Sprite BerserkImage;

	private int _totalNumItems = 3;

	private int _player1CurrentItemIndex = 0;
	private int _player2CurrentItemIndex = 0;

	private Image _player1CurrentItem;
	private Image _player2CurrentItem;
	private Text _player1ItemName;
	private Text _player2ItemName;
	private Text _player1ItemQuantity;
	private Text _player2ItemQuantity;

	private Sprite[] _player1Items;
	private Sprite[] _player2Items;

	private PlayerController _player1ControllerScript;
	private Player2Controller _player2ControllerScript;

	private Dictionary<string,int> _player1ItemQuantityDictionary = new Dictionary<string,int>();
	private Dictionary<string,int> _player2ItemQuantityDictionary = new Dictionary<string,int>();
	private Dictionary<string,Sprite> _itemImageDictionary = new Dictionary<string,Sprite>();
	private Dictionary<int,string> _itemIndexNameDictionary = new Dictionary<int,string>();

	void Start() {
		GameObject player1 = GameObject.FindGameObjectWithTag("Player");
		_player1ControllerScript = (PlayerController)player1.GetComponent(typeof(PlayerController));

		GameObject player2 = GameObject.FindGameObjectWithTag("Player2");
		_player2ControllerScript = (Player2Controller)player2.GetComponent(typeof(Player2Controller));

		_player1ItemName = GameObject.Find ("Player1ItemName").GetComponent<Text> ();
		_player2ItemName = GameObject.Find ("Player2ItemName").GetComponent<Text> ();

		_player1ItemQuantity = GameObject.Find ("Player1ItemQuantity").GetComponent<Text> ();
		_player2ItemQuantity = GameObject.Find ("Player2ItemQuantity").GetComponent<Text> ();

		_player1CurrentItem = GameObject.FindWithTag("Player 1 SubInventory").GetComponent<Image>();
		_player2CurrentItem = GameObject.FindWithTag("Player 2 SubInventory").GetComponent<Image>();

		_itemIndexNameDictionary.Add (0, "Health Pot");
		_itemIndexNameDictionary.Add (1, "Attack Pot");
		_itemIndexNameDictionary.Add (2, "Defense Pot");

		_itemImageDictionary.Add ("Health Pot", HealthImage);
		_itemImageDictionary.Add ("Attack Pot", DefenseImage);
		_itemImageDictionary.Add ("Defense Pot", BerserkImage);

		_player1ItemQuantityDictionary.Add ("Health Pot", 0);
		_player1ItemQuantityDictionary.Add ("Attack Pot", 0);
		_player1ItemQuantityDictionary.Add ("Defense Pot", 0);
		_player2ItemQuantityDictionary.Add ("Health Pot", 0);
		_player2ItemQuantityDictionary.Add ("Attack Pot", 0);
		_player2ItemQuantityDictionary.Add ("Defense Pot", 0);

		SetItemOnScreen ("Health Pot", true);
		SetItemOnScreen ("Health Pot", false);
	}

	// Update is called once per frame
	void Update () {
		// Player 1 cycling through
		if (Input.GetKeyDown(KeyCode.O))
		{
			CycleItems (true);
		}

		// Player 2 cycling through
		if (Input.GetKeyDown(KeyCode.P))
		{
			CycleItems (false);
		}

		if (Input.GetKeyDown(KeyCode.Q))
		{
			UseItem (true);

        }

		if (Input.GetKeyDown(KeyCode.L))
		{
			UseItem (false);
		}
	}
		
	void CycleItems (bool cyclePlayer1) {
		// Cycle through for player 1's sub inventory
		if (cyclePlayer1) {
			if (_player1CurrentItemIndex != _totalNumItems-1) {
				_player1CurrentItemIndex += 1;		
			} else {
				_player1CurrentItemIndex = 0;
			}
			SetItemOnScreen(_itemIndexNameDictionary[_player1CurrentItemIndex], true);	
		} else {
			// Cycle through player 2's sub inventory
			if (_player2CurrentItemIndex != _totalNumItems-1) {
				_player2CurrentItemIndex += 1;		
			} else {
				_player2CurrentItemIndex = 0;
			}
			SetItemOnScreen(_itemIndexNameDictionary[_player2CurrentItemIndex], false);
		}
	}

	public void UseItem (bool player1) {
		if (player1) {
			if (_player1ItemQuantityDictionary [_player1ItemName.text] != 0) {
				_player1ItemQuantityDictionary[_player1ItemName.text] -= 1;
				SetItemOnScreen (_player1ItemName.text, true);
			}
			//HealthPotActivated
            if (_player1ItemName.text == "Health Pot")
            {
                _player1ControllerScript.HealthPotActivated();
            }
            //AttackPotActivated
            if (_player1ItemName.text == "Attack Pot")
            {
                _player1ControllerScript.AttackPotActivated();
            }
            //DefensePotActivated
            if (_player1ItemName.text == "Defense Pot")
            {
                _player1ControllerScript.DefensePotActivated();
            }
        } else {
			if (_player2ItemQuantityDictionary [_player2ItemName.text] != 0) {
				_player2ItemQuantityDictionary[_player2ItemName.text] -= 1;
				SetItemOnScreen (_player2ItemName.text, false);
			}
            //HealthPotActivated
            if (_player2ItemName.text == "Health Pot")
            {
                _player2ControllerScript.HealthPotActivated();
            }
            //AttackPotActivated
            if (_player2ItemName.text == "Attack Pot")
            {
                _player2ControllerScript.AttackPotActivated();
            }
            //DefensePotActivated
            if (_player2ItemName.text == "Defense Pot")
            {
                _player2ControllerScript.DefensePotActivated();
            }
        }
	}

	public void AddItemQuantity (string itemName, bool player1) {
		if (player1) {
			_player1ItemQuantityDictionary [itemName] += 1;
			SetItemOnScreen (_player1ItemName.text, true);
		} else {
			_player2ItemQuantityDictionary [itemName] += 1;	
			SetItemOnScreen (_player2ItemName.text, false);
		}
	}

	void SetItemOnScreen(string itemName, bool player1) {
		if (player1) {
			_player1CurrentItem.sprite = _itemImageDictionary [itemName];
			_player1ItemName.text = itemName;
			_player1ItemQuantity.text = "" + _player1ItemQuantityDictionary [itemName];
		} else {
			_player2CurrentItem.sprite = _itemImageDictionary[itemName];
			_player2ItemName.text = itemName;
			_player2ItemQuantity.text = "" + _player2ItemQuantityDictionary [itemName];
		}
	}
}
