using UnityEngine;
using System.Collections;
using System;

public class TorchFuelController : MonoBehaviour {

    public float TotalFuelPercentage = 100;
    public float FuelBurnRate = 5;

    public GameObject TorchP1;
    public GameObject TorchP2;

    public GameObject SwordP1;
    public GameObject SwordP2;

    public Light Player1TorchLight;
    public Light Player2TorchLight;
    public bool TorchInPlayer1 = true;


    private float _maxAngle = 134;
    private bool _flickerUp = false;
    private int _flickerCount = 0;
    private int _flckerAmount = 50;
    private float _flickerChange = 0.03f;
    private System.Random random = new System.Random();

    // Use this for initialization
    void Start ()
    {
        TorchP2.SetActive(false);
        Player2TorchLight.gameObject.SetActive(false);
        InvokeRepeating("RemoveFuelAmount", 0, 1);
    }

    internal void AddFuel(float fuelAmount)
    {
        TotalFuelPercentage += fuelAmount;
        TotalFuelPercentage = Math.Min(100, TotalFuelPercentage);
    }

    public GameObject GetCurrentTorch()
    {
        if (TorchInPlayer1)
        {
            return TorchP1;
        }

        return TorchP2;
    }

    public bool IsInTorchRange(float x, float z)
    {
        var currentTorch = TorchP2;
        var currentTorchLight = Player2TorchLight;
        if (TorchInPlayer1)
        {
            currentTorch = TorchP1;
            currentTorchLight = Player1TorchLight;
        }

        var torchPosition = currentTorch.gameObject.transform.position;

        var distanceToTorch = Math.Sqrt(Math.Abs((torchPosition.x - x) * (torchPosition.x - x)) + Math.Abs((torchPosition.z - z) * (torchPosition.z - z)));

        var torchRadius = torchPosition.y*2.5 * Math.Tan((currentTorchLight.spotAngle/2) * (Math.PI / 180));
        torchRadius = Math.Abs(torchRadius);

        if (distanceToTorch < torchRadius)
        {
            return true;
        }

        return false;

    }
    // Update is called once per frame
    void Update () {
        return;
        //Flicker the torch
        if(_flickerCount == _flckerAmount)
        {
            _flickerCount = 0;
            
            if (random.Next(100) < 50)
            {
                _flickerUp = true;
            }
            else
            {
                _flickerUp = false;
            }
        }

        if (_flickerUp)
        {
            TotalFuelPercentage += _flickerChange;
        }
        else
        {
            TotalFuelPercentage -= _flickerChange;
        }

        _flickerCount++;
        UpdateTorch();
	}

    public void RemoveFuelAmount()
    {
        if(TotalFuelPercentage > 0)
        {
            TotalFuelPercentage -= FuelBurnRate;
        }
        
        UpdateTorch();
    }

    private void UpdateTorch()
    {
        Player1TorchLight.spotAngle = findSpotAngle(TotalFuelPercentage);
        Player2TorchLight.spotAngle = findSpotAngle(TotalFuelPercentage);
    }

    private float findSpotAngle(float totalFuelPercentage)
    {
        return _maxAngle * (TotalFuelPercentage / 100);
    }


    
    public void SwapPlayers()
    {
        Debug.Log("HERE");
        // Disabling current player's torch and activating the other
        if (TorchInPlayer1)
        {
            TorchP1.SetActive(false);
            SwordP1.SetActive(true);
            SwordP2.SetActive(false);
            Player1TorchLight.gameObject.SetActive(false);
            TorchInPlayer1 = false;
            TorchP2.SetActive(true);
            Player2TorchLight.gameObject.SetActive(true);
        }
        else
        {
            TorchP1.SetActive(true);
            SwordP1.SetActive(false);
            SwordP2.SetActive(true);
            Player1TorchLight.gameObject.SetActive(true);
            TorchInPlayer1 = true;
            TorchP2.SetActive(false);
            Player2TorchLight.gameObject.SetActive(false);
        }
        
    }
}
