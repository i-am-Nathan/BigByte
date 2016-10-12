using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Used to control player 1
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerController : Player
{
    public AchievementManager AchievementManager;
    // Health image on floor
    public Image healthCircle;                                 // Reference to the UI's health circle.
    private bool damaged;                                               // True when the player gets damaged.

    // Handling
    public float RotationSpeed;
    public float WalkSpeed;
    public int WeaponState;//unarmed, 1H, 2H, bow, dual, pistol, rifle, spear and ss(sword and shield)
    //public bool dead = false;
    public bool IsInCircle = false;
   
    public int PushPower = 20;
    public bool IsDisabled;
    private bool _lastPressed = false;
    private TorchFuelController TorchFuelControllerScript;

    private Quaternion _targetRotation;
    private Animator _animator;
    private GameObject _torch;
    private CharacterController _controller;

    //Potion effects 

    // UI
    private Slider _healthSlider;
    private LifeManager _lifeManagerScript;
    private float _lastJumpTime;

    public bool IsMainMenu = false;
    /// <summary>
    /// Starts this instance.
    /// </summary>
    protected override void Start()
    {
        base.Start();
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
    
    public void MockUp()
    {
        base.Start();
    }

    /// <summary>
    /// Updates this instance.
    /// </summary>
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
        UpdateEffects();
    }

    /// <summary>
    /// Controls this character with its keys
    /// </summary>
    void ControlWASD()
    {
        if(TorchFuelControllerScript.IsInTorchRange(gameObject.transform.position.x, gameObject.transform.position.z))
        {
            IsInCircle = true;
        }else
        {
            IsInCircle = false;
        }


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

        if (Input.GetKey("up") || Input.GetKey("down") || Input.GetKey("left") || Input.GetKey("right"))
        {
            _animator.SetBool("Idling", false);
        }
        else if (Input.GetKeyDown(KeyCode.Return) && !_torch.activeInHierarchy)
        {
            this.setAttacking(true);
            _animator.SetTrigger("Use");//tell mecanim to do the attack animation(trigger)
            AchievementManager.AddProgressToAchievement("First Hits",1.0f);
        }
        else
        {
            _animator.SetBool("Idling", true);
        }


        if (Input.GetKeyDown(KeyCode.RightControl) && (Time.time - _lastJumpTime) > .5)
        {
            transform.Translate(Vector3.up * 260 * Time.deltaTime, Space.World);
            _lastJumpTime = Time.time;
        }

    }

    /// <summary>
    /// When collision occurs between two objects
    /// </summary>
    /// <param name="other">The other.</param>
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


    /// <summary>
    /// Used to push rigid body objects in the scene
    /// </summary>
    /// <param name="hit">The hit.</param>
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag.Equals("Clockwise Door"))
        {

            Debug.Log(hit.gameObject.transform.parent);
            hit.gameObject.transform.parent.gameObject.GetComponent<RotatingDoor>().rotateClockwise();

        } else if (hit.gameObject.tag.Equals("Anticlockwise Door"))
        {
            
        }
    }

    /// <summary>
    /// Damages the specified amount.
    /// </summary>
    /// <param name="amount">The amount.</param>
    /// <param name="attacker">The attacker.</param>
    public override void Damage(float amount, Transform attacker)
    {
        Debug.Log("Player damaged");
        
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
        } catch {}      

        // If the player has lost all it's health and the death flag hasn't been set yet...
        if (CurrentHealth <= 0 && !isDead)
        {
            // ... it should die.
            Killed();
        }
    }

    /// <summary>
    /// Killeds this instance.
    /// </summary>
    public override void Killed()
    {
        // Set the death flag so this function won't be called again.
        base.Killed();
        _lifeManagerScript.LoseLife();
        Debug.Log("Dead");
    }

    /// <summary>
    /// Hides the health.
    /// </summary>
    public void HideHealth()
    {
        healthCircle.enabled = false;
    }    
}
