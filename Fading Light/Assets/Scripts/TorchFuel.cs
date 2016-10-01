using UnityEngine;
using System.Collections;

public class TorchFuel : MonoBehaviour
{
    public float FuelAmount = 10;

    private TorchFuelController TorchFuelControllerScript;
    // Use this for initialization
    void Start()
    {
        GameObject go = GameObject.FindGameObjectWithTag("TorchFuelController");
        TorchFuelControllerScript = (TorchFuelController)go.GetComponent(typeof(TorchFuelController));
    }

    // Update is called once per frame
    void Update()
    {

    }

    // When collision occurs between two objects
    void OnTriggerStay(Collider other)
    {
        // Checking if players are next to each other
        if (other.gameObject.tag.Equals("Player2") && !TorchFuelControllerScript.TorchInPlayer1)
        {
            TorchFuelControllerScript.AddFuel(FuelAmount);
            TorchFuelControllerScript.RemoveFuelAmount();
            Destroy(this.gameObject);
        }else if(other.gameObject.tag.Equals("Player") && TorchFuelControllerScript.TorchInPlayer1)
        {
            TorchFuelControllerScript.AddFuel(FuelAmount);
            TorchFuelControllerScript.RemoveFuelAmount();
            Destroy(this.gameObject);
        }
    }

}
