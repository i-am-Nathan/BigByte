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

	private Sprite[] _player1Items;
	private Sprite[] _player2Items;

	private PlayerController _player1ControllerScript;
	private Player2Controller _player2ControllerScript;

	private Dictionary<string,int> _itemQuantityDictionary = new Dictionary<string,int>();

	void Start() {
		GameObject go = GameObject.FindGameObjectWithTag("Player");
		_player1ControllerScript = (PlayerController)go.GetComponent(typeof(PlayerController));

		GameObject go1 = GameObject.FindGameObjectWithTag("Player2");
		_player2ControllerScript = (Player2Controller)go1.GetComponent(typeof(Player2Controller));

		_player1CurrentItem = GameObject.FindWithTag("Player 1 SubInventory").GetComponent<Image>();
		_player2CurrentItem = GameObject.FindWithTag("Player 2 SubInventory").GetComponent<Image>();

		_player1Items = new Sprite[_totalNumItems];
		_player1Items [0] = HealthImage;
		_player1Items [1] = DefenseImage;
		_player1Items [2] = BerserkImage;

		_player2Items = new Sprite[_totalNumItems];
		_player2Items [0] = HealthImage;
		_player2Items [1] = DefenseImage;
		_player2Items [2] = BerserkImage;

		_player1CurrentItem.sprite = HealthImage;
		_player2CurrentItem.sprite = HealthImage;

		_itemQuantityDictionary.Add ("Health Pot", 0);
		_itemQuantityDictionary.Add ("Attack Pot", 0);
		_itemQuantityDictionary.Add ("Defense Pot", 0);
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

		if (Input.GetKeyDown(KeyCode.K))
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
			_player1CurrentItem.sprite = _player1Items [_player1CurrentItemIndex];	
		} else {
			// Cycle through player 2's sub inventory
			if (_player2CurrentItemIndex != _totalNumItems-1) {
				_player2CurrentItemIndex += 1;		
			} else {
				_player2CurrentItemIndex = 0;
			}
			_player2CurrentItem.sprite = _player2Items [_player2CurrentItemIndex];
		}
	}

	void UseItem (bool player1) {
		if (player1) {
			if (_player1CurrentItemIndex == 0) {
				//HealthPotActivated
				//_player1ControllerScript.HealthPotActivated();
			} else if (_player1CurrentItemIndex == 1) {
				//AttackPotActivated
				//_player1ControllerScript.AttackPotActivated();
			} else if (_player1CurrentItemIndex == 2) {
				//DefensePotActivated
				//_player1ControllerScript.DefensePotActivated();
			}
		} else {
			if (_player2CurrentItemIndex == 0) {
				//HealthPotActivated
				//_player2ControllerScript.HealthPotActivated();
			} else if (_player2CurrentItemIndex == 1) {
				//AttackPotActivated
				//_player2ControllerScript.AttackPotActivated();
			} else if (_player2CurrentItemIndex == 2) {
				//DefensePotActivated
				//_player2ControllerScript.DefensePotActivated();
			}
		}
	}
}
