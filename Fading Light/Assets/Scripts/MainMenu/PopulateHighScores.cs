using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PopulateHighScores : MonoBehaviour
{
    public GameObject Grid;
    public GameObject ListItem;
    private DatabaseScores databaseInteracter = new DatabaseScores();
    private bool isDone = false;
    List<HighScore> _scores = new List<HighScore>();

    void Start()
    {
        StartCoroutine(databaseInteracter.GetScores());
    }

    void Update()
    {
        if(!databaseInteracter.IsDone || isDone)
        {
            return;
        }

        _scores = databaseInteracter.GetResults();


        isDone = true;

        foreach (var highscore in _scores)
        {
           
            GameObject listItem = Instantiate(ListItem) as GameObject;
            HighScorePanel panel = (HighScorePanel)listItem.GetComponent(typeof(HighScorePanel));

            if (panel != null && highscore != null)
            {
                panel.Score = highscore;
                panel.Populate();
                panel.transform.parent = Grid.transform;
                panel.transform.localScale = Vector3.one;
            }
        }

        
    }

}
