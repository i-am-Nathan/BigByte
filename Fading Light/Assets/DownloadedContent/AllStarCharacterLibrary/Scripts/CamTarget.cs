// file:	Assets\DownloadedContent\AllStarCharacterLibrary\Scripts\CamTarget.cs
//
// summary:	Implements the camera target class

using UnityEngine;
using System.Collections;

/// <summary>   A camera target. </summary>
///
/// <remarks>    . </remarks>

public class CamTarget : MonoBehaviour 
{

    /// <summary>   Target for the. </summary>
	public Transform target;
    /// <summary>   The camera speed. </summary>
	float camSpeed = 50.0f;
    /// <summary>   The linearly interpolate position. </summary>
	Vector3 lerpPos;

    /// <summary>   Updates this object. </summary>
    ///
 

	void Update() 
	{
		lerpPos = (target.position-transform.position)* Time.deltaTime * camSpeed;
		transform.position += lerpPos;
	}
}
