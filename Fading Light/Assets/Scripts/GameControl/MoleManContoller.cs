using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Used to control a MoleMan, walks and idles to a position
/// </summary>
public class MoleManContoller : MonoBehaviour {

    Animator _animator;

    public List<GameObject> targets;
    public float Speed = 8;
    public bool IsDisabled = true;
    public Storyline ThisStoryline;

    private bool _storylineNotified = false;

    /// <summary>
    /// Starts this instance.
    /// </summary>
    void Start () {
        _animator = GetComponentInChildren<Animator>();//need this...
        
    }


    /// <summary>
    /// Updates this instance.
    /// </summary>
    void Update () {

        //Only move the moleman if it is not disabled and has a target left
        if (!IsDisabled && targets.Count != 0)
        {
            //Set animator to move
            _animator.SetFloat("speed", 1f);

            //Transform the moleman
            float step = Speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targets[0].transform.position, step);
            Vector3 targetDir = targets[0].transform.position - transform.position;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
            Debug.DrawRay(transform.position, newDir, Color.red);
            transform.rotation = Quaternion.LookRotation(newDir);

            //Idle the moleman and notify the storyline if the moleman has reached its destination
            if (Distance(targets[0].transform.position, transform.position) < 1 && !_storylineNotified)
            {
                ThisStoryline.NextMoleMan();
                _storylineNotified = true;
            }
        }
        else
        {
            //When the moleman isn't moving, make it play the idle animation
            _animator.SetFloat("speed", 0f);
        }
    }

    /// <summary>
    /// Moves the moleman to the next position
    /// </summary>
    public void Next()
    {
        _storylineNotified = false;
        if(targets.Count != 0)
        {
            targets.RemoveAt(0);
        }
        
    }

    /// <summary>
    /// Finds the 2d distance between two Vector3s
    /// </summary>
    /// <param name="target">The target.</param>
    /// <param name="position">The position.</param>
    /// <returns></returns>
    private float Distance(Vector3 target, Vector3 position)
    {
        var xDifference = target.x - position.x;
        var zDifference = target.z - position.z;

        var distanceSquared = xDifference * xDifference + zDifference * zDifference;

        return Mathf.Sqrt(distanceSquared);
    }

    /// <summary>
    /// Teleports the moleman to its target
    /// </summary>
    public void Teleport()
    {
        transform.position = targets[0].transform.position;
        IsDisabled = true;
    }
}
