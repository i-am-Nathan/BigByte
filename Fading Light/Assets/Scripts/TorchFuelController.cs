using UnityEngine;
using System.Collections;
using System;

public class TorchFuelController : MonoBehaviour {

    public float TotalFuelPercentage = 100;
    public float FuelBurnRate = 5;

    public GameObject TorchP1;
    public GameObject TorchP2;
    public GameObject SpotlightP1;
    public GameObject SpotlightP2;
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
        SpotlightP2.SetActive(false);
        InvokeRepeating("RemoveFuelAmount", 0, 1);
    }

    internal void AddFuel(float fuelAmount)
    {
        TotalFuelPercentage += fuelAmount;
        TotalFuelPercentage = Math.Min(100, TotalFuelPercentage);
    }


    // Update is called once per frame
    void Update () {

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
        TotalFuelPercentage -= FuelBurnRate;
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

        // Disabling current player's torch and activating the other
        if (TorchP1.gameObject.activeSelf)
        {
            TorchP1.SetActive(false);
            SpotlightP1.SetActive(false);
            TorchInPlayer1 = false;
            TorchP2.SetActive(true);
            SpotlightP2.SetActive(true);
        }
        else
        {
            TorchP1.SetActive(true);
            SpotlightP1.SetActive(true);
            TorchInPlayer1 = true;
            TorchP2.SetActive(false);
            SpotlightP2.SetActive(false);
        }
        
    }
}
