using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// This class will manage the dialogue system and handles the textfiles associated with it. This file handles dialogue through
/// a specific format : CharacterAbbreviation:Sound(Optional):Dialogue
/// </summary>
public class TextBoxManager : MonoBehaviour
{

    public GameObject TextBox;
    public Text Dialogue;
    public TextAsset TextFile;
    public string[] TextLines;
    private string[] _splitText;
    public Image CharacterImage;
    public Text CharacterName;
    public Sprite[] Images;
    private Dictionary<string, Sprite> SpriteDictionary = new Dictionary<string, Sprite>();
    private Dictionary<string, string> CharacterNameDictionairy = new Dictionary<string, string>();
    GameObject gameUI;
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

    /// <summary>
    /// Start this instance.
    /// </summary>
    void Start()
    {
        //Grabs the Game UI
        gameUI = GameObject.FindGameObjectWithTag("GameUIWrapper");

        //Dictionairy which will be used to store the character names and images based on the dialogue.
        SpriteDictionary.Add("MM", Images[0]);
        SpriteDictionary.Add("BS", Images[1]);
        SpriteDictionary.Add("LS", Images[2]);
        SpriteDictionary.Add("PB", Images[3]);
        CharacterNameDictionairy.Add("MM", "Mole Man");
        CharacterNameDictionairy.Add("BS", "Big Sibling");
        CharacterNameDictionairy.Add("LS", "Little Sibling");
        CharacterNameDictionairy.Add("PB", "Post Board");


        if (TextFile != null)
        {
            TextLines = (TextFile.text.Split('\n'));
        }

        if (EndLine == 0)
        {
            EndLine = TextLines.Length - 1;
        }

        if (IsActive)
        {
            EnableDialogue();
        }
        else
        {
            DisableDialogue();
        }

    }

    /// <summary>
    /// Awake this instance.
    /// </summary>
    void Awake()
    {
        _source = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!IsActive)
        {
            return;
        }
        //Format of dialogue is going to be CharacterName:AudioClip:Dialogue

        if (FirstLine)
        {
            _splitText = new string[3];
            _splitText = TextLines[CurrentLine].Split(':');
            CharacterImage.sprite = SpriteDictionary[_splitText[0]];
            CharacterName.text = CharacterNameDictionairy[_splitText[0]];
            //StartCoroutine (TextScroll (textLines [currentLine]));
            if (_splitText[1] == "S")
            {
                print("PLAYING CLIP");
                _source.PlayOneShot(_dialogueSounds[_currentClip], 33f);
                _currentClip += 1;

            }
            StartCoroutine(TextScroll(_splitText[2]));
            FirstLine = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!_isTyping)
            {
                CurrentLine += 1;

                if (CurrentLine > EndLine)
                {
                    DisableDialogue();
                }
                else
                {
                    _splitText = new string[3];
                    _splitText = TextLines[CurrentLine].Split(':');
                    CharacterImage.sprite = SpriteDictionary[_splitText[0]];
                    CharacterName.text = CharacterNameDictionairy[_splitText[0]];
                    //StartCoroutine (TextScroll (textLines [currentLine]));
                    if (_splitText[1] == "S")
                    {
                        print("PLAYING CLIP NOW");
                        _source.PlayOneShot(_dialogueSounds[_currentClip], 33f);
                        _currentClip += 1;

                    }
                    StartCoroutine(TextScroll(_splitText[2]));
                    //StartCoroutine (TextScroll (textLines [currentLine]));
                    //dialogue.text = textLines [currentLine];
                }

            }
            else if (_isTyping && !_cancelTyping)
            {
                _cancelTyping = true;
            }

        }




    }

    /// <summary>
    /// This will make the letters appear one by one. If will also instantly display the whole text while its typing, if space bar is pressed. 
    /// </summary>
    /// <returns>The scroll.</returns>
    /// <param name="lineOfText">Line of text.</param>
    private IEnumerator TextScroll(string lineOfText)
    {
        int letter = 0;
        Dialogue.text = "";
        _isTyping = true;
        _cancelTyping = false;
        while (_isTyping && !_cancelTyping && (letter < lineOfText.Length - 1))
        {

            Dialogue.text += lineOfText[letter];
            _source.PlayOneShot(TypeSound, 1f);
            letter += 1;
            yield return new WaitForSeconds(TypeSpeed);
        }
        Dialogue.text = lineOfText;
        if (_splitText[1] == "S")
        {
            _source.Stop();
        }
        _isTyping = false;
        _cancelTyping = false;
    }

    /// <summary>
    /// This will enable the dialogue box and disable movement from the players. This will also hide the Game UI elements again on the screen.
    /// </summary>
    public void EnableDialogue()
    {
        gameUI.SetActive(false);
        TextBox.SetActive(true);
        IsActive = true;
        Player1.IsDisabled = true;
        Player2.IsDisabled = true;
    }

    /// <summary>
    /// This will disable the dialogue and enable movement from player again. This will also show the Game UI elements again on the screen.
    /// </summary>
    public void DisableDialogue()
    {
        gameUI.SetActive(true);
        TextBox.SetActive(false);
        IsActive = false;
        Player1.IsDisabled = false;
        Player2.IsDisabled = false;
    }

    /// <summary>
    /// This will load a new script for the dialogue.
    /// </summary>
    /// <param name="thisText">This text.</param>
    /// <param name="audioClips">Audio clips.</param>
    public void ReloadScript(TextAsset thisText, AudioClip[] audioClips)
    {
        if (thisText != null)
        {
            TextLines = new string[1];
            TextLines = (thisText.text.Split('\n'));
            FirstLine = true;
            _currentClip = 0;
            _dialogueSounds = audioClips;
        }
    }
}