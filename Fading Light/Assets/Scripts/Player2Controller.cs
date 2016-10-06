﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Used to control player 2
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class Player2Controller : Player
{
    // Health image on floor
    public Image healthCircle;                                 // Reference to the UI's health circle.


    public AchievementManager AchievementManager;

    // Handling
    public float RotationSpeed;
	public float WalkSpeed;
    public int PushPower = 20;
    public int WeaponState;//unarmed, 1H, 2H, bow, dual, pistol, rifle, spear and ss(sword and shield)
    public bool IsDisabled;
   
    private Quaternion targetRotation;

    //Animation
    private bool _isPlayingJump = false;
    private bool _isPlayingAttack = false;
    private bool damaged;                                               // True when the player gets damaged.
    private Animator _animator;
    private GameObject _torch;
    private CharacterController controller;

    // UI
    private Slider _healthSlider;
    private Text _goldAmountText;
	private int _goldAmount = 0;
    private LifeManager _lifeManagerScript;
    private float _lastJumpTime;

    /// <summary>
    /// Starts this instance.
    /// </summary>
    void Start()
	{
        base.Start();
        healthCircle.enabled = false;
        _animator = GetComponentInChildren<Animator>();//need this...
        controller = GetComponent<CharacterController>();
        _lastJumpTime = Time.time;
        _healthSlider = GameObject.FindWithTag("Player 2 Health Slider").GetComponent<Slider>();
        _goldAmountText = GameObject.FindWithTag("Player 2 Gold").GetComponent<Text>();
        _torch = transform.Find("ROOT/Hips/Spine/Spine1/R Clavicle/R UpperArm/R Forearm/R Hand/R Weapon/Torch Light Holder").gameObject;
        var go = GameObject.FindGameObjectWithTag("Life Manager");
        _lifeManagerScript = (LifeManager)go.GetComponent(typeof(LifeManager));
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
	}

    /// <summary>
    /// Controls the character using the keys
    /// </summary>
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
        if (Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d"))
        {
            _animator.SetBool("Idling", false);
        }
        else if (Input.GetKeyDown(KeyCode.E) && !_torch.activeInHierarchy)
        {
            this.setAttacking(true);
            _animator.SetTrigger("Use");//tell mecanim to do the attack animation(trigger)
            AchievementManager.AddProgressToAchievement("First Hits", 1.0f);
        }
        else
        {
            _animator.SetBool("Idling", true);
        }

        //Character  jumps
        if (Input.GetKeyDown(KeyCode.Q) && (Time.time - _lastJumpTime) > .5)
        {
            transform.Translate(Vector3.up * 260 * Time.deltaTime, Space.World);
            _lastJumpTime = Time.time;
        }

    }


    /// <summary>
    // Used to push rigid body objects in the scene
    // Obtained from Unity Documentation
    /// </summary>
    /// <param name="hit">The hit.</param>
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

    /// <summary>
    /// Applied a specified amount of damage to the character
    /// </summary>
    /// <param name="amount">The amount.</param>
    /// <param name="attacker">The attacker.</param>
    public override void Damage(float amount, Transform attacker)
    {
        Debug.Log("Player damaged");
        healthCircle.enabled = true;
        base.Damage(amount, null);

        // Set the damaged flag so the screen will flash.
        damaged = true;

        // Set the health bar's value to the current health.
        healthCircle.fillAmount -= amount / 100.0f;
        _healthSlider.value -= amount;
        Invoke("HideHealth", 3);

        // If the player has lost all it's health and the death flag hasn't been set yet...
        if (CurrentHealth <= 0 && !isDead)
        {
            // ... it should die.
            Killed();
        }
    }

    /// <summary>
    /// Runs when the character dies
    /// </summary>
    public override void Killed()
    {
        // Set the death flag so this function won't be called again.
        base.Killed();
        IsDisabled = true;
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

    /// <summary>
    /// Updates the gold.
    /// </summary>
    /// <param name="amount">The amount.</param>
    public void UpdateGold(int amount)
    {
		_goldAmount += amount;
		_goldAmountText.text = "" + _goldAmount ;
    }
}
