// file:	assets\scripts\player\player2controller.cs
//
// summary:	Implements the player 2controller class

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>   Used to control player 2. </summary>
///
/// <remarks>    . </remarks>

[RequireComponent(typeof(CharacterController))]
public class Player2Controller : Player
{
    // Health image on floor
    /// <summary>   Reference to the UI's health circle. </summary>
    public Image healthCircle;


    /// <summary>   Manager for achievement. </summary>
    public AchievementManager AchievementManager;

    // Handling
    /// <summary>   The rotation speed. </summary>
    public float RotationSpeed;
    /// <summary>   The walk speed. </summary>
	public float WalkSpeed;
    /// <summary>   The push power. </summary>
    public int PushPower = 20;
    /// <summary>   unarmed, 1H, 2H, bow, dual, pistol, rifle, spear and ss(sword and shield) </summary>
    public int WeaponState;
    /// <summary>   True if this object is disabled. </summary>
    public bool IsDisabled;
   
    /// <summary>   Target rotation. </summary>
    private Quaternion targetRotation;

    //Animation
    /// <summary>   True if this object is playing jump. </summary>
    private bool _isPlayingJump = false;
    /// <summary>   True if this object is playing attack. </summary>
    private bool _isPlayingAttack = false;
    /// <summary>   True when the player gets damaged. </summary>
    private bool damaged;
    /// <summary>   The animator. </summary>
    private Animator _animator;
    /// <summary>   The torch. </summary>
    private GameObject _torch;
    /// <summary>   The controller. </summary>
    private CharacterController controller;

    // UI
    /// <summary>   The health slider. </summary>
    private Slider _healthSlider;
    /// <summary>   The life manager script. </summary>
    private LifeManager _lifeManagerScript;
    /// <summary>   The last jump time. </summary>
    private float _lastJumpTime;
    /// <summary>   The torch fuel script. </summary>
    private TorchFuelController _torchFuelScript;

    /// <summary>   The last attack. </summary>
	private float _lastAttack;
    //audio
    /// <summary>   The walk sounds. </summary>
    public AudioSource WalkSounds;
    /// <summary>   The hit sounds. </summary>
    public AudioSource HitSounds;
    /// <summary>   The hurt sounds. </summary>
    public AudioSource HurtSounds;
    /// <summary>   The death sound. </summary>
    public AudioSource DeathSound;

    /// <summary>   True if this object is main menu. </summary>
    public bool IsMainMenu = false;

    /// <summary>   Starts this instance. </summary>
    ///
 

    void Start()
	{
        base.Start();
		_lastAttack = Time.time;
        healthCircle.enabled = false;
        _animator = GetComponentInChildren<Animator>();//need this...
        controller = GetComponent<CharacterController>();
        _lastJumpTime = Time.time;
        if (!IsMainMenu)
        {
            _healthSlider = GameObject.FindWithTag("Player 2 Health Slider").GetComponent<Slider>();
            _torch = transform.Find("ROOT/Hips/Spine/Spine1/R Clavicle/R UpperArm/R Forearm/R Hand/R Weapon/Torch Light Holder").gameObject;
            var go = GameObject.FindGameObjectWithTag("Life Manager");
            _lifeManagerScript = (LifeManager)go.GetComponent(typeof(LifeManager));
            var go1 = GameObject.FindGameObjectWithTag("TorchFuelController");
            _torchFuelScript = (TorchFuelController)go1.GetComponent(typeof(TorchFuelController));

        }

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

		UpdateEffects ();
    }

    /// <summary>   Controls the character using the keys. </summary>
    ///
 

    void ControlWASD()
	{
        //Find the direction the character should be moving toward
		var input = new Vector3(-Input.GetAxisRaw("Vertical1"), 0, Input.GetAxisRaw("Horizontal1"));

		if (input != Vector3.zero)
		{
			targetRotation = Quaternion.LookRotation(input);
			transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, RotationSpeed * Time.deltaTime);
		}

        //Set the weapon to sword
        _animator.SetInteger("WeaponState", WeaponState);// probably would be better to check for change rather than bashing the value in like this

        //Disable idling animation if we are walking
        if (Input.GetKeyDown(KeyCode.E) && !_torch.activeInHierarchy && Time.time - _lastAttack > 1.4f)
        {
            _lastAttack = Time.time;
            this.setAttacking(true);
            _animator.SetTrigger("Use");//tell mecanim to do the attack animation(trigger)
            AchievementManager.AchievementObtained("First Swing");
            HitSounds.Play();
        }
        else if (Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d"))
        {
            _animator.SetBool("Idling", false);
        }
        else
        {
            _animator.SetBool("Idling", true);
        }
	    if (Input.GetKeyDown("w") || Input.GetKeyDown("s") || Input.GetKeyDown("a") || Input.GetKeyDown("d"))
        {
            WalkSounds.Play();
        }

        else if ((Input.GetKeyUp("w") || Input.GetKeyUp("s") || Input.GetKeyUp("a") || Input.GetKeyUp("d")) && !(Input.GetKey("w") || Input.GetKey("s") || Input.GetKey("a") || Input.GetKey("d")))
        {
            WalkSounds.Stop();
        }
    }

    /// <summary>
    // Used to push rigid body objects in the scene
    // Obtained from Unity Documentation
    /// </summary>
    /// <param name="hit">The hit.</param>

    /// <summary>   Executes the controller collider hit action. </summary>
    ///
 
    ///
    /// <param name="hit">  The hit. </param>

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        var body = hit.collider.attachedRigidbody;

        if (body == null || body.isKinematic)
        {
            return;
        }

        if (hit.moveDirection.y < -0.3F)
        {
            return;
        }
            
        var pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        body.velocity = pushDir * PushPower;
    }

    /// <summary>   Applied a specified amount of damage to the character. </summary>
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
        healthCircle.enabled = true;
        base.Damage(amount, null);

        // Set the damaged flag so the screen will flash.
        damaged = true;

        // Set the health bar's value to the current health.
        healthCircle.fillAmount -= amount / base.IntialHealth;
        _healthSlider.value -= amount;
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

    /// <summary>   Runs when the character dies. </summary>
    ///
 

    public override void Killed()
    {
        // Set the death flag so this function won't be called again.
        base.Killed();
        IsDisabled = true;
        _lifeManagerScript.LoseLife();

        DeathSound.Play();
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
		if(other.name.Equals("Afterburner")) {
			Damage(0.6f, transform);
		}
			
		else if (_torchFuelScript.TorchInPlayer1 == false && other.name.Equals("Wind"))
        {
            _torchFuelScript.RemoveFuelWithAmount(1f);
            Debug.Log("P2Wind");
        }
    }

}
