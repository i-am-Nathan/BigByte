// file:	Assets\Scripts\Interactables\RotatingDoor.cs
//
// summary:	Implements the rotating door class

using UnityEngine;
using System.Collections;

/// <summary>   A rotating door. </summary>
///
/// <remarks>    . </remarks>

public class RotatingDoor : MonoBehaviour {

    /// <summary>   Target angle. </summary>
    private int _targetAngle = 0;
    /// <summary>   The rotate speed. </summary>
    private int _rotateSpeed = 5;
    /// <summary>   True to moving. </summary>
    private bool _moving = false;

    /// <summary>   Updates this object. </summary>
    ///
 

    void Update()
    {
        Debug.Log(_targetAngle);
        Debug.Log(transform.rotation.eulerAngles.y);
        Debug.Log(_targetAngle != transform.rotation.eulerAngles.y);
        Debug.Log(_moving);
        if (_targetAngle != Mathf.Floor(transform.rotation.eulerAngles.y))
        {
  
            rotate();
        }
        else
        {
            _moving = false;
        }
    }

    /// <summary>   Rotate clockwise. </summary>
    ///
 

    public void rotateClockwise()
    {
        if (!_moving)
        {
            _moving = true;
            _targetAngle += 90;
            if (Mathf.Abs(Mathf.Floor(_targetAngle)) %360 == 0)
            {
                _targetAngle = 0;
            }
            Debug.Log(_targetAngle);
        }
    }

    /// <summary>   Rotates this object. </summary>
    ///
 

    public void rotate()
    {
        Debug.Log("rotating");
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, _targetAngle, 0));
        Debug.Log(targetRotation.eulerAngles);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotateSpeed);
    }
}
