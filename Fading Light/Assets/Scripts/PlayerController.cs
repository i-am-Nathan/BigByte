using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{

    // Health image on floor
    public Image healthCircle;                                 // Reference to the UI's health circle.

    // Handling
    public float RotationSpeed;
    public float WalkSpeed;
    public int WeaponState;//unarmed, 1H, 2H, bow, dual, pistol, rifle, spear and ss(sword and shield)
    public bool dead = false;

   
    public int PushPower = 20;
    public bool IsDisabled;
    private bool _lastPressed = false;
    private TorchFuelController TorchFuelControllerScript;
    // System
    private Quaternion _targetRotation;
    Animator _animator;
    // Components
    private CharacterController _controller;


    void Start()
    {
        GameObject go = GameObject.FindGameObjectWithTag("TorchFuelController");
        TorchFuelControllerScript = (TorchFuelController)go.GetComponent(typeof(TorchFuelController));

        _animator = GetComponentInChildren<Animator>();//need this...
        _controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        ControlWASD();	
    }



    void ControlWASD()
    {
        if (IsDisabled)
        {
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

}
