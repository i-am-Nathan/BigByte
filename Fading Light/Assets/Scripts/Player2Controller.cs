using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class Player2Controller : MonoBehaviour
{

	// Handling
	public float RotationSpeed;
	public float WalkSpeed;
    public bool CanMove;
    public int PushPower = 20;
    
    // System
    private Quaternion targetRotation;

    //Animation
    private bool _isPlayingJump = false;
    private bool _isPlayingAttack = false;
    private Animation _animator;

    // Components
    private CharacterController controller;

	void Start()
	{
        _animator = GetComponentInChildren<Animation>();//need this...
        controller = GetComponent<CharacterController>();
       
    }

	void Update()
	{
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

        if (!CanMove)
        {
            input = Vector3.zero;
        }

        Vector3 motion = input;
		//motion *= (-Mathf.Abs(input.x) == 1 && -Mathf.Abs(input.z) == 1) ? .7f : 1;
		Vector3 pos = GameObject.FindGameObjectWithTag("Player").transform.position;
		Vector3 difference = pos - transform.position;

		if ((difference.x > 80f && difference.x+motion.x*WalkSpeed < difference.x) ||  (difference.x < -80f && difference.x+motion.x*WalkSpeed > difference.x)) {
			motion.x = 0;
		}else{
			motion.x = motion.x * WalkSpeed;
		}

		if ((difference.z > 80f && difference.z+motion.z*WalkSpeed < difference.z) ||  (difference.z < -80f && difference.z+motion.z*WalkSpeed > difference.z)) {
			print ("THE DIFFERENCE OF Z IS " +difference.z);
			motion.z = 0;
		}else{
			motion.z = motion.z * WalkSpeed;
		}
		//motion.z = motion.z * walkSpeed;
		//motion *=  walkSpeed;
		// motion += Vector3.up * -8;

		controller.Move(motion * Time.deltaTime);


        if (!CanMove)
        {
            _animator.Play("idle");
            return;
        }

        if (Input.GetKey("e"))
        {
            _animator.Play("Jump");
            _isPlayingJump = true;
            _isPlayingAttack = false;
        }
        else if (Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d"))
        {
            if (!_animator.isPlaying || !_isPlayingJump)
            {
                _animator.Play("Walk");
                _isPlayingJump = false;
                _isPlayingAttack = false;
            }
           
            //animator.SetTrigger("Walk");//tell mecanim to do the attack animation(trigger)
        }
        else if (Input.GetKey("q"))
        {
            _animator.Play("Attack");
            _isPlayingJump = false;
            _isPlayingAttack = true;
            //animator.SetTrigger("Attack");//tell mecanim to do the attack animation(trigger)
        }
        else
        {
            if ((!_animator.isPlaying || !_isPlayingJump) && !_isPlayingAttack){
                _animator.Play("idle");
                _isPlayingJump = false;
            }
           
            //animator.SetBool("idle", true);
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
}
