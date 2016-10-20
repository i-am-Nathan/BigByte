// file:	Assets\Scripts\Player\PlayerController.cs
//
// summary:	Implements the player controller class

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>   Used to control player 1. </summary>
///
/// <remarks>    . </remarks>

[RequireComponent(typeof(CharacterController))]
public class PlayerController : Player
{
    /// <summary>   Manager for achievement. </summary>
    public AchievementManager AchievementManager;
    // Health image on floor
    /// <summary>   Reference to the UI's health circle. </summary>
    public Image healthCircle;
    /// <summary>   True when the player gets damaged. </summary>
    private bool damaged;

    // Handling
    /// <summary>   The rotation speed. </summary>
    public float RotationSpeed;
    /// <summary>   The walk speed. </summary>
    public float WalkSpeed;
    /// <summary>   State of the weapon. </summary>
    public int WeaponState;

    /// <summary>   The push power. </summary>
    public int PushPower = 20;
    /// <summary>   True if this object is disabled. </summary>
    public bool IsDisabled;
    /// <summary>   True if last pressed. </summary>
    private bool _lastPressed = false;
    /// <summary>   The torch fuel controller script. </summary>
    private TorchFuelController TorchFuelControllerScript;

    /// <summary>   Target rotation. </summary>
    private Quaternion _targetRotation;
    /// <summary>   The animator. </summary>
    private Animator _animator;
    /// <summary>   The torch. </summary>
    private GameObject _torch;
    /// <summary>   The controller. </summary>
    private CharacterController _controller;

    // UI
    /// <summary>   The health slider. </summary>
    private Slider _healthSlider;
    /// <summary>   The life manager script. </summary>
    private LifeManager _lifeManagerScript;
    /// <summary>   The last jump time. </summary>
    private float _lastJumpTime;
    /// <summary>   The last attack. </summary>
	private float _lastAttack;
    //audio
    /// <summary>   The walking sounds. </summary>
    public AudioSource WalkingSounds;
    /// <summary>   The death sound. </summary>
    public AudioSource DeathSound;
    /// <summary>   The hurt sounds. </summary>
    public AudioSource HurtSounds;
    /// <summary>   The hit sounds. </summary>
    public AudioSource HitSounds;
    
    /// <summary>   True if this object is main menu. </summary>
    public bool IsMainMenu = false;

    /// <summary>   Starts this instance. </summary>
    ///
 

    protected override void Start()
    {
        base.Start();
		_lastAttack = Time.time;
        _lastJumpTime = Time.time;
        GameObject go = GameObject.FindGameObjectWithTag("TorchFuelController");
        TorchFuelControllerScript = (TorchFuelController)go.GetComponent(typeof(TorchFuelController));
        healthCircle.enabled = false;
        _animator = GetComponentInChildren<Animator>();//need this...
        _controller = GetComponent<CharacterController>();
        _torch = transform.Find("ROOT/Hips/Spine/Spine1/R Clavicle/R UpperArm/R Forearm/R Hand/R Weapon/Torch Light Holder").gameObject;

        if (!IsMainMenu)
        {
            _healthSlider = GameObject.FindWithTag("Player 1 Health Slider").GetComponent<Slider>();
            GameObject go1 = GameObject.FindGameObjectWithTag("Life Manager");
            _lifeManagerScript = (LifeManager)go1.GetComponent(typeof(LifeManager));

        }

    }

    /// <summary>   Mock up. </summary>
    ///
 

    public void MockUp()
    {
        base.Start();
    }

    /// <summary>   Updates this instance. </summary>
    ///
 

    void Update()
    {
        if (IsDisabled || isDead)
        {
            _animator.SetBool("Idling", true);
            if (isDead && _animator)
            {
                IsDisabled = true;
                _animator.Play("2HDeathB");//tell mecanim to do the attack animation(trigger)
            }

            return;
        }
        else
        {
            ControlWASD();
        }

		if (isHealthPotActive ()) {
			UpdateHealthUI ();
			SetHealthPotActive ();
		}

		UpdateEffects ();
    }

    /// <summary>   Controls this character with its keys. </summary>
    ///
 

    void ControlWASD()
    {
        if (IsDisabled)
        {
			_animator.SetBool("Idling", true);
            return;
        }

        Vector3 input = new Vector3(-Input.GetAxisRaw("Vertical"), 0, Input.GetAxisRaw("Horizontal"));

        if (input != Vector3.zero)
        {
            _targetRotation = Quaternion.LookRotation(input);
            transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, _targetRotation.eulerAngles.y, RotationSpeed * Time.deltaTime);
        }

        _animator.SetInteger("WeaponState", WeaponState);// probably would be better to check for change rather than bashing the value in like this

        if (Input.GetKeyDown(KeyCode.P) && !_torch.activeInHierarchy && Time.time - _lastAttack > 1.4f)
        {
            _lastAttack = Time.time;
            this.setAttacking(true);
            _animator.SetTrigger("Use");//tell mecanim to do the attack animation(trigger)
            AchievementManager.AchievementObtained("First Swing");
            HitSounds.Play();
        }
        else if (Input.GetKey("up") || Input.GetKey("down") || Input.GetKey("left") || Input.GetKey("right"))
        {
            _animator.SetBool("Idling", false);
        }

        else
        {
            _animator.SetBool("Idling", true);
        }
	
        if (Input.GetKeyDown("up") || Input.GetKeyDown("down") || Input.GetKeyDown("left") || Input.GetKeyDown("right"))
        {
            WalkingSounds.Play();
        }

        else if ((Input.GetKeyUp("up") || Input.GetKeyUp("down") || Input.GetKeyUp("left") || Input.GetKeyUp("right"))&&!(Input.GetKey("up") || Input.GetKey("down") || Input.GetKey("left") || Input.GetKey("right")))
        {
            WalkingSounds.Stop();
        }
        //transform.position = new Vector3(transform.position.x, 0, transform.position.z);

    }

    /// <summary>   When collision occurs between two objects. </summary>
    ///
 
    ///
    /// <param name="other">    The other. </param>

    void OnTriggerStay(Collider other)
    {

        // Checking if players are next to each other
        if (other.gameObject.tag.Equals("Player2") && !IsDisabled)
        {
            if (Input.GetButtonDown("SwapTorch") && !_lastPressed)
            {
                // Disabling current player's torch and activating the other
                _lastPressed = true;
                TorchFuelControllerScript.SwapPlayers();
            }
            else if (Input.GetButtonUp("SwapTorch"))
            {
                _lastPressed = false;
            } 
        }
    }

    /// <summary>   Used to push rigid body objects in the scene. </summary>
    ///
 
    ///
    /// <param name="hit">  The hit. </param>

    void OnControllerColliderHit(ControllerColliderHit hit)
    {

        if (hit.gameObject.tag.Equals("Clockwise Door")) {
            Debug.Log("pushed");
            Debug.Log(transform.forward * 10);
            hit.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * 10000000);

        }

    }

    /// <summary>   Damages the specified amount. </summary>
    ///
 
    ///
    /// <param name="amount">   The amount. </param>
    /// <param name="attacker"> The attacker. </param>

    public override void Damage(float amount, Transform attacker)
    {
        if (!CanTakeDamage)
        {
            return;
        }

        Debug.Log("Player damaged");

		if (isDefensePotActive ()) {
			amount = amount / 2;
			Debug.Log ("Damage taken p1 " + amount);
		}

        base.Damage(amount, attacker);
        // Set the damaged flag so the screen will flash.
        damaged = true;

        // Set the health bar's value to the current health.
        try
        {
            healthCircle.enabled = true;
            healthCircle.fillAmount -= amount / base.IntialHealth;
            _healthSlider.value -= amount;
            Debug.Log(healthCircle.fillAmount);
            Invoke("HideHealth", 3);

            // If the player has lost all it's health and the death flag hasn't been set yet...
            if (CurrentHealth <= 0 && !isDead)
            {
                // ... it should die.
                Killed();
            }
            else
            {
                if (!HurtSounds.isPlaying)
                {
                    HurtSounds.Play();
                }
            }
        }
        catch { }
    }

    /// <summary>   Killeds this instance. </summary>
    ///
 

    public override void Killed()
    {
        // Set the death flag so this function won't be called again.
        base.Killed();
        try
        {
            _lifeManagerScript.LoseLife();
            Debug.Log("Dead");
            DeathSound.Play();
        }
        catch { }
    }

    /// <summary>   Hides the health. </summary>
    ///
 

    public void HideHealth()
    {
        healthCircle.enabled = false;
    }

    /// <summary>   Executes the particle collision action. </summary>
    ///
 
    ///
    /// <param name="other">    The other. </param>

    void OnParticleCollision(GameObject other)
    {
		if(other.name.Equals("Afterburner")){
			Damage(0.6f, transform);
		}
			
		if (TorchFuelControllerScript.TorchInPlayer1 && other.name.Equals("Wind"))
        {
            TorchFuelControllerScript.RemoveFuelWithAmount(1);
        }
    }

    /// <summary>   Increases health sliders when health pot is activated. </summary>
    ///
 

	public void UpdateHealthUI () {
		healthCircle.fillAmount += 30f;
		_healthSlider.value += 30f;
	}

}
