using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LifeManager : MonoBehaviour {

    private GameObject[] _lives;
    private int _numberOfLivesLeft;
    private LifeTrack _lifeTrackScript;
    public Canvas DeathScreen;

    void Awake()
    {
        GameObject go = GameObject.FindGameObjectWithTag("Life Track");
        _lifeTrackScript = (LifeTrack)go.GetComponent(typeof(LifeTrack));

        _lives = GameObject.FindGameObjectsWithTag("Lives");
        _numberOfLivesLeft = _lifeTrackScript.GetNumberOfLives();
        
        for (int i = 0; i < _numberOfLivesLeft; i++)
        {
            _lives[i].SetActive(true);
        }
    }

    void Start()
    {
        DeathScreen.enabled = false;
    }
	
	public void LoseLife()
    {
        _numberOfLivesLeft = _numberOfLivesLeft - 1;
        _lifeTrackScript.SetNumberOfLives(_numberOfLivesLeft);

        // Removing a heart from the UI
        for (int i = _numberOfLivesLeft; i < _lives.Length; i++)
        {
            _lives[i].SetActive(false);
        }

        if (_numberOfLivesLeft <= 0)
        {
            // Game over
            DeathScreen.enabled = true;
        }
        else
        {
            StartCoroutine("Wait");
        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(5f);
        // Restart level
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

}
