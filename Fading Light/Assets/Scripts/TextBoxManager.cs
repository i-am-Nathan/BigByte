using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
public class TextBoxManager : MonoBehaviour {

	public GameObject TextBox;
	public Text Dialogue;
	public TextAsset TextFile;
	public string[] TextLines;
	private string[] _splitText;
	public Image CharacterImage;
	public Text CharacterName;
	public Sprite[] Images;
	private Dictionary<string,Sprite> SpriteDictionary = new Dictionary<string,Sprite>();
	private Dictionary<string,string> CharacterNameDictionairy = new Dictionary<string,string>();

	private AudioClip[] _dialogueSounds;
	private int _currentClip;

	public int CurrentLine;
	public int EndLine;
	public PlayerController Player1;
	public Player2Controller Player2;

	public bool StopMovement;
	public bool IsActive;
	public bool FirstLine;

	private bool _isTyping = false;
	private bool _cancelTyping = false;

	public AudioClip TypeSound;
	private AudioSource _source;

	public float TypeSpeed;
	// Use this for initialization
	void Start () {
		SpriteDictionary.Add ("MM", Images [0]);
		SpriteDictionary.Add ("BS", Images [1]);
		SpriteDictionary.Add ("LS", Images [2]);
		CharacterNameDictionairy.Add ("MM", "Mole Man");
		CharacterNameDictionairy.Add ("BS", "Big Sibling");
		CharacterNameDictionairy.Add ("LS", "Little Sibling");
		if (TextFile != null) {
			TextLines = (TextFile.text.Split ('\n'));	
		}

		if (EndLine == 0) {
			EndLine = TextLines.Length - 1;
		}

		if (IsActive) {
			EnableDialogue ();
		} else {
			DisableDialogue ();
		}

	}

	void Awake(){
		_source = GetComponent<AudioSource>();
	}
	// Update is called once per frame
	void Update () {
		if (!IsActive) {
			return;
		}
			//Format of dialogue is going to be CharacterName:AudioClip:Dialogue

		if (FirstLine) {
			_splitText= new string[3];
			_splitText = TextLines [CurrentLine].Split (':');
			CharacterImage.sprite = SpriteDictionary [_splitText [0]];
			CharacterName.text = CharacterNameDictionairy [_splitText [0]];
			//StartCoroutine (TextScroll (textLines [currentLine]));
			if (_splitText [1] == "S") {
				print ("PLAYING CLIP");
				_source.PlayOneShot (_dialogueSounds [_currentClip]);
				_currentClip += 1;

			}
			StartCoroutine (TextScroll (_splitText[2]));
			FirstLine = false;
		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			if (!_isTyping) {
				CurrentLine += 1;

				if (CurrentLine > EndLine) {
					DisableDialogue ();
				} else {
					_splitText= new string[3];
					_splitText = TextLines [CurrentLine].Split (':');
					CharacterImage.sprite = SpriteDictionary [_splitText [0]];
					CharacterName.text = CharacterNameDictionairy [_splitText [0]];
					//StartCoroutine (TextScroll (textLines [currentLine]));
					if (_splitText [1] == "S") {
						print ("PLAYING CLIP NOW");
						_source.PlayOneShot (_dialogueSounds [_currentClip]);
						_currentClip += 1;

					}
					StartCoroutine (TextScroll (_splitText[2]));
					//StartCoroutine (TextScroll (textLines [currentLine]));
					//dialogue.text = textLines [currentLine];
				}

			} else if (_isTyping && !_cancelTyping) {
				_cancelTyping = true;
			}
				
		} 




	}

	private IEnumerator TextScroll (string lineOfText){
		int letter = 0;
		Dialogue.text = "";
		_isTyping = true;
		_cancelTyping = false;
		while (_isTyping && !_cancelTyping && (letter < lineOfText.Length - 1)) {
			
			Dialogue.text += lineOfText [letter];
			_source.PlayOneShot (TypeSound);
			letter += 1;
			yield return new WaitForSeconds (TypeSpeed);
		}
		Dialogue.text = lineOfText;
		if (_splitText [1] == "S") {
			print ("STOP CLIP");
			_source.Stop();
		}
		_isTyping = false;
		_cancelTyping = false;
	}

	public void EnableDialogue(){
		TextBox.SetActive (true);
		IsActive = true;
			Player1.IsDisabled = true;
			Player2.IsDisabled = true;
	}

	public void DisableDialogue(){
		TextBox.SetActive (false);
		IsActive = false;
		Player1.IsDisabled = false;
		Player2.IsDisabled = false;
	}

	public void ReloadScript(TextAsset thisText, AudioClip[] audioClips){
		if (thisText != null) {
			TextLines = new string[1];
			TextLines = (thisText.text.Split ('\n'));	
			FirstLine = true;
			_currentClip = 0;
			_dialogueSounds = audioClips;
		}
	}
}
