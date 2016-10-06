using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoleManContoller : MonoBehaviour {

    Animator _animator;

    public List<GameObject> targets;
    private float Speed = 8;
    public bool IsDisabled = true;
    public Storyline ThisStoryline;
    private bool _storylineNotified = false;

    // Use this for initialization
    void Start () {
        _animator = GetComponentInChildren<Animator>();//need this...
        
    }
	
	// Update is called once per frame
	void Update () {
        if (!IsDisabled && targets.Count != 0)
        {
            _animator.SetFloat("speed", 1f);
            float step = Speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targets[0].transform.position, step);

            Vector3 targetDir = targets[0].transform.position - transform.position;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
            Debug.DrawRay(transform.position, newDir, Color.red);
            transform.rotation = Quaternion.LookRotation(newDir);

            if (Distance(targets[0].transform.position, transform.position) < 1 && !_storylineNotified)
            {
                ThisStoryline.MoleManInPosition();
                _storylineNotified = true;
            }
        }
        else
        {
            _animator.SetFloat("speed", 0f);
        }
    }

    public void Next()
    {
        _storylineNotified = false;
        if(targets.Count != 0)
        {
            targets.RemoveAt(0);
        }
        
    }

    private float Distance(Vector3 target, Vector3 position)
    {
        var xDifference = target.x - position.x;
        var zDifference = target.z - position.z;

        var distanceSquared = xDifference * xDifference + zDifference * zDifference;

        return Mathf.Sqrt(distanceSquared);
    }

    public void Teleport()
    {
        transform.position = targets[0].transform.position;
        IsDisabled = true;
    }
}
