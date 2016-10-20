// file:	Assets\Scripts\Mob Spawners\MobSpawnPoint.cs
//
// summary:	Implements the mob spawn point class

using UnityEngine;
using System.Collections;

/// <summary>   Used to turn an object into torch fuel collectable. </summary>
///
/// <remarks>    . </remarks>

public class MobSpawnPoint : MonoBehaviour
{
    /// <summary>   The fuel amount. </summary>
    public float FuelAmount = 10;
    
    /// <summary>   The torch fuel controller script. </summary>
    private TorchFuelController TorchFuelControllerScript;
    /// <summary>   Manager for achievement. </summary>
	private AchievementManager _achievementManager;

    /// <summary>   The objects. </summary>
    public GameObject[] objects;

    /// <summary>   Spawn random. </summary>
    ///
    /// <remarks>    . </remarks>

    public void SpawnRandom()
    {
        Instantiate(objects[UnityEngine.Random.Range(0, objects.Length - 1)]);
    }

    /// <summary>   Starts this instance. </summary>
    ///
    /// <remarks>    . </remarks>

    void Start()
    {
        GameObject go = GameObject.FindGameObjectWithTag("TorchFuelController");
        TorchFuelControllerScript = (TorchFuelController)go.GetComponent(typeof(TorchFuelController));

        var parent = this.transform.parent;
        var plane = parent.transform.Find("Plane").gameObject;
        Debug.Log(plane.GetType());
        var collider = (MeshCollider)plane.GetComponent<MeshCollider>();

        var xOffset = Random.Range(-collider.bounds.size.x / 2, collider.bounds.size.x / 2);
        var zOffset = Random.Range(-collider.bounds.size.z / 2, collider.bounds.size.z / 2);

        Vector3 newPos = new Vector3(parent.transform.position.x + xOffset, parent.transform.position.y, parent.transform.position.z + zOffset);
        transform.position = newPos;

		_achievementManager = (AchievementManager)GameObject.FindGameObjectWithTag ("AchievementManager").GetComponent(typeof(AchievementManager));

    }

    /// <summary>   Update is called once per frame. </summary>
    ///
    /// <remarks>    . </remarks>

    void Update()
    {

    }

    /// <summary>   When collision occurs between two objects. </summary>
    ///
    /// <remarks>    . </remarks>
    ///
    /// <param name="other">    The other. </param>

    void OnTriggerStay(Collider other)
    {
        // Checking if players are next to each other
        if (other.gameObject.tag.Equals("Player2") && !TorchFuelControllerScript.TorchInPlayer1)
        {
            TorchFuelControllerScript.AddFuel(FuelAmount);
            TorchFuelControllerScript.RemoveFuelAmount();
			_achievementManager.AchievementObtained ("Let there be light!");
            Destroy(this.gameObject);
        }else if(other.gameObject.tag.Equals("Player") && TorchFuelControllerScript.TorchInPlayer1)
        {
            TorchFuelControllerScript.AddFuel(FuelAmount);
            TorchFuelControllerScript.RemoveFuelAmount();
            _achievementManager.AchievementObtained("Let there be light!");
            Destroy(this.gameObject);
        }
    }

}
