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
    // Use this for initialization
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
           
            GameObject newAchievement = Instantiate(ListItem) as GameObject;
            HighScorePanel controller = (HighScorePanel)newAchievement.GetComponent(typeof(HighScorePanel));

            if (controller != null)
            {
                Debug.Log("ACHIEVEMENT");
                controller.Name.text = highscore.name;
                controller.Description.text = highscore.gold;
                newAchievement.transform.parent = Grid.transform;
                newAchievement.transform.localScale = Vector3.one;
            }
        }

        
    }

}
