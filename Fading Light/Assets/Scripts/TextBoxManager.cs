using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
public class TextBoxManager : MonoBehaviour {

	public GameObject textBox;
	public Text dialogue;
	public TextAsset textFile;
	public string[] textLines;
	private string[] splitText;
	public Image characterImage;
	public Text characterName;
	public Sprite[] images;
	private Dictionary<string,Sprite> myDictionary = new Dictionary<string,Sprite>();
	private Dictionary<string,string> characterNames = new Dictionary<string,string>();
	public int currentLine;
	public int endAtLine;
	public PlayerController player1;
	public Player2Controller player2;

	public bool stopMovement;
	public bool isActive;
	public bool firstline;

	private bool isTyping = false;
	private bool cancelTyping = false;

	public AudioClip typeSound;
	private AudioSource source;

	public float typeSpeed;
	// Use this for initialization
	void Start () {
		myDictionary.Add ("MM", images [0]);
		myDictionary.Add ("BS", images [1]);
		myDictionary.Add ("LS", images [2]);
		characterNames.Add ("MM", "Mole Man");
		characterNames.Add ("BS", "Big Sibling");
		characterNames.Add ("LS", "Little Sibling");
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

	void Awake(){
		source = GetComponent<AudioSource>();
	}
	// Update is called once per frame
	void Update () {
		if (!isActive) {
			return;
		}
			

		if (firstline) {
			splitText= new string[2];
			splitText = textLines [currentLine].Split (':');
			characterImage.sprite = myDictionary [splitText [0]];
			characterName.text = characterNames [splitText [0]];
			//StartCoroutine (TextScroll (textLines [currentLine]));
			StartCoroutine (TextScroll (splitText[1]));
			firstline = false;
		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			if (!isTyping) {
				currentLine += 1;

				if (currentLine > endAtLine) {
					DisableDialogue ();
				} else {
					splitText= new string[2];
					splitText = textLines [currentLine].Split (':');
					characterImage.sprite = myDictionary [splitText [0]];
					characterName.text = characterNames [splitText [0]];
					StartCoroutine (TextScroll (splitText[1]));
					//StartCoroutine (TextScroll (textLines [currentLine]));
					//dialogue.text = textLines [currentLine];
				}

			} else if (isTyping && !cancelTyping) {
				cancelTyping = true;
			}
				
		} 




	}

	private IEnumerator TextScroll (string lineOfText){
		int letter = 0;
		dialogue.text = "";
		isTyping = true;
		cancelTyping = false;
		while (isTyping && !cancelTyping && (letter < lineOfText.Length - 1)) {
			
			dialogue.text += lineOfText [letter];
			source.PlayOneShot (typeSound);
			letter += 1;
			yield return new WaitForSeconds (typeSpeed);
		}
		dialogue.text = lineOfText;

		isTyping = false;
		cancelTyping = false;
	}

	public void EnableDialogue(){
		textBox.SetActive (true);
		isActive = true;
			player1.IsDisabled = true;
			player2.IsDisabled = true;
	}

	public void DisableDialogue(){
		textBox.SetActive (false);
		isActive = false;
		player1.IsDisabled = false;
		player2.IsDisabled = false;
	}

	public void ReloadScript(TextAsset thisText){
		if (thisText != null) {
			textLines = new string[1];
			textLines = (thisText.text.Split ('\n'));	
			firstline = true;

		}
	}
}
