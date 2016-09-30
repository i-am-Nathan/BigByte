using UnityEngine;
using System.Collections;

/// <summary>
/// Allows an NPC to wander arond the map with a speed, and randomly changing its heading ever x seconds
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class NpcWander : MonoBehaviour
{
    public float Speed = 5;
    public float DirectionChangeInterval = 1;
    public float MaxHeadingChange = 30;

    private CharacterController _characterController;
    private float _heading;
    private Vector3 _targetRotation;

    /// <summary>
    /// Awakes this instance.
    /// </summary>
    void Awake()
    {
        _characterController = GetComponent<CharacterController>();

        _heading = Random.Range(0, 360);
        transform.eulerAngles = new Vector3(0, _heading, 0);

        StartCoroutine(NewHeading());
    }

    /// <summary>
    /// Updates this instance.
    /// </summary>
    void Update()
    {
        transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, _targetRotation, Time.deltaTime * DirectionChangeInterval);
        var forward = transform.TransformDirection(Vector3.forward);
        _characterController.SimpleMove(forward * Speed);
    }

    /// <summary>
    /// This runs in the background and repeatedly calculates a new deading (after DirectionChangeInterval has elapsed)
    /// </summary>
    IEnumerator NewHeading()
    {
        while (true)
        {
            NewHeadingRoutine();
            yield return new WaitForSeconds(DirectionChangeInterval);
        }
    }

    /// <summary>
    /// Calculates a new direction to move towards.
    /// </summary>
    void NewHeadingRoutine()
    {
        var floor = Mathf.Clamp(_heading - MaxHeadingChange, 0, 360);
        var ceil = Mathf.Clamp(_heading + MaxHeadingChange, 0, 360);
        _heading = Random.Range(floor, ceil);
        _targetRotation = new Vector3(0, _heading, 0);
    }
}