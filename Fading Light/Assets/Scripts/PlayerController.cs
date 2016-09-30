﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{

    Animator animator;

    // Handling
    public float rotationSpeed;
    public float walkSpeed;
    public int WeaponState;//unarmed, 1H, 2H, bow, dual, pistol, rifle, spear and ss(sword and shield)
    public TextMesh levelText;
    public bool dead = false;
    public bool wasAttacking;// we need this so we can take lock the direction we are facing during attacks, mecanim sometimes moves past the target which would flip the character around wildly

    // System
    private Quaternion targetRotation;

    // Components
    private CharacterController controller;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();//need this...
        controller = GetComponent<CharacterController>();
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


        animator.SetInteger("WeaponState", WeaponState);// probably would be better to check for change rather than bashing the value in like this

        if (Input.GetKey("up") || Input.GetKey("down") || Input.GetKey("left") || Input.GetKey("right"))
        {
            animator.SetBool("Idling", false);
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            
            animator.SetTrigger("Use");//tell mecanim to do the attack animation(trigger)
        }
        else
        {
            animator.SetBool("Idling", true);
        }
        

    }


}
