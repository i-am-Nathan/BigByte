// file:	assets\scripts\dialogue\textboxmanager.cs
//
// summary:	Implements the textboxmanager class

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// This class will manage the dialogue system and handles the textfiles associated with it. This
/// file handles dialogue through a specific format :
/// CharacterAbbreviation:Sound(Optional):Dialogue Referenced -
/// www.youtube.com/watch?v=ehmBIP5sj0M.
/// </summary>
///
/// <remarks>    . </remarks>

public class TextBoxManager : MonoBehaviour {

    /// <summary>   The text box. </summary>
	public GameObject TextBox;
    /// <summary>   The dialogue. </summary>
	public Text Dialogue;
    /// <summary>   The text file. </summary>
	public TextAsset TextFile;
    /// <summary>   The text lines. </summary>
	public string[] TextLines;
    /// <summary>   The split text. </summary>
	private string[] _splitText;
    /// <summary>   The character image. </summary>
	public Image CharacterImage;
    /// <summary>   True to talking. </summary>
    public bool Talking = false;
    /// <summary>   Name of the character. </summary>
	public Text CharacterName;
    /// <summary>   The images. </summary>
	public Sprite[] Images;
    /// <summary>   Dictionary of sprites. </summary>
	private Dictionary<string,Sprite> SpriteDictionary = new Dictionary<string,Sprite>();
    /// <summary>   The character name dictionairy. </summary>
	private Dictionary<string,string> CharacterNameDictionairy = new Dictionary<string,string>();
    /// <summary>   The game user interface. </summary>
	private GameObject _gameUI;
    /// <summary>   The dialogue sounds. </summary>
	private AudioClip[] _dialogueSounds;
    /// <summary>   The current clip. </summary>
	private int _currentClip;


    /// <summary>   The current line. </summary>
	public int CurrentLine;
    /// <summary>   The end line. </summary>
	public int EndLine;
    /// <summary>   The first player. </summary>
	public PlayerController Player1;
    /// <summary>   The second player. </summary>
	public Player2Controller Player2;


    /// <summary>   this storyline. </summary>
    public Storyline ThisStoryline;
    /// <summary>   The torch controller. </summary>
    public TorchFuelController TorchController;

    /// <summary>   True to notify storyline. </summary>
    public bool NotifyStoryline = true;

    /// <summary>   True to stop movement. </summary>
    public bool StopMovement;
    /// <summary>   True if this object is active. </summary>
	public bool IsActive;
    /// <summary>   True to first line. </summary>
	public bool FirstLine;

    /// <summary>   True if this object is typing. </summary>
	private bool _isTyping = false;
    /// <summary>   True to cancel typing. </summary>
	private bool _cancelTyping = false;

    /// <summary>   The type sound. </summary>
	public AudioClip TypeSound;
    /// <summary>   Source for the. </summary>
	private AudioSource _source;

    /// <summary>   The type speed. </summary>
	public float TypeSpeed;

    /// <summary>   Start this instance. </summary>
    ///
 

	void Start () {
		_gameUI = GameObject.FindGameObjectWithTag ("GameUIWrapper");
		SpriteDictionary.Add ("MM", Images [0]);
		SpriteDictionary.Add ("BS", Images [1]);
		SpriteDictionary.Add ("LS", Images [2]);
		SpriteDictionary.Add ("PB", Images [3]);
        SpriteDictionary.Add("PS", Images[4]);
        CharacterNameDictionairy.Add ("MM", "Mole Man");
		CharacterNameDictionairy.Add ("BS", "Big Sibling");
		CharacterNameDictionairy.Add ("LS", "Little Sibling");
		CharacterNameDictionairy.Add ("PB", "Post Board");
        CharacterNameDictionairy.Add("PS", "Prisoner");

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

    /// <summary>   Gets the audio source component. </summary>
    ///
 

	void Awake(){
		_source = GetComponent<AudioSource>();
	}

    /// <summary>
    /// This will detect whether there is more dialogue to go and also print the dialogue to the
    /// screen. Space is used to navigate through the dialogue.
    /// </summary>
    ///
 

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

    /// <summary>   This will make the characters in each dialogue line appear one by one. </summary>
    ///
 
    ///
    /// <param name="lineOfText">   Line of text. </param>
    ///
    /// <returns>   The scroll. </returns>

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
    ///
 

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

    /// <summary>   This will stop the dialogue and allow the players to move again. </summary>
    ///
 

	public void DisableDialogue(){
        ThisStoryline.CharacterDamageEnabled(true);
        _gameUI.SetActive (true);
		TextBox.SetActive (false);
		IsActive = false;
		Player1.IsDisabled = false;
		Player2.IsDisabled = false;


        if (NotifyStoryline)
        {
            ThisStoryline.EnableMoleMan();
        }
        
        TorchController.IsDisabled = false;
    }

    /// <summary>   This will load a new dialogue script. </summary>
    ///
 
    ///
    /// <param name="thisText">     This text. </param>
    /// <param name="audioClips">   Audio clips. </param>
    /// <param name="notify">       True to notify. </param>

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
