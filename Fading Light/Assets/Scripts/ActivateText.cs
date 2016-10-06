using UnityEngine;
using System.Collections;

/// <summary>
/// 
/// </summary>
public class ActivateText : MonoBehaviour {
	public TextAsset Dialogue;

	public int StartLine;
	public int EndLine;

	public TextBoxManager TextBox;
	// Use this for initialization

	public bool RequireButtonPress;
	private bool _waitForPress;
	public bool DestroyWhenActivated;
	public AudioClip[] DialogueSpeech;
	public bool TalkOnce;
	private bool _talked;
	void Start () {
		_talked = false;
	}

    // Update is called once per frame
    /// <summary>
    /// Updates this instance.
    /// </summary>
    void Update () {
		if (_waitForPress && Input.GetKeyDown (KeyCode.T)) {
			if ((TalkOnce == true && _talked == false) || (TalkOnce == false)) {
				print ("IN HERE UPDATE");
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
    /// Called when [trigger enter].
    /// </summary>
    /// <param name="other">The other.</param>
    void OnTriggerEnter(Collider other)
	{
		if (RequireButtonPress == true) {
			_waitForPress = true;
			return;
		}
		if (other.name == "Player 1" || other.name == "Player2") {
			if ((TalkOnce == true && _talked == false) || (TalkOnce == false)) {
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
    /// Called when [trigger exit].
    /// </summary>
    /// <param name="other">The other.</param>
    void OnTriggerExit(Collider other){
		if (other.name == "Player 1" || other.name == "Player2") {
			print ("EXITING");
			_waitForPress = false;
		}
	}
}
