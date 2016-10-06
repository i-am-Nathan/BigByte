using UnityEngine;
using UnityEngine.UI;
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
	public Projector Player1Projector;
    public Light Player2TorchLight;
	public Projector Player2Projector;
    public bool TorchInPlayer1 = true;

    public Sprite torchSprite;
    public Sprite swordSprite;

	private double _maxAngle = 67*(Math.PI/180);
	private double _maxFOV = 64*(Math.PI/180);
    private bool _flickerUp = false;
    private int _flickerCount = 0;
    private int _flckerAmount = 50;
    private float _flickerChange = 0.03f;
    private System.Random random = new System.Random();
    private Slider _torchFuelSlider;
    private Image _player1InventoryImage;
    private Image _player2InventoryImage;
    public bool IsDisabled;

    // Use this for initialization
    void Start ()
    {
        TorchP2.SetActive(false);
        Player2TorchLight.gameObject.SetActive(false);
        InvokeRepeating("RemoveFuelAmount", 0, 0.1f);
        _torchFuelSlider = GameObject.FindWithTag("Torch Fuel Slider").GetComponent<Slider>();
        _player1InventoryImage = GameObject.FindWithTag("Player 1 Inventory").GetComponent<Image>();
        _player2InventoryImage = GameObject.FindWithTag("Player 2 Inventory").GetComponent<Image>();
    }

    internal void AddFuel(float fuelAmount)
    {
        TotalFuelPercentage += fuelAmount;
        TotalFuelPercentage = Math.Min(100, TotalFuelPercentage);
        _torchFuelSlider.value = TotalFuelPercentage;
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
        if (IsDisabled)
        {
            return;
        }

        if(TotalFuelPercentage > 0)
        {
            TotalFuelPercentage -= FuelBurnRate;
            _torchFuelSlider.value = TotalFuelPercentage;
        }
        
        UpdateTorch();
    }

    private void UpdateTorch()
    {
        Player1TorchLight.spotAngle = findSpotAngle(TotalFuelPercentage);
        Player2TorchLight.spotAngle = findSpotAngle(TotalFuelPercentage);
		Player1Projector.fieldOfView = findProjFOV(TotalFuelPercentage);
		Player2Projector.fieldOfView = findProjFOV(TotalFuelPercentage);
    }

    private float findSpotAngle(float totalFuelPercentage)
    {
		double radius = Math.Tan (_maxAngle) * (totalFuelPercentage / 100);
		return Convert.ToSingle (Math.Atan (radius) * (360 / Math.PI));
    }

	private float findProjFOV(float totalFuelPercentage)
	{
		
		double radius = Math.Tan (_maxFOV) * (totalFuelPercentage / 100);
		return Convert.ToSingle (Math.Atan (radius) * (360 / Math.PI));
	}
    
    public void SwapPlayers()
    {
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
            _player1InventoryImage.sprite = swordSprite;
            _player2InventoryImage.sprite = torchSprite;
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
            _player1InventoryImage.sprite = torchSprite;
            _player2InventoryImage.sprite = swordSprite;
        }
        
    }
}
