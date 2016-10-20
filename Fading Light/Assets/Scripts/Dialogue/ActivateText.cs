// file:	Assets\Scripts\Dialogue\ActivateText.cs
//
// summary:	Implements the activate text class

using UnityEngine;
using System.Collections;

/// <summary>
/// This class will be placed on characters or objects which have dialogue on them. Referenced -
/// www.youtube.com/watch?v=ehmBIP5sj0M.
/// </summary>
///
/// <remarks>    . </remarks>

public class ActivateText : MonoBehaviour
{
    /// <summary>   The dialogue. </summary>
    public TextAsset Dialogue;

    /// <summary>   The start line. </summary>
    public int StartLine;
    /// <summary>   The end line. </summary>
    public int EndLine;

    /// <summary>   The text box. </summary>
    public TextBoxManager TextBox;
    // Use this for initialization

    /// <summary>   True to require button press. </summary>
	public bool RequireButtonPress;
    /// <summary>   True to wait for press. </summary>
	private bool _waitForPress;
    /// <summary>   True if destroy when activated. </summary>
	public bool DestroyWhenActivated;
    /// <summary>   The dialogue speech. </summary>
	public AudioClip[] DialogueSpeech;
    /// <summary>   True to talk once. </summary>
	public bool TalkOnce;
    /// <summary>   True to notify. </summary>
    public bool Notify = true;
    /// <summary>   True if talked. </summary>
    private bool _talked;

    /// <summary>   Starts this object. </summary>
    ///
 

	void Start () {
        Debug.Log(Notify);
		_talked = false;
	}

    /// <summary>   Detects Keypress and activates dialogue when detected. </summary>
    ///
 

	void Update () {
        //This will detect whether a keypress is required and execute when the condition is fulfilled. 
		if (_waitForPress && (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.O)) && TextBox.Talking == false) {
			//This will check if the character can only talk once or not and check if it has talked.
			if ((TalkOnce == true && _talked == false) || (TalkOnce == false)) {
				//This will execute the functions in TextBoxManager class and load the new dialogue.
				TextBox.ReloadScript (Dialogue,DialogueSpeech, Notify);
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
    ///
 
    ///
    /// <param name="other">    Other. </param>

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
                _talked = true;
                //Debug.Log("called");
                //This will execute the functions in TextBoxManager class and load the new dialogue.
                Debug.Log(Notify);
                TextBox.ReloadScript (Dialogue, DialogueSpeech, Notify);
				TextBox.CurrentLine = StartLine;
				TextBox.EndLine = EndLine;
				TextBox.EnableDialogue ();

				if (DestroyWhenActivated) {
                    //Debug.Log("Death");
                    
					Destroy (gameObject);
				}
			}
		}

	}

    /// <summary>
    /// Ensures that if a player leaves the radius of a NPC, pressing the talk key (T) won't enable
    /// dialogue.
    /// </summary>
    ///
 
    ///
    /// <param name="other">    Other. </param>

	void OnTriggerExit(Collider other){
		if (other.name == "Player 1" || other.name == "Player2") {
			//print ("EXITING");
			_waitForPress = false;
		}
	}
}