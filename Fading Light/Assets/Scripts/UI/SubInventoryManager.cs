// file:	assets\scripts\ui\subinventorymanager.cs
//
// summary:	Implements the subinventorymanager class

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Class used to display and keep track of the players's sub inventory (i.e. their potions)
/// through a level.
/// </summary>
///
/// <remarks>    . </remarks>

public class SubInventoryManager : MonoBehaviour {

	// Images for potions
    /// <summary>   The health image. </summary>
	public Sprite HealthImage;
    /// <summary>   The defense image. </summary>
	public Sprite DefenseImage;
    /// <summary>   The berserk image. </summary>
	public Sprite BerserkImage;

    /// <summary>   The total number items. </summary>
	private int _totalNumItems = 3;
    /// <summary>   True to in shop. </summary>
	private bool _inShop = false;

	// The current item the player has
    /// <summary>   Zero-based index of the player 1 current item. </summary>
	private int _player1CurrentItemIndex = 0;
    /// <summary>   Zero-based index of the player 2 current item. </summary>
	private int _player2CurrentItemIndex = 0;
    /// <summary>   The player 1 current item. </summary>
	private Image _player1CurrentItem;
    /// <summary>   The player 2 current item. </summary>
	private Image _player2CurrentItem;
    /// <summary>   Name of the player 1 item. </summary>
	private Text _player1ItemName;
    /// <summary>   Name of the player 2 item. </summary>
	private Text _player2ItemName;
    /// <summary>   The player 1 item quantity. </summary>
	private Text _player1ItemQuantity;
    /// <summary>   The player 2 item quantity. </summary>
	private Text _player2ItemQuantity;

	// All sprites which will be cycle through
    /// <summary>   The player 1 items. </summary>
	private Sprite[] _player1Items;
    /// <summary>   The player 2 items. </summary>
	private Sprite[] _player2Items;

    /// <summary>   The player 1 controller script. </summary>
	private PlayerController _player1ControllerScript;
    /// <summary>   The player 2 controller script. </summary>
	private Player2Controller _player2ControllerScript;

	// Holds how much of each item a player has
    /// <summary>   Dictionary of player 1 item quantities. </summary>
	private Dictionary<string,int> _player1ItemQuantityDictionary;
    /// <summary>   Dictionary of player 2 item quantities. </summary>
	private Dictionary<string,int> _player2ItemQuantityDictionary;

	// Holds the sprite of the item to allow cycling of images
    /// <summary>   Dictionary of item images. </summary>
	private Dictionary<string,Sprite> _itemImageDictionary = new Dictionary<string,Sprite>();
    /// <summary>   Dictionary of item index names. </summary>
	private Dictionary<int,string> _itemIndexNameDictionary = new Dictionary<int,string>();

    /// <summary>   The game data script. </summary>
	private GameData _gameDataScript;

    /// <summary>   Initialises all data required for the sub inventory system. </summary>
    ///
 

	void Start() {
		// Obtaining item quantity data from game data
		GameObject go = GameObject.FindGameObjectWithTag("Game Data");
		_gameDataScript = (GameData)go.GetComponent(typeof(GameData));
		_player1ItemQuantityDictionary = _gameDataScript.GetPlayer1ItemQuantityDictionary ();
		_player2ItemQuantityDictionary = _gameDataScript.GetPlayer2ItemQuantityDictionary ();

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

		// Adding the potions to the relevant dictionaries
		_itemIndexNameDictionary.Add (0, "Health Potion");
		_itemIndexNameDictionary.Add (1, "Attack Potion");
		_itemIndexNameDictionary.Add (2, "Defense Potion");

		_itemImageDictionary.Add ("Health Potion", HealthImage);
		_itemImageDictionary.Add ("Attack Potion", BerserkImage);
		_itemImageDictionary.Add ("Defense Potion", DefenseImage);

		// Setting the health potion to show on screen
		SetItemOnScreen ("Health Potion", true);
		SetItemOnScreen ("Health Potion", false);
	}

    /// <summary>   Update is called once per frame. </summary>
    ///
 

	void Update () {
		// Player 1 cycling through their items
		if (Input.GetKeyDown (KeyCode.K)) {
			CycleItems (true);
		}

		// Player 2 cycling through their items
		if (Input.GetKeyDown (KeyCode.X)) {
			CycleItems (false);
		}

		// Player 1 using their items
		if (Input.GetKeyDown (KeyCode.L) && !_inShop) {
			UseItem (true);
		}

		// Player 2 using their items
		if (Input.GetKeyDown (KeyCode.C) && !_inShop) {
			UseItem (false);
		}
	}

    /// <summary>   Called to allow players to cycle through their items. </summary>
    ///
 
    ///
    /// <param name="cyclePlayer1"> True to cycle player 1. </param>

	void CycleItems (bool cyclePlayer1) {
		// Cycle through for player 1's sub inventory
		if (cyclePlayer1) {
			if (_player1CurrentItemIndex != _totalNumItems-1) {
				_player1CurrentItemIndex += 1;		
			} else {
				_player1CurrentItemIndex = 0;
			}
			// Setting the item on screen
			SetItemOnScreen(_itemIndexNameDictionary[_player1CurrentItemIndex], true);	
		} else {
			// Cycle through player 2's sub inventory
			if (_player2CurrentItemIndex != _totalNumItems-1) {
				_player2CurrentItemIndex += 1;		
			} else {
				_player2CurrentItemIndex = 0;
			}
			// Setting the item on screen
			SetItemOnScreen(_itemIndexNameDictionary[_player2CurrentItemIndex], false);
		}
	}

    /// <summary>
    /// Called to allow players to use
    ///  through their items.
    /// </summary>
    ///
 
    ///
    /// <param name="player1">  True to player 1. </param>

	public void UseItem (bool player1) {

        var _achievementManager = (AchievementManager)GameObject.FindGameObjectWithTag("AchievementManager").GetComponent(typeof(AchievementManager));
        _achievementManager.AchievementObtained("Thirsty.");

        if (player1) {
			if (_player1ItemQuantityDictionary [_player1ItemName.text] != 0) {
				_player1ItemQuantityDictionary[_player1ItemName.text] -= 1;

				// Updating in persistent game data
				_gameDataScript.SetPlayer1ItemQuantityDictionary (_player1ItemName.text, _player1ItemQuantityDictionary[_player1ItemName.text]);

				SetItemOnScreen (_player1ItemName.text, true);
				//HealthPotActivated
        	    if (_player1ItemName.text == "Health Potion")
        	    {
        	        _player1ControllerScript.HealthPotActivated();
        	    }
        	    //AttackPotActivated
        	    if (_player1ItemName.text == "Attack Potion")
        	    {
        	        _player1ControllerScript.AttackPotActivated();
        	    }
        	    //DefensePotActivated
        	    if (_player1ItemName.text == "Defense Potion")
        	    {
        	        _player1ControllerScript.DefensePotActivated();
        		}
			}
        } else {
			if (_player2ItemQuantityDictionary [_player2ItemName.text] != 0) {
				_player2ItemQuantityDictionary[_player2ItemName.text] -= 1;
				SetItemOnScreen (_player2ItemName.text, false);

				// Updating in persistent game data
				_gameDataScript.SetPlayer2ItemQuantityDictionary (_player2ItemName.text, _player2ItemQuantityDictionary[_player2ItemName.text]);

	            //HealthPotActivated
	            if (_player2ItemName.text == "Health Potion")
	            {
	                _player2ControllerScript.HealthPotActivated();
	            }
	            //AttackPotActivated
	            if (_player2ItemName.text == "Attack Potion")
	            {
	                _player2ControllerScript.AttackPotActivated();
	            }
	            //DefensePotActivated
	            if (_player2ItemName.text == "Defense Potion")
	            {
	                _player2ControllerScript.DefensePotActivated();
	            }
			}
		}	
	}

    /// <summary>   Increments the quantity of an item a player has. </summary>
    ///
 
    ///
    /// <param name="itemName"> Name of the item. </param>
    /// <param name="player1">  True to player 1. </param>

	public void AddItemQuantity (string itemName, bool player1) {
		if (player1) {
			// For player 1
			_player1ItemQuantityDictionary [itemName] += 1;
			// Updating in persistent game data
			_gameDataScript.SetPlayer1ItemQuantityDictionary (itemName, _player1ItemQuantityDictionary[itemName]);
			SetItemOnScreen (_player1ItemName.text, true);
		} else {
			// For player 2
			_player2ItemQuantityDictionary [itemName] += 1;	
			// Updating in persistent game data
			_gameDataScript.SetPlayer2ItemQuantityDictionary (itemName, _player2ItemQuantityDictionary[itemName]);
			SetItemOnScreen (_player2ItemName.text, false);
		}
	}

    /// <summary>   Displays the item on the UI. </summary>
    ///
 
    ///
    /// <param name="itemName"> Name of the item. </param>
    /// <param name="player1">  True to player 1. </param>

	void SetItemOnScreen(string itemName, bool player1) {
		if (player1) {
			// For player 1
			// Changing the sprite and text
			_player1CurrentItem.sprite = _itemImageDictionary [itemName];
			_player1ItemName.text = itemName;

			// Updating local player 1 item quantities
			_player1ItemQuantityDictionary = _gameDataScript.GetPlayer1ItemQuantityDictionary ();
			_player1ItemQuantity.text = "" + _player1ItemQuantityDictionary [itemName];
		} else {
			// For player 2
			_player2CurrentItem.sprite = _itemImageDictionary[itemName];
			_player2ItemName.text = itemName;

			// Updating local player 2 item quantities
			_player2ItemQuantityDictionary = _gameDataScript.GetPlayer2ItemQuantityDictionary ();

			_player2ItemQuantity.text = "" + _player2ItemQuantityDictionary [itemName];
		}
	}

	// Toggles whether the players are in "shop" mode or not

    /// <summary>   Toggle in shop. </summary>
    ///
 

	public void ToggleInShop () {
		_inShop = !_inShop;
	}
}
