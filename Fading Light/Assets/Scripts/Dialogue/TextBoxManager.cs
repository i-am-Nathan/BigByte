using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


/// <summary>
/// This class will manage the dialogue system and handles the textfiles associated with it. This file handles dialogue through
/// a specific format : CharacterAbbreviation:Sound(Optional):Dialogue
/// </summary>
public class TextBoxManager : MonoBehaviour {

	public GameObject TextBox;
	public Text Dialogue;
	public TextAsset TextFile;
	public string[] TextLines;
	private string[] _splitText;
	public Image CharacterImage;
    public bool Talking = false;
	public Text CharacterName;
	public Sprite[] Images;
	private Dictionary<string,Sprite> SpriteDictionary = new Dictionary<string,Sprite>();
	private Dictionary<string,string> CharacterNameDictionairy = new Dictionary<string,string>();
	private GameObject _gameUI;
	private AudioClip[] _dialogueSounds;
	private int _currentClip;


	public int CurrentLine;
	public int EndLine;
	public PlayerController Player1;
	public Player2Controller Player2;

    public MoleManContoller MoleMan;
    public Storyline ThisStoryline;
    public TorchFuelController TorchController;

    public bool NotifyStoryline = true;

    public bool StopMovement;
	public bool IsActive;
	public bool FirstLine;

	private bool _isTyping = false;
	private bool _cancelTyping = false;

	public AudioClip TypeSound;
	private AudioSource _source;

	public float TypeSpeed;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () {
		_gameUI = GameObject.FindGameObjectWithTag ("GameUIWrapper");
		SpriteDictionary.Add ("MM", Images [0]);
		SpriteDictionary.Add ("BS", Images [1]);
		SpriteDictionary.Add ("LS", Images [2]);
		SpriteDictionary.Add ("PB", Images [3]);
		CharacterNameDictionairy.Add ("MM", "Mole Man");
		CharacterNameDictionairy.Add ("BS", "Big Sibling");
		CharacterNameDictionairy.Add ("LS", "Little Sibling");
		CharacterNameDictionairy.Add ("PB", "Post Board");


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

	/// <summary>
	/// Gets the audio source component.
	/// </summary>
	void Awake(){
		_source = GetComponent<AudioSource>();
	}

	/// <summary>
	/// This will detect whether there is more dialogue to go and also print the dialogue to the screen. Space is used to navigate through the dialogue.
	/// </summary>
	void Update () {
		if (!IsActive) {
			return;
		}
			//Format of dialogue is going to be CharacterName:AudioClip:Dialogue

		//Ensures that the first line will also have the letter by letter feature.
		if (FirstLine) {
            Talking = true;
			_splitText= new string[3];
			_splitText = TextLines [CurrentLine].Split (':');
			CharacterImage.sprite = SpriteDictionary [_splitText [0]];
			CharacterName.text = CharacterNameDictionairy [_splitText [0]];
			//StartCoroutine (TextScroll (textLines [currentLine]));
			if (_splitText [1] == "S") {
				_source.PlayOneShot (_dialogueSounds [_currentClip],33f);
				_currentClip += 1;
			}
			StartCoroutine (TextScroll (_splitText[2]));
			FirstLine = false;
		}

		//This will detect when the space button is pressed execute the following command.
		if (Input.GetKeyDown (KeyCode.Space)) {
			if (!_isTyping) {
				CurrentLine += 1;

				//Checks whether it has reached the end of the required dialogue.
				if (CurrentLine > EndLine) {
					DisableDialogue ();
                    Talking = false;
                    if (NotifyStoryline)
                    {
                        ThisStoryline.DialogueComplete();
                    }
                    
				} else {
					//Splits the string and assigns the appropriate character image and name on the dialogue.
					_splitText= new string[3];
					_splitText = TextLines [CurrentLine].Split (':');
					CharacterImage.sprite = SpriteDictionary [_splitText [0]];
					CharacterName.text = CharacterNameDictionairy [_splitText [0]];
					//StartCoroutine (TextScroll (textLines [currentLine]));
					//Detects if sound is required during this part of the dialogue.
					if (_splitText [1] == "S") {
						_source.PlayOneShot (_dialogueSounds [_currentClip],33f);
						_currentClip += 1;

					}
					StartCoroutine (TextScroll (_splitText[2]));
					//StartCoroutine (TextScroll (textLines [currentLine]));
					//dialogue.text = textLines [currentLine];
				}

			//This will display the whole line of dialogue if spaced is pressed and it is still typing.
			} else if (_isTyping && !_cancelTyping) {
				_cancelTyping = true;
			}
				
		} 




	}

	/// <summary>
	/// This will make the characters in each dialogue line appear one by one.
	/// </summary>
	/// <returns>The scroll.</returns>
	/// <param name="lineOfText">Line of text.</param>
	private IEnumerator TextScroll (string lineOfText){
		int letter = 0;
		Dialogue.text = "";
		_isTyping = true;
		_cancelTyping = false;
		while (_isTyping && !_cancelTyping && (letter < lineOfText.Length - 1)) {
			
			Dialogue.text += lineOfText [letter];
			_source.PlayOneShot (TypeSound, 1f);
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

	/// <summary>
	/// This will bring up the chat dialogue and freeze the movements of the characters and moleman.
	/// </summary>
	public void EnableDialogue(){
		_gameUI.SetActive (false);
		TextBox.SetActive (true);
		IsActive = true;
		Player1.IsDisabled = true;
		Player2.IsDisabled = true;
        ThisStoryline.CharacterDamageEnabled(false);
        if (NotifyStoryline)
        {
            ThisStoryline.StartText();
        }
        TorchController.IsDisabled = true;
    }


	/// <summary>
	/// This will stop the dialogue and allow the players to move again.
	/// </summary>
	public void DisableDialogue(){
        ThisStoryline.CharacterDamageEnabled(true);
        _gameUI.SetActive (true);
		TextBox.SetActive (false);
		IsActive = false;
		Player1.IsDisabled = false;
		Player2.IsDisabled = false;
        MoleMan.IsDisabled = false;

        if (NotifyStoryline)
        {
            ThisStoryline.EnableMoleMan();
        }
        
        TorchController.IsDisabled = false;
    }
		
	/// <summary>
	/// This will load a new dialogue script
	/// </summary>
	/// <param name="thisText">This text.</param>
	/// <param name="audioClips">Audio clips.</param>
	public void ReloadScript(TextAsset thisText, AudioClip[] audioClips, bool notify){
        NotifyStoryline = notify;
		if (thisText != null) {
			TextLines = new string[1];
			TextLines = (thisText.text.Split ('\n'));	
			FirstLine = true;
			_currentClip = 0;
			_dialogueSounds = audioClips;
		}
	}
}
