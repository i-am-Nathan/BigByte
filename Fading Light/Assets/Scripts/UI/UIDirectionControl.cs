// file:	Assets\Scripts\UI\UIDirectionControl.cs
//
// summary:	Implements the direction control class

using UnityEngine;

/// <summary>   This script will keep the rotation of objects static. </summary>
///
/// <remarks>    . </remarks>

public class UIDirectionControl : MonoBehaviour
{
    /// <summary>   The rotation. </summary>
    private Quaternion _rotation;

    /// <summary>   Awake this instance. </summary>
    ///
 

    void Awake()
    {
        _rotation = transform.rotation;
    }

    /// <summary>   This will ensure that the rotation remains constant. </summary>
    ///
 

    void LateUpdate()
    {
        transform.rotation = _rotation;
    }
}