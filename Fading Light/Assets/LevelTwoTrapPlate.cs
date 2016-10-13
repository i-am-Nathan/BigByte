using UnityEngine;
using System.Collections;

public class LevelTwoTrapPlate : MonoBehaviour
{
    public GameObject[] axe;
    public GameObject[] saws;
    private bool _pressed = false;

    // Use this for initialization
    void Start()
    {
        foreach (GameObject obj in axe)
        {
            obj.GetComponent<Animation>().Stop();
        }
        foreach (GameObject obj2 in saws)
        {
            obj2.GetComponent<Animation>().Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        // the crate has a weight of 2
        if (other.name == "Player 1" || other.name == "Player2")
        {

            if (!_pressed)
            {
                foreach (GameObject obj in axe)
                {
                    obj.GetComponent<Animation>().Play();
                }
                foreach (GameObject obj2 in saws)
                {
                    obj2.GetComponent<Animation>().Play();
                }
                _pressed = true;
            }
        }
    }
}
