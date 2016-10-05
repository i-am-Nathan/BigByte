using UnityEngine;
using System.Collections;

public class LifeTrack : MonoBehaviour {

    private int _numberOfLivesLeft;

    void Awake()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Life Track");

        if (!(objects.Length > 0))
        {
            Debug.LogWarning("Making new one");
            _numberOfLivesLeft = 3;
            this.tag = "Life Track";
            DontDestroyOnLoad(this);
        }
    }

    public int GetNumberOfLives()
    {
        return _numberOfLivesLeft;
    }

    public void SetNumberOfLives(int lives)
    {
        _numberOfLivesLeft = lives;
    }
}
