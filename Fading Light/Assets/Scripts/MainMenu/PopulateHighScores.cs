using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PopulateHighScores : MonoBehaviour
{
    public GameObject Grid;
    public GameObject B;
    public GameObject ListItem;
    private DatabaseScores databaseInteracter = new DatabaseScores();
    private bool isDone = false;
    List<HighScore> _scores = new List<HighScore>();

    void Start()
    {
        StartCoroutine(databaseInteracter.GetScores());
    }

    private int _timeDirectionMultiplier = -1;
    private int _goldDirectionMultiplier = 1;
    private int _accuracyDirectionMultiplier = 1;
    private int _deathsDirectionMultiplier = 1;


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
