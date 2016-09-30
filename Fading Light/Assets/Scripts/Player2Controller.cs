using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class Player2Controller : MonoBehaviour
{

	// Handling
	public float rotationSpeed = 20000;
	public float walkSpeed = .0000002f;
	public TextMesh levelText;
	public bool dead = false;
    Animation animator;
    // System
    private Quaternion targetRotation;

	// Components
	private CharacterController controller;

	void Start()
	{
        animator = GetComponentInChildren<Animation>();//need this...
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

		if (input != Vector3.zero)
		{
			targetRotation = Quaternion.LookRotation(input);
			transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);
		}

		Vector3 motion = input;
		//motion *= (-Mathf.Abs(input.x) == 1 && -Mathf.Abs(input.z) == 1) ? .7f : 1;
		Vector3 pos = GameObject.FindGameObjectWithTag("Player").transform.position;
		Vector3 difference = pos - transform.position;

		if ((difference.x > 80f && difference.x+motion.x*walkSpeed < difference.x) ||  (difference.x < -80f && difference.x+motion.x*walkSpeed > difference.x)) {
			motion.x = 0;
		}else{
			motion.x = motion.x * walkSpeed;
		}

		if ((difference.z > 80f && difference.z+motion.z*walkSpeed < difference.z) ||  (difference.z < -80f && difference.z+motion.z*walkSpeed > difference.z)) {
			print ("THE DIFFERENCE OF Z IS " +difference.z);
			motion.z = 0;
		}else{
			motion.z = motion.z * walkSpeed;
		}
		//motion.z = motion.z * walkSpeed;
		//motion *=  walkSpeed;
		// motion += Vector3.up * -8;

		controller.Move(motion * Time.deltaTime);

        if (Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d"))
        {
            animator.Play("Walk");
            //animator.SetTrigger("Walk");//tell mecanim to do the attack animation(trigger)
        }
        else if (Input.GetKey("q"))
        {
           
            animator.Play("Attack");
            //animator.SetTrigger("Attack");//tell mecanim to do the attack animation(trigger)
        }
        else
        {
            if (!animator.isPlaying){
                animator.Play("idle");
            }
           
            //animator.SetBool("idle", true);
        }
    }


}
