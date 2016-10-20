// file:	Assets\Scripts\Mob Spawners\SpiderSpawnArea.cs
//
// summary:	Implements the spider spawn area class

using UnityEngine;
using System.Collections;

/// <summary>   Used to turn an object into torch fuel collectable. </summary>
///
/// <remarks>    . </remarks>

public class SpiderSpawnArea : MonoBehaviour
{
    /// <summary>   The fuel amount. </summary>
    public float FuelAmount = 10;
    /// <summary>   The lower bound. </summary>
    public int LowerBound;
    /// <summary>   The upper bound. </summary>
    public int UpperBound;

    /// <summary>   The torch fuel controller script. </summary>
    private TorchFuelController TorchFuelControllerScript;
    /// <summary>   Manager for achievement. </summary>
	private AchievementManager _achievementManager;

    /// <summary>   True to debug. </summary>
    private bool DEBUG = false;

    /// <summary>   Starts this instance. </summary>
    ///
 

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

    /// <summary>   Update is called once per frame. </summary>
    ///
 

    void Update()
    {

    }

   
}
