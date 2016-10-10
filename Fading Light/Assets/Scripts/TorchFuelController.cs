using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

/// <summary>
/// Used to control player torches and torch fuel burn rate
/// </summary>
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

	//The _maxAngle and _maxFOV fields are represented in radians as the only case where they are used is in the Math.Tan function.
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

    /// <summary>
    /// Use this for initialization
    /// </summary>
    void Start ()
    {
        TorchP2.SetActive(false);
        Player2TorchLight.gameObject.SetActive(false);
        InvokeRepeating("RemoveFuelAmount", 0, 0.1f);
        _torchFuelSlider = GameObject.FindWithTag("Torch Fuel Slider").GetComponent<Slider>();
        _player1InventoryImage = GameObject.FindWithTag("Player 1 Inventory").GetComponent<Image>();
        _player2InventoryImage = GameObject.FindWithTag("Player 2 Inventory").GetComponent<Image>();
    }


    /// <summary>
    /// Adds the fuel.
    /// </summary>
    /// <param name="fuelAmount">The fuel amount.</param>
    internal void AddFuel(float fuelAmount)
    {
        TotalFuelPercentage += fuelAmount;
        TotalFuelPercentage = Math.Min(100, TotalFuelPercentage);
        _torchFuelSlider.value = TotalFuelPercentage;
    }

    /// <summary>
    /// Gets the current torch.
    /// </summary>
    /// <returns></returns>
    public GameObject GetCurrentTorch()
    {
        if (TorchInPlayer1)
        {
            return TorchP1;
        }

        return TorchP2;
    }

    /// <summary>
    /// Determines whether [is in torch range] [the specified x].
    /// </summary>
    /// <param name="x">The x.</param>
    /// <param name="z">The z.</param>
    /// <returns>
    ///   <c>true</c> if [is in torch range] [the specified x]; otherwise, <c>false</c>.
    /// </returns>
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

        var torchRadius = GetTorchRadius();
        torchRadius = Math.Abs(torchRadius);

        if (distanceToTorch < torchRadius)
        {
            return true;
        }

        return false;

    }

    /// <summary>
    /// Gets the torch position.
    /// </summary>
    /// <returns></returns>
    public Vector3 GetTorchPosition()
    {
        var currentTorch = TorchP2;
        var currentTorchLight = Player2TorchLight;

        var torchPosition = currentTorch.gameObject.transform.position;

        return torchPosition;
    }

    /// <summary>
    /// Gets the torch radius.
    /// </summary>
    /// <returns></returns>
    public double GetTorchRadius()
    {
        var currentTorch = TorchP2;
        var currentTorchLight = Player2TorchLight;

        var torchPosition = currentTorch.gameObject.transform.position;
        
        var torchRadius = torchPosition.y * Math.Tan((currentTorchLight.spotAngle / 2) * (Math.PI / 180));
        torchRadius = Math.Abs(torchRadius);
        
        return torchRadius;
    }

    /// <summary>
    /// Updates this instance.
    /// </summary>
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

    /// <summary>
    /// Removes the fuel amount.
    /// </summary>
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

    /// <summary>
    /// Updates the torch.
	/// This happens by changing the angle of the spotlight and its child projector
	/// based on the percentage of fuel left in the torch.
    /// </summary>
    private void UpdateTorch()
    {
        Player1TorchLight.spotAngle = findSpotAngle(TotalFuelPercentage);
        Player2TorchLight.spotAngle = findSpotAngle(TotalFuelPercentage);
		Player1Projector.fieldOfView = findProjFOV(TotalFuelPercentage);
		Player2Projector.fieldOfView = findProjFOV(TotalFuelPercentage);
    }

    /// <summary>
    /// Finds the spotlight angle.
	/// This happens by setting the radius of the circle in proportion to the fuel percentage,
	/// then converting the radius to an angle using trigonometric functions.
    /// </summary>
    /// <param name="totalFuelPercentage">The total fuel percentage.</param>
	/// <returns>The angle (in degrees) of the spotlight</returns>
    private float findSpotAngle(float totalFuelPercentage)
    {
		double radius = Math.Tan (_maxAngle) * (totalFuelPercentage / 100);
		return Convert.ToSingle (Math.Atan (radius) * (360 / Math.PI));
    }

    /// <summary>
    /// Finds the Field of view of the projector.
	/// This happens by setting the radius of the circle in proportion to the fuel percentage,
	/// then converting the radius to an angle using trigonometric functions.
	/// </summary>
    /// <param name="totalFuelPercentage">The total fuel percentage.</param>
    /// <returns></returns>
    private float findProjFOV(float totalFuelPercentage)
	{
		
		double radius = Math.Tan (_maxFOV) * (totalFuelPercentage / 100);
		return Convert.ToSingle (Math.Atan (radius) * (360 / Math.PI));
	}

    /// <summary>
    /// Swaps the players.
    /// </summary>
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
