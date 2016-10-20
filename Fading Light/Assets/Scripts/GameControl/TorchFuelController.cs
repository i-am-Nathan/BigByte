// file:	Assets\Scripts\GameControl\TorchFuelController.cs
//
// summary:	Implements the torch fuel controller class

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

/// <summary>   Used to control player torches and torch fuel burn rate. </summary>
///
/// <remarks>    . </remarks>

public class TorchFuelController : MonoBehaviour {

    /// <summary>   The total fuel percentage. </summary>
    public float TotalFuelPercentage = 100;
    /// <summary>   The fuel burn rate. </summary>
    public float FuelBurnRate = 5;

    /// <summary>   The first torch p. </summary>
    public GameObject TorchP1;
    /// <summary>   The second torch p. </summary>
    public GameObject TorchP2;

    /// <summary>   The first sword p. </summary>
    public GameObject SwordP1;
    /// <summary>   The second sword p. </summary>
    public GameObject SwordP2;

    /// <summary>   The player 1 torch light. </summary>
    public Light Player1TorchLight;
    /// <summary>   The player 1 projector. </summary>
	public Projector Player1Projector;
    /// <summary>   The player 2 torch light. </summary>
    public Light Player2TorchLight;
    /// <summary>   The player 2 projector. </summary>
	public Projector Player2Projector;
    /// <summary>   True to torch in player 1. </summary>
    public bool TorchInPlayer1 = true;

    /// <summary>   The torch sprite. </summary>
    public Sprite torchSprite;
    /// <summary>   The sword sprite. </summary>
    public Sprite swordSprite;

	//The _maxAngle and _maxFOV fields are represented in radians as the only case where they are used is in the Math.Tan function.
    /// <summary>   The maximum angle. </summary>
	private double _maxAngle = 67*(Math.PI/180);
    /// <summary>   The maximum fov. </summary>
	private double _maxFOV = 64*(Math.PI/180);
    /// <summary>   True to flicker up. </summary>
    private bool _flickerUp = false;
    /// <summary>   Number of flickers. </summary>
    private int _flickerCount = 0;
    /// <summary>   The flcker amount. </summary>
    private int _flckerAmount = 50;
    /// <summary>   The flicker change. </summary>
    private float _flickerChange = 0.03f;
    /// <summary>   The random. </summary>
    private System.Random random = new System.Random();
    /// <summary>   The torch fuel slider. </summary>
    private Slider _torchFuelSlider;
    /// <summary>   The player 1 inventory image. </summary>
    private Image _player1InventoryImage;
    /// <summary>   The player 2 inventory image. </summary>
    private Image _player2InventoryImage;
    /// <summary>   True if this object is disabled. </summary>
    public bool IsDisabled;
	
    //Audio
    /// <summary>   The add fuel sound. </summary>
    public AudioSource AddFuelSound;
    /// <summary>   The flame sound. </summary>
    public AudioSource FlameSound;

    /// <summary>   True if this object is main menu. </summary>
    public bool IsMainMenu = false;

    /// <summary>   Use this for initialization. </summary>
    ///
    /// <remarks>    . </remarks>

    void Start ()
    {
	    
	    FlameSound.loop = true;
	    FlameSound.Play();
        TorchP2.SetActive(false);
        Player2TorchLight.gameObject.SetActive(false);
        InvokeRepeating("RemoveFuelAmount", 0, 0.1f);

        if (!IsMainMenu)
        {
            _torchFuelSlider = GameObject.FindWithTag("Torch Fuel Slider").GetComponent<Slider>();
            _player1InventoryImage = GameObject.FindWithTag("Player 1 Inventory").GetComponent<Image>();
            _player2InventoryImage = GameObject.FindWithTag("Player 2 Inventory").GetComponent<Image>();
        }

    }

    /// <summary>   Adds the fuel. </summary>
    ///
    /// <remarks>    . </remarks>
    ///
    /// <param name="fuelAmount">   The fuel amount. </param>

    internal void AddFuel(float fuelAmount)
    {
	    AddFuelSound.Play();
        TotalFuelPercentage += fuelAmount;
        TotalFuelPercentage = Math.Min(100, TotalFuelPercentage);
        _torchFuelSlider.value = TotalFuelPercentage;
    }

    /// <summary>   Gets the current torch. </summary>
    ///
    /// <remarks>    . </remarks>
    ///
    /// <returns>   The current torch. </returns>

    public GameObject GetCurrentTorch()
    {
        if (TorchInPlayer1)
        {
            return TorchP1;
        }

        return TorchP2;
    }

    /// <summary>   Determines whether [is in torch range] [the specified x]. </summary>
    ///
    /// <remarks>    . </remarks>
    ///
    /// <param name="x">    The x. </param>
    /// <param name="z">    The z. </param>
    ///
    /// <returns>
    /// <c>true</c> if [is in torch range] [the specified x]; otherwise, <c>false</c>.
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

        var torchPosition = currentTorch.gameObject.transform.parent.position;

        var distanceToTorch = Math.Sqrt(Math.Abs((torchPosition.x - x) * (torchPosition.x - x)) + Math.Abs((torchPosition.z - z) * (torchPosition.z - z)));

        var torchRadius = GetTorchRadius();
        torchRadius = Math.Abs(torchRadius);


        if (distanceToTorch < torchRadius)
        {
            return true;
        }

        return false;

    }

    /// <summary>   Gets the torch position. </summary>
    ///
    /// <remarks>    . </remarks>
    ///
    /// <returns>   The torch position. </returns>

    public Vector3 GetTorchPosition()
    {
        var currentTorch = TorchP2;
        var currentTorchLight = Player2TorchLight;

        var torchPosition = currentTorch.gameObject.transform.position;

        return torchPosition;
    }

    /// <summary>   Gets the torch radius. </summary>
    ///
    /// <remarks>    . </remarks>
    ///
    /// <returns>   The torch radius. </returns>

    public double GetTorchRadius()
    {
        var currentTorch = TorchP2;
        var currentTorchLight = Player2TorchLight;

        var torchPosition = currentTorch.gameObject.transform.position;
        
        var torchRadius = torchPosition.y * 1.2 * Math.Tan((currentTorchLight.spotAngle / 2) * (Math.PI / 180));
        torchRadius = Math.Abs(torchRadius);
        return torchRadius;
    }

    /// <summary>   Updates this instance. </summary>
    ///
    /// <remarks>    . </remarks>

    void Update () {
	        
	    if(TotalFuelPercentage == 0)
        {
            FlameSound.Stop();
        }
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

    /// <summary>   Removes the fuel with amount described by amount. </summary>
    ///
    /// <remarks>    . </remarks>
    ///
    /// <param name="amount">   The amount. </param>

    public void RemoveFuelWithAmount(float amount)
    {

        if (IsDisabled)
        {
            return;
        }
        Debug.Log("trug");
        if (TotalFuelPercentage > 0 && !IsMainMenu)
        {
            TotalFuelPercentage -= amount;
            _torchFuelSlider.value = TotalFuelPercentage;
        }

        UpdateTorch();
    }

    /// <summary>   Removes the fuel amount. </summary>
    ///
    /// <remarks>    . </remarks>

    public void RemoveFuelAmount()
    {
        if (IsDisabled)
        {
            return;
        }

        if(TotalFuelPercentage > 0 && !IsMainMenu)
        {
            TotalFuelPercentage -= FuelBurnRate;
            _torchFuelSlider.value = TotalFuelPercentage;
        }
        
        UpdateTorch();
    }

    /// <summary>
    /// Updates the torch. This happens by changing the angle of the spotlight and its child
    /// projector based on the percentage of fuel left in the torch.
    /// </summary>
    ///
    /// <remarks>    . </remarks>

    private void UpdateTorch()
    {
        Player1TorchLight.spotAngle = findSpotAngle(TotalFuelPercentage);
        Player2TorchLight.spotAngle = findSpotAngle(TotalFuelPercentage);
		Player1Projector.fieldOfView = findProjFOV(TotalFuelPercentage);
		Player2Projector.fieldOfView = findProjFOV(TotalFuelPercentage);
    }

    /// <summary>
    /// Finds the spotlight angle. This happens by setting the radius of the circle in proportion to
    /// the fuel percentage, then converting the radius to an angle using trigonometric functions.
    /// </summary>
    ///
    /// <remarks>    . </remarks>
    ///
    /// <param name="totalFuelPercentage">  The total fuel percentage. </param>
    ///
    /// <returns>   The angle (in degrees) of the spotlight. </returns>

    private float findSpotAngle(float totalFuelPercentage)
    {
		double radius = Math.Tan (_maxAngle) * (totalFuelPercentage / 100);
		return Convert.ToSingle (Math.Atan (radius) * (360 / Math.PI));
    }

    /// <summary>
    /// Finds the Field of view of the projector. This happens by setting the radius of the circle in
    /// proportion to the fuel percentage, then converting the radius to an angle using trigonometric
    /// functions.
    /// </summary>
    ///
    /// <remarks>    . </remarks>
    ///
    /// <param name="totalFuelPercentage">  The total fuel percentage. </param>
    ///
    /// <returns>   The found project fov. </returns>

    private float findProjFOV(float totalFuelPercentage)
	{
		
		double radius = Math.Tan (_maxFOV) * (totalFuelPercentage / 100);
		return Convert.ToSingle (Math.Atan (radius) * (360 / Math.PI));
	}

    /// <summary>   Swaps the players. </summary>
    ///
    /// <remarks>    . </remarks>

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

    /// <summary>   Determines if we can torch with player 1. </summary>
    ///
    /// <remarks>    . </remarks>
    ///
    /// <returns>   True if it succeeds, false if it fails. </returns>

    public bool TorchWithPlayer1()
    {
        if (TorchInPlayer1)
        {
            return true;
        } else
        {
            return false;
        }
    }
}
