using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{



    // Handling
    public float RotationSpeed;
    public float WalkSpeed;
    public int WeaponState;//unarmed, 1H, 2H, bow, dual, pistol, rifle, spear and ss(sword and shield)
    public bool dead = false;
    bool inPlayer1 = true;
    public GameObject TorchP1;
    public GameObject TorchP2;
    public GameObject SpotlightP1;
    public GameObject SpotlightP2;
    public int PushPower = 20;


    private bool _lastPressed = false;

    // System
    private Quaternion _targetRotation;
    Animator _animator;
    // Components
    private CharacterController _controller;


    void Start()
    {
        TorchP2.SetActive(false);
        SpotlightP2.SetActive(false);

        _animator = GetComponentInChildren<Animator>();//need this...
        _controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        //ControlMouse();
       
        ControlWASD();
        //transform.Rotate(Vector3.up * Time.deltaTime * 1000);
			
       
    }



    void ControlWASD()
    {
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
        if (other.gameObject.tag.Equals("Player2"))
        {
            Debug.Log("HERE");
            // Checking if users wanted to swap the torch
            if (Input.GetButtonDown("SwapTorch") && !_lastPressed)
            {
                // Disabling current player's torch and activating the other
                _lastPressed = true;

                if (TorchP1.gameObject.activeSelf)
                {
                    TorchP1.SetActive(false);
                    SpotlightP1.SetActive(false);

                    TorchP2.SetActive(true);
                    SpotlightP2.SetActive(true);
                } else
                {
                    TorchP1.SetActive(true);
                    SpotlightP1.SetActive(true);

                    TorchP2.SetActive(false);
                    SpotlightP2.SetActive(false);
                }

            } else if (Input.GetButtonUp("SwapTorch"))
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
