using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class TextBoxManager : MonoBehaviour {

	public GameObject textBox;
	public Text dialogue;
	public TextAsset textFile;
	public string[] textLines;

	public int currentLine;
	public int endAtLine;

	public bool isActive;
	// Use this for initialization
	void Start () {
		if (textFile != null) {
			textLines = (textFile.text.Split ('\n'));	
		}

		if (endAtLine == 0) {
			endAtLine = textLines.Length - 1;
		}

		if (isActive) {
			EnableDialogue ();
		} else {
			DisableDialogue ();
		}
	}

	// Update is called once per frame
	void Update () {
		if (!isActive) {
			return;
		}
		if (currentLine > endAtLine) {
			DisableDialogue ();
		}
		dialogue.text = textLines [currentLine];

		if (Input.GetKeyDown (KeyCode.T)) {
			currentLine += 1;
		}



	}

	void EnableDialogue(){
		textBox.SetActive (true);
	}

	void DisableDialogue(){
		textBox.SetActive (false);
	}
}
