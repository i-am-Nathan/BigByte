using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class Player2Controller : BaseEntity
{
    // Health image on floor
    public Image healthCircle;                                 // Reference to the UI's health circle.
    bool damaged;                                               // True when the player gets damaged.


    // Handling
    public float RotationSpeed;
	public float WalkSpeed;
    public bool CanMove;
    public int PushPower = 20;
    public int WeaponState;//unarmed, 1H, 2H, bow, dual, pistol, rifle, spear and ss(sword and shield)
    public bool IsDisabled;
    // System
    private Quaternion targetRotation;

    //Animation
    private bool _isPlayingJump = false;
    private bool _isPlayingAttack = false;
    Animator _animator;

    // Components
    private CharacterController controller;

	void Start()
	{
        base.Start();
        _animator = GetComponentInChildren<Animator>();//need this...
        controller = GetComponent<CharacterController>();
       
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

        //ControlMouse();

        ControlWASD();

		//transform.Rotate(Vector3.up * Time.deltaTime * 1000);
		/*Vector3 pos = Camera.main.WorldToViewportPoint (transform.position);
		pos.x = Mathf.Clamp01 (pos.x);
		pos.z = Mathf.Clamp01 (pos.z);
		transform.position = Camera.main.ViewportToWorldPoint (pos); */

	}




	void ControlWASD()
	{
       
       

		Vector3 input = new Vector3(-Input.GetAxisRaw("Vertical1"), 0, Input.GetAxisRaw("Horizontal1"));

		if (input != Vector3.zero && CanMove)
		{
			targetRotation = Quaternion.LookRotation(input);
			transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, RotationSpeed * Time.deltaTime);
		}

        _animator.SetInteger("WeaponState", WeaponState);// probably would be better to check for change rather than bashing the value in like this

        if (Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d"))
        {
            _animator.SetBool("Idling", false);
        }
        else if (Input.GetKey("e"))
        {
            _animator.SetTrigger("Use");//tell mecanim to do the attack animation(trigger)

        }
        else
        {
            _animator.SetBool("Idling", true);
        }

    }

    // Used to push rigid body objects in the scene
    // Obtained from Unity Documentation
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
        Debug.Log("Ow");
        base.Damage(amount, null);
        // Set the damaged flag so the screen will flash.
        damaged = true;

        // Set the health bar's value to the current health.
        healthCircle.fillAmount -= amount / 100.0f;
        Debug.Log(healthCircle.fillAmount);

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
        IsDisabled = true;


        Debug.Log("Dead");
    }
}
