using UnityEngine;
using System.Collections;

public class RotatingDoor : MonoBehaviour {

    private int _targetAngle = 0;
    private int _rotateSpeed = 5;
    private bool _moving = false;

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

    public void rotate()
    {
        Debug.Log("rotating");
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, _targetAngle, 0));
        Debug.Log(targetRotation.eulerAngles);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotateSpeed);
    }
}
