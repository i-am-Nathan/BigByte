using UnityEngine;
using System.Collections;

public class EndOfLevelTrigger : MonoBehaviour {

    public Canvas LevelFinishedMenu;

    void Start()
    {
        LevelFinishedMenu.enabled = false;
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag == "Player" || c.gameObject.tag == "Player2")
        {
            // Pauses the game and shows the menu indicating that players have completed the level
            Time.timeScale = 0;
            LevelFinishedMenu.enabled = true;
        }
        
    }
}
