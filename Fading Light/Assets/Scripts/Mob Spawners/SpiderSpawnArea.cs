using UnityEngine;
using System.Collections;

/// <summary>
/// Used to turn an object into torch fuel collectable
/// </summary>
public class SpiderSpawnArea : MonoBehaviour
{
    public float FuelAmount = 10;
    public int LowerBound;
    public int UpperBound;

    private TorchFuelController TorchFuelControllerScript;
	private AchievementManager _achievementManager;

    private bool DEBUG = false;

    /// <summary>
    /// Starts this instance.
    /// </summary>
    void Start()
    {
        float randomNumber = Random.Range(LowerBound, UpperBound);
        if (DEBUG) Debug.Log("Random number of spiders: " + randomNumber);

        var plane = this.transform.Find("Plane").gameObject;
        var collider = (MeshCollider)plane.GetComponent<MeshCollider>();

        for (int i = 0; i< randomNumber; i++)
        {
            if (DEBUG) Debug.Log("Create spider number: " + i);
            GameObject newSpider = (GameObject) Instantiate(Resources.Load("Spider"));

            var xOffset = Random.Range(-collider.bounds.size.x / 2, collider.bounds.size.x / 2);
            var zOffset = Random.Range(-collider.bounds.size.z / 2, collider.bounds.size.z / 2);
            Vector3 newPos = new Vector3(this.transform.position.x + xOffset, -0.4f, this.transform.position.z + zOffset);
            newSpider.transform.position = newPos;
        }   
		_achievementManager = (AchievementManager)GameObject.FindGameObjectWithTag ("AchievementManager").GetComponent(typeof(AchievementManager));
    }


    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {

    }

   
}
