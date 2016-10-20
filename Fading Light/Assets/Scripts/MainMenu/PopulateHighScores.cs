// file:	Assets\Scripts\MainMenu\PopulateHighScores.cs
//
// summary:	Implements the populate high scores class

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>   A populate high scores. </summary>
///
/// <remarks>    . </remarks>

public class PopulateHighScores : MonoBehaviour
{
    /// <summary>   The grid. </summary>
    public GameObject Grid;
    /// <summary>   The GameObject to process. </summary>
    public GameObject B;
    /// <summary>   The list item. </summary>
    public GameObject ListItem;
    /// <summary>   The database interacter. </summary>
    private DatabaseScores databaseInteracter = new DatabaseScores();
    /// <summary>   True if this object is done. </summary>
    private bool isDone = false;
    /// <summary>   The scores. </summary>
    List<HighScore> _scores = new List<HighScore>();

    /// <summary>   Starts this object. </summary>
    ///
 

    void Start()
    {
        StartCoroutine(databaseInteracter.GetScores());
    }

    /// <summary>   The time direction multiplier. </summary>
    private int _timeDirectionMultiplier = -1;
    /// <summary>   The gold direction multiplier. </summary>
    private int _goldDirectionMultiplier = 1;
    /// <summary>   The accuracy direction multiplier. </summary>
    private int _accuracyDirectionMultiplier = 1;
    /// <summary>   The deaths direction multiplier. </summary>
    private int _deathsDirectionMultiplier = 1;

    /// <summary>   Sort list. </summary>
    ///
 
    ///
    /// <param name="buttonName">   Name of the button. </param>

    public void SortList(string buttonName)
    {
        if (buttonName.Equals("Time"))
        {
            //Sorting by gold
            _scores.Sort((x, y) => _timeDirectionMultiplier * x.GetTotalSeconds().CompareTo(y.GetTotalSeconds()));
            _timeDirectionMultiplier = _timeDirectionMultiplier * -1;
            ReloadData();
        }

        if (buttonName.Equals("Gold"))
        {
            //Sorting by gold
            _scores.Sort((x, y) => _goldDirectionMultiplier * int.Parse(y.gold).CompareTo(int.Parse(x.gold)));
            _goldDirectionMultiplier = _goldDirectionMultiplier * -1;
            ReloadData();
        }

        if (buttonName.Equals("Accuracy"))
        {
            //Sorting by gold
            _scores.Sort((x, y) => _accuracyDirectionMultiplier * (float.Parse(y.p1accuracy) + float.Parse(y.p2accuracy) / 2).CompareTo(float.Parse(x.p1accuracy) + float.Parse(x.p2accuracy) / 2));
            _accuracyDirectionMultiplier = _accuracyDirectionMultiplier * -1;
            ReloadData();
        }

        if (buttonName.Equals("Deaths"))
        {
            //Sorting by gold
            _scores.Sort((x, y) => _deathsDirectionMultiplier * x.timeskilled.CompareTo(y.timeskilled));
            _deathsDirectionMultiplier = _deathsDirectionMultiplier * -1;
            ReloadData();
        }
    }

    /// <summary>   Reload data. </summary>
    ///
 

    void ReloadData()
    {
        var count = 0;
        foreach (Transform child in Grid.transform)
        {
            count++;

            if (count == 1)
            {
                continue;
            }

            Destroy(child.gameObject);
        }

        foreach (var highscore in _scores)
        {
            var listItem = Instantiate(ListItem) as GameObject;
            var panel = (HighScorePanel)listItem.GetComponent(typeof(HighScorePanel));

            if (panel != null && highscore != null)
            {
                panel.Score = highscore;
                panel.Populate();
                panel.transform.parent = Grid.transform;
                panel.transform.localScale = Vector3.one;
            }
        }
    }

    /// <summary>   Updates this object. </summary>
    ///
 

    void Update()
    {
        
        if(!databaseInteracter.IsDone || isDone)
        {
            return;
        }

        _scores = databaseInteracter.GetResults();

        _scores.Remove(null);

        isDone = true;

        ReloadData();  
    }

}
