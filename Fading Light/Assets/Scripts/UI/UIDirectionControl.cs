using UnityEngine;

/// <summary>
/// This script will keep the rotation of objects static.
/// </summary>
public class UIDirectionControl : MonoBehaviour
{
    private Quaternion _rotation;
    /// <summary>
    /// Awake this instance.
    /// </summary>
    void Awake()
    {
        _rotation = transform.rotation;
    }
    /// <summary>
    /// This will ensure that the rotation remains constant.
    /// </summary>
    void LateUpdate()
    {
        transform.rotation = _rotation;
    }
}