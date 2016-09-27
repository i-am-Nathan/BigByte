using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{

    // Handling
    public float rotationSpeed = 1000;
    public float walkSpeed = 20;
  
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



    void ControlWASD()
    {
        Vector3 input = new Vector3(-Input.GetAxisRaw("Vertical"), 0, Input.GetAxisRaw("Horizontal"));

        if (input != Vector3.zero)
        {
            targetRotation = Quaternion.LookRotation(input);
            transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);
        }

        Vector3 motion = input;
        motion.x = motion.x * walkSpeed;
        motion.z = motion.z * walkSpeed;

        controller.Move(motion * Time.deltaTime);
    }


}
