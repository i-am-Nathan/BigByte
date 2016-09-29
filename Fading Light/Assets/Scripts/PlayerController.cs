using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{

    // Handling
    public float rotationSpeed = 20000;
    public float walkSpeed = .0000002f;
    public TextMesh levelText;
    public bool dead = false;

    // System
    private Quaternion targetRotation;

    // Components
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        //ControlMouse();
       
        ControlWASD();
        //transform.Rotate(Vector3.up * Time.deltaTime * 1000);

    }


	// When collision occurs between two objects
	void OnTriggerStay(Collider other) {

		// Checking if players are next to each other
		if (other.gameObject.tag == "Player2") {

			// Checking if users wanted to swap the torch
			if (Input.GetButtonDown ("SwapTorch")) {

				// Checking with which player the torch is
				bool inPlayer1 = false;
				Transform[] ts = GetComponentsInChildren<Transform> ();
				foreach (Transform t in ts) {
					if (t.tag == "Torch") {
						inPlayer1 = true;
					}
				}

				GameObject player;
				GameObject torchPos;
				if (inPlayer1) {
					player = GameObject.FindWithTag ("Player2");
					torchPos = GameObject.FindWithTag ("Torch Position 2");
				} else {
					player = GameObject.FindWithTag ("Player");
					torchPos = GameObject.FindWithTag ("Torch Position 1");
				}

				GameObject torch = GameObject.FindWithTag ("Torch");

				// Assigning parent of torch to the corresponding player
				torch.transform.parent = player.transform;
				Debug.Log (torch.transform.parent.tag);
				torch.transform.localPosition = torchPos.transform.localPosition;
			}
		}
	}

    void ControlWASD()
    {
        Vector3 input = new Vector3(-Input.GetAxisRaw("Vertical"), 0, Input.GetAxisRaw("Horizontal"));

        if (input != Vector3.zero)
        {
            targetRotation = Quaternion.LookRotation(input);
            transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);
        }

        Vector3 motion = input;
        //motion *= (-Mathf.Abs(input.x) == 1 && -Mathf.Abs(input.z) == 1) ? .7f : 1;
		Vector3 pos = GameObject.FindGameObjectWithTag("Player2").transform.position;
		Vector3 difference = pos - transform.position;
		if ((difference.x > 80f && difference.x+motion.x*walkSpeed < difference.x) ||  (difference.x < -80f && difference.x+motion.x*walkSpeed > difference.x)) {
			motion.x = 0;
		}else{
			motion.x = motion.x * walkSpeed;
		}
		if ((difference.z > 80f && difference.z+motion.z*walkSpeed < difference.z) ||  (difference.z < -80f && difference.z+motion.z*walkSpeed > difference.z)) {
			motion.z = 0;
		}else{
			motion.z = motion.z * walkSpeed;
		}
        controller.Move(motion * Time.deltaTime);
    }


}
