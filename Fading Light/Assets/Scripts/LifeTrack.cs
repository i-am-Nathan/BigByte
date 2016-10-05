using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Used to enable persistent data throughout scenes
public class LifeTrack : MonoBehaviour {

    private int _numberOfLivesLeft;
    private float _totalTime;
    private Text _totalTimeText;

    void Awake()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Life Track");
        _totalTimeText = GameObject.FindWithTag("Total Time").GetComponent<Text>();

        // Checking if this object already exists
        if (!(objects.Length > 0))
        {
            // Assigning a tag and instantiating number of lives
            _numberOfLivesLeft = 3;
            this.tag = "Life Track";
            _totalTime = 0f;
            DontDestroyOnLoad(this);
        }
    }

    void Update()
    {
        SetTime();
    }

    // Getters and setters for the number of total lives
    public int GetNumberOfLives()
    {
        return _numberOfLivesLeft;
    }

    public void SetNumberOfLives(int lives)
    {
        _numberOfLivesLeft = lives;
    }

    private void SetTime()
    {
        // Getting current time since users started playing 
        _totalTime += Time.deltaTime;

      

        



        //float minutes = Mathf.Floor(_totalTime / 60); float seconds = Mathf.RoundToInt(_totalTime % 60);


        //if (minutes < 10) { minutes = "0" + minutes.ToString(); }
        //if (seconds < 10) { seconds = "0" + Mathf.RoundToInt(seconds).ToString(); }
        //_totalTimeText.text = "Time:  " + minutes + ":" + seconds;

    }
}
