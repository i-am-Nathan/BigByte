using UnityEngine;
using System.Collections;

/// <summary>
/// This class will be placed on characters or objects which have dialogue on them. 
/// </summary>
public class ActivateText : MonoBehaviour {
	public TextAsset Dialogue;

	public int StartLine;
	public int EndLine;

	public TextBoxManager TextBox;

	public bool RequireButtonPress;
	private bool _waitForPress;
	public bool DestroyWhenActivated;
	public AudioClip[] DialogueSpeech;
	public bool TalkOnce;
	private bool _talked;
	void Start () {
		_talked = false;
	}
	

	void Update () {
		//This will detect whether a keypress is required and execute when the condition is fulfilled. 
		if (_waitForPress && Input.GetKeyDown (KeyCode.T)) {
			//This will check if the character can only talk once or not and check if it has talked.
			if ((TalkOnce == true && _talked == false) || (TalkOnce == false)) {
				//This will execute the functions in TextBoxManager class and load the new dialogue.
				TextBox.ReloadScript (Dialogue,DialogueSpeech);
				TextBox.CurrentLine = StartLine;
				TextBox.EndLine = EndLine;
				TextBox.EnableDialogue ();
				_talked = true;
				if (DestroyWhenActivated) {
					Destroy (gameObject);
				}
			}
		}
	}

	/// <summary>
	/// This method will execute when the players come close to the character with the dialogue.
	/// </summary>
	/// <param name="other">Other.</param>
	void OnTriggerEnter(Collider other)
	{
		//Checks if a key press is required to converse with the chracter
		if (RequireButtonPress == true) {
			_waitForPress = true;
			return;
		}
		//Makes sure that the characters that collide with the object are players.
		if (other.name == "Player 1" || other.name == "Player2") {
			if ((TalkOnce == true && _talked == false) || (TalkOnce == false)) {
				//This will execute the functions in TextBoxManager class and load the new dialogue.
				TextBox.ReloadScript (Dialogue,DialogueSpeech);
				TextBox.CurrentLine = StartLine;
				TextBox.EndLine = EndLine;
				TextBox.EnableDialogue ();
				_talked = true;
				if (DestroyWhenActivated) {
					Destroy (gameObject);
				}
			}
		}

}
	/// <summary>
	/// Ensures that if a player leaves the radius of a NPC, pressing the talk key (T) won't enable dialogue.
	/// </summary>
	/// <param name="other">Other.</param>
	void OnTriggerExit(Collider other){
		if (other.name == "Player 1" || other.name == "Player2") {
			print ("EXITING");
			_waitForPress = false;
		}
	}
}
