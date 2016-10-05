﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : BaseEntity
{
    public AchievementManager AchievementManager;
    // Health image on floor
    public Image healthCircle;                                 // Reference to the UI's health circle.
    bool damaged;                                               // True when the player gets damaged.

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
    // System
    private Quaternion _targetRotation;
    Animator _animator;
    // Components
    private CharacterController _controller;

    // UI
    private Slider _healthSlider;
    private Text _goldAmount;
    private LifeManager _lifeManagerScript;

    protected override void Start()
    {
        
        base.Start();
        
        GameObject go = GameObject.FindGameObjectWithTag("TorchFuelController");
        TorchFuelControllerScript = (TorchFuelController)go.GetComponent(typeof(TorchFuelController));
        healthCircle.enabled = false;
        _animator = GetComponentInChildren<Animator>();//need this...
        _controller = GetComponent<CharacterController>();

        _healthSlider = GameObject.FindWithTag("Player 1 Health Slider").GetComponent<Slider>();
        _goldAmount = GameObject.FindWithTag("Player 1 Gold").GetComponent<Text>();

        GameObject go1 = GameObject.FindGameObjectWithTag("Life Manager");
        _lifeManagerScript = (LifeManager)go1.GetComponent(typeof(LifeManager));
    }

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

        ControlWASD();
        //Damage(1f, null);
        // If the player has just been damaged...
        if (damaged)
        {
            // ... set the colour of the damageImage to the flash colour.
        }

        // Reset the damaged flag.
        damaged = false;
    }

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
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            
            _animator.SetTrigger("Use");//tell mecanim to do the attack animation(trigger)
            AchievementManager.AddProgressToAchievement("First Hits",1.0f);
        }
        else
        {
            _animator.SetBool("Idling", true);
        }


    }


    // When collision occurs between two objects
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


    // Used to push rigid body objects in the scene
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic)
            return;

        if (hit.moveDirection.y < -0.3F)
            return;

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        body.velocity = pushDir * PushPower;
    }

    public override void Damage(float amount, Transform attacker)
    {
        healthCircle.enabled = true;
        Debug.Log("Ow");
        base.Damage(amount, attacker);
        // Set the damaged flag so the screen will flash.
        damaged = true;

        // Set the health bar's value to the current health.
        healthCircle.fillAmount -= amount / 100.0f;
        _healthSlider.value -= amount;
        Debug.Log(healthCircle.fillAmount);
        Invoke("HideHealth", 3);
        // If the player has lost all it's health and the death flag hasn't been set yet...
        if (CurrentHealth <= 0 && !isDead)
        {
            // ... it should die.
            Killed();
        }
    }

    public override void Killed()
    {
        // Set the death flag so this function won't be called again.
        base.Killed();
        _lifeManagerScript.LoseLife();
        Debug.Log("Dead");
    }

    public void HideHealth()
    {
        healthCircle.enabled = false;
    }

    public void UpdateGold(int amount)
    {
        _goldAmount.text = "" + amount;
    }
    
}
