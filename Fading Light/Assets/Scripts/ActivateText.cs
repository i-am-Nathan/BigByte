using UnityEngine;
using System.Collections;

public class ActivateText : MonoBehaviour {
	public TextAsset dialogue;

	public int startLine;
	public int endLine;

	public TextBoxManager textBox;
	// Use this for initialization

	public bool requireButtonPress;
	private bool waitForPress;
	public bool destroyWhenActivated;

	public bool talkOnce;
	private bool talked;
	void Start () {
		talked = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (waitForPress && Input.GetKeyDown (KeyCode.T)) {
			if ((talkOnce == true && talked == false) || (talkOnce == false)) {
				print ("IN HERE UPDATE");
				textBox.ReloadScript (dialogue);
				textBox.currentLine = startLine;
				textBox.endAtLine = endLine;
				textBox.EnableDialogue ();
				talked = true;
				if (destroyWhenActivated) {
					Destroy (gameObject);
				}
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (requireButtonPress == true) {
			waitForPress = true;
			return;
		}
		if (other.name == "Player 1" || other.name == "Player2") {
			if ((talkOnce == true && talked == false) || (talkOnce == false)) {
				textBox.ReloadScript (dialogue);
				textBox.currentLine = startLine;
				textBox.endAtLine = endLine;
				textBox.EnableDialogue ();
				talked = true;
				if (destroyWhenActivated) {
					Destroy (gameObject);
				}
			}
		}

}
	void onTriggerExit(Collider other){
		if (other.name == "Player 1" || other.name == "Player2") {
			waitForPress = false;
		}
	}
}
