using UnityEngine;
using System.Collections;

public class LevelTwoMiddleTrap : MonoBehaviour {
    public GameObject[] sawTrap;
    public GameObject[] axeTrap;
    public GameObject door;
    private bool _pulled = false;

    // Use this for initialization
    void Start () {
        foreach (GameObject obj in sawTrap)
        {
            obj.GetComponent<Animation>().Stop();
        }
        foreach (GameObject obj in axeTrap)
        {
            obj.GetComponent<Animation>().Stop();
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// Called when the player is close enough to the lever, and presses T
    /// </summary>
    void OnTriggerStay(Collider other)
    {
        //if O is pressed to interact with the lever, the walls move
		if (Input.GetKeyDown(KeyCode.O) && !_pulled && other.gameObject.tag.Equals("Player"))
        {
            this.GetComponent<Animation>().Play("Armature|LeverDown");
            foreach (GameObject obj in sawTrap)
            {
                obj.GetComponent<Animation>().Play();
            }
            foreach (GameObject obj in axeTrap)
            {
                obj.GetComponent<Animation>().Play();
            }
            Destroy(door);
            _pulled = true;
        }
		//if Q is pressed to interact with the lever, the walls move
		if (Input.GetKeyDown(KeyCode.Q) && !_pulled && other.gameObject.tag.Equals("Player2"))
		{
			this.GetComponent<Animation>().Play("Armature|LeverDown");
			foreach (GameObject obj in sawTrap)
			{
				obj.GetComponent<Animation>().Play();
			}
			foreach (GameObject obj in axeTrap)
			{
				obj.GetComponent<Animation>().Play();
			}
			Destroy(door);
			_pulled = true;
		}
    }
}
