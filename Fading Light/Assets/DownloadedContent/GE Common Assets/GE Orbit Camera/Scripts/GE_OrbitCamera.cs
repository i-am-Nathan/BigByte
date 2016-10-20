// GE Common Assets 1.0
// Free asssets for using in many packages of Gold Experience Team.
//
// Author:	Gold Experience Team (http://www.ge-team.com)

// Support:	geteamdev@gmail.com
//
// Please direct any bugs/comments/suggestions to support e-mail.


#region Namespaces

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

#endregion // Namespaces

// ######################################################################
// GE_OrbitCamera class
// Handles mouse and touch inputs for orbiting the camera around the target object.
// ######################################################################

/// <summary>   A ge orbit camera. </summary>
///
/// <remarks>    . </remarks>

public class GE_OrbitCamera : MonoBehaviour
{

	// ########################################
	// Variables
	// ########################################

	#region Variables

    /// <summary>   Target for the. </summary>
	public Transform m_Target;

	// Distance
    /// <summary>   The distance. </summary>
	private float m_Distance = 10.0f;
    /// <summary>   The minimum distance. </summary>
	public float m_MinDistance = 5.0f;
    /// <summary>   The maximum distance. </summary>
	public float m_MaxDistance = 25.0f;

	// Orbit speed
    /// <summary>   The speed. </summary>
	public float m_XSpeed = 250.0f;
    /// <summary>   The speed. </summary>
	public float m_YSpeed = 120.0f;

	// Zoom speed
    /// <summary>   The zoom speed. </summary>
	public float m_ZoomSpeed = 5.0f;

	// Orbit inversion
    /// <summary>   True to invert. </summary>
	public bool m_XInvert = false;
    /// <summary>   True to invert. </summary>
	public bool m_YInvert = false;

	// Zoom inversion
    /// <summary>   True to zoom invert. </summary>
	public bool m_ZoomInvert = false;

	// y angle limit
    /// <summary>   The minimum limit. </summary>
	public float m_YMinLimit = -20f;
    /// <summary>   The maximum limit. </summary>
	public float m_YMaxLimit = 70f;

	// Angles
    /// <summary>   The x coordinate. </summary>
	private float m_X = 0.0f;
    /// <summary>   The y coordinate. </summary>
	private float m_Y = 0.0f;

	// Orbit Sensitivity
    /// <summary>   The orbit speed delay time. </summary>
	private float m_OrbitSpeedDelayTime = 0.0f;
    /// <summary>   The orbit speed multiplier. </summary>
	public float m_OrbitSpeedMultiplier = 2.0f;

	// Two fingers touch zoom
    /// <summary>   The first m curr touch. </summary>
	private Vector2 m_CurrTouch1 = Vector2.zero;
    /// <summary>   The first m last touch. </summary>
	private Vector2 m_LastTouch1 = Vector2.zero;
    /// <summary>   The second m curr touch. </summary>
	private Vector2 m_CurrTouch2 = Vector2.zero;
    /// <summary>   The second m last touch. </summary>
	private Vector2 m_LastTouch2 = Vector2.zero;
    /// <summary>   The curr distance. </summary>
	private float m_CurrDist = 0.0f;
    /// <summary>   The last distance. </summary>
	private float m_LastDist = 0.0f;

    /// <summary>   Values that represent zoom methods. </summary>
    ///
 

	public enum ZoomMethod
	{
        /// <summary>   An enum constant representing the pinch zoom option. </summary>
		PinchZoom = 1,
        /// <summary>   An enum constant representing the slide zoom option. </summary>
		SlideZoom = 2
	}
    /// <summary>   The zoom method. </summary>
	public ZoomMethod m_ZoomMethod = ZoomMethod.PinchZoom;

	#endregion // Variables

	// ########################################
	// MonoBehaviour Functions
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.html
	// ########################################

	#region MonoBehaviour

	// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html

    /// <summary>   Starts this object. </summary>
    ///
 

	void Start()
	{
		this.gameObject.transform.LookAt(m_Target);
		var angles = transform.eulerAngles;
		m_X = angles.y;
		m_Y = angles.x;

		// Check Min and Max distance.
		if (m_MaxDistance < m_MinDistance)
			m_MaxDistance = m_MinDistance + 1;
		if (m_MinDistance > m_MaxDistance)
			m_MinDistance = m_MaxDistance + 1;

		// Set start position according to distance
		bool ShouldUpdateZoom = false;
		m_Distance = Vector3.Distance(this.transform.position, m_Target.transform.position);

		if (m_Distance < m_MinDistance)
		{
			m_Distance = m_MinDistance;
			ShouldUpdateZoom = true;
		}
		else if (m_Distance > m_MaxDistance)
		{
			m_Distance = m_MaxDistance;
			ShouldUpdateZoom = true;
		}
		if (ShouldUpdateZoom == true)
		{
			UpdatePosition();
		}

		// Make the rigid body not change rotation
		if (GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true;

	}

	// Update is called every frame, if the MonoBehaviour is enabled.
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html

    /// <summary>   Updates this object. </summary>
    ///
 

	void Update()
	{
		if (m_Target)
		{
			bool ShouldUpdateZoom = false;
			bool ShouldUpdateOrbit = false;

			////////////////////////////////////////////////
			// ZOOM
			////////////////////////////////////////////////

			// Touch or mouse flag for zoom
			bool ZoomedbyTouch = false;

			if (Input.touchCount == 2)
			{
				// Use two fingers to pinch zoom
				if (m_ZoomMethod == ZoomMethod.PinchZoom)
				{
					if (Input.touchCount == 2)
					{
						ZoomedbyTouch = true;
						ShouldUpdateZoom = true;

						for (int i = 0; i < Input.touchCount; i++)
						{
							Touch touch = Input.GetTouch(i);
							// Touch moved
							if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
							{
								if (i == 0)
								{
									m_CurrTouch1 = touch.position;
									m_LastTouch1 = m_CurrTouch1 - touch.deltaPosition;
								}
								else
								{
									m_CurrTouch2 = touch.position;
									m_LastTouch2 = m_CurrTouch2 - touch.deltaPosition;
								}
							}
						}

						m_CurrDist = Vector2.Distance(m_CurrTouch1, m_CurrTouch2);
						m_LastDist = Vector2.Distance(m_LastTouch1, m_LastTouch2);

						// Calculate the zoom magnitude
						float delta = m_LastDist - m_CurrDist;

						// Invert zoom
						if (m_ZoomInvert == true)
							delta *= -1;

						m_Distance += delta * m_ZoomSpeed * 0.01f;
						if (m_Distance < m_MinDistance)
						{
							m_Distance = m_MinDistance;
						}
						else if (m_Distance > m_MaxDistance)
						{
							m_Distance = m_MaxDistance;
						}
					}
					// reset touch
					else if (Input.touchCount < 2)
					{
						m_CurrDist = 0.0f;
						m_LastDist = 0.0f;
					}
				}
				// Use two fingers slide up/down to zoom
				else if (m_ZoomMethod == ZoomMethod.SlideZoom)
				{
					if (Input.touchCount == 2)
					{
						ZoomedbyTouch = true;
						ShouldUpdateZoom = true;

						for (int i = 0; i < Input.touchCount; i++)
						{
							Touch touch = Input.GetTouch(i);
							// Touch moved
							if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
							{
								if (i == 0)
								{
									m_CurrTouch1 = touch.position;
									m_LastTouch1 = m_CurrTouch1 - touch.deltaPosition;
								}
								else
								{
									m_CurrTouch2 = touch.position;
									m_LastTouch2 = m_CurrTouch2 - touch.deltaPosition;
								}
							}
						}

						// Calculate the zoom magnitude
						float PinchZoomFactor = Vector2.Distance(m_LastTouch1, m_LastTouch2) - Vector2.Distance(m_CurrTouch1, m_CurrTouch2);

						if (PinchZoomFactor > -2.0f && PinchZoomFactor < 2.0f)
						{
							ZoomedbyTouch = true;
							ShouldUpdateZoom = true;

							m_CurrDist = (m_CurrTouch1.y + m_CurrTouch2.y) / 2.0f;
							m_LastDist = (m_LastTouch1.y + m_LastTouch2.y) / 2.0f;

							//Calculate the zoom magnitude
							float delta = m_LastDist - m_CurrDist;

							// Invert zoom
							if (m_ZoomInvert == true)
								delta *= -1;

							m_Distance += delta * m_ZoomSpeed * 0.01f;

							if (m_Distance < m_MinDistance)
							{
								m_Distance = m_MinDistance;
							}
							else if (m_Distance > m_MaxDistance)
							{
								m_Distance = m_MaxDistance;
							}
						}

					}
					// reset touch
					else if (Input.touchCount < 2)
					{
						m_CurrDist = 0.0f;
						m_LastDist = 0.0f;
					}
				}
			}

			float deadZone = 0.01f;
			// Zoom by mouse
			if (ZoomedbyTouch == false)
			{
				if (Input.GetAxis("Mouse ScrollWheel") < -deadZone || Input.GetAxis("Mouse ScrollWheel") > deadZone)
				{
					ShouldUpdateZoom = true;

					float delta = Input.GetAxis("Mouse ScrollWheel");

					// Invert zoom
					if (m_ZoomInvert == true)
						delta *= -1;

					m_Distance -= delta * m_ZoomSpeed;
					if (m_Distance < m_MinDistance)
					{
						m_Distance = m_MinDistance;
					}
					else if (m_Distance > m_MaxDistance)
					{
						m_Distance = m_MaxDistance;
					}
				}
			}

			////////////////////////////////////////////////
			// ORBIT
			////////////////////////////////////////////////

			// Touch or mouse flag for orbit
			bool OrbitByTouch = false;

			// Orbit by touch
			if (Input.touchCount == 1)
			{
				OrbitByTouch = true;

				// Get touch
				Touch touch = Input.GetTouch(0);

				// Touch began
				if (touch.phase == TouchPhase.Began)
				{
					m_OrbitSpeedDelayTime = 0.0f;
				}
				// Touch ended or canceld
				else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
				{
					m_OrbitSpeedDelayTime = 0.0f;
				}
				// Touch moved
				else if (touch.phase == TouchPhase.Moved)
				{
					ShouldUpdateOrbit = true;

					float TouchDeltaX = touch.deltaPosition.x;
					float TouchDeltaY = touch.deltaPosition.y;

					// Invert X and Y axis
					if (m_XInvert)
						TouchDeltaX *= -1;
					if (m_YInvert)
						TouchDeltaY *= -1;

					m_X += touch.deltaPosition.x * m_XSpeed * 0.005f * m_OrbitSpeedDelayTime;
					m_Y -= touch.deltaPosition.y * m_YSpeed * 0.005f * m_OrbitSpeedDelayTime;
				}
			}

			// Orbit by mouse
			if (OrbitByTouch == false)
			{
				// Reset orbit speed when left mouse down
				if (Input.GetMouseButtonDown(0))
				{
					m_OrbitSpeedDelayTime = 0.0f;
				}
				else if (Input.GetMouseButtonUp(0))
				{
					m_OrbitSpeedDelayTime = 0.0f;
				}
				if (Input.GetMouseButton(0))
				{
					ShouldUpdateOrbit = true;

					float MouseAxisX = Input.GetAxis("Mouse X");
					float MouseAxisY = Input.GetAxis("Mouse Y");

					// Invert X and Y axis
					if (m_XInvert)
						MouseAxisX *= -1;
					if (m_YInvert)
						MouseAxisY *= -1;

					m_X += MouseAxisX * m_XSpeed * 0.02f * m_OrbitSpeedDelayTime;
					m_Y -= MouseAxisY * m_YSpeed * 0.02f * m_OrbitSpeedDelayTime;

				}
			}

			////////////////////////////////////////////////
			// UPDATE POSITON
			////////////////////////////////////////////////

			if (ShouldUpdateZoom || ShouldUpdateOrbit)
			{
				UpdatePosition();
			}
		}
	}

	#endregion // MonoBehaviour

	// ########################################
	// Utilities functions
	// ########################################

	#region Utilities

	// Update camera position

    /// <summary>   Updates the position. </summary>
    ///
 

	void UpdatePosition()
	{
		m_OrbitSpeedDelayTime += Time.deltaTime * m_OrbitSpeedMultiplier;
		if (m_OrbitSpeedDelayTime > 1.0f)
			m_OrbitSpeedDelayTime = 1.0f;

		m_Y = ClampAngle(m_Y, m_YMinLimit, m_YMaxLimit);

		Quaternion rotation = Quaternion.Euler(m_Y, m_X, 0);

		Vector3 direction = new Vector3(0.0f, 0.0f, -m_Distance);
		Vector3 position = (rotation * direction) + m_Target.position;

		transform.rotation = rotation;
		transform.position = position;
	}

	// Clamp angle around 0 to 360 degree

    /// <summary>   Clamp angle. </summary>
    ///
 
    ///
    /// <param name="angle">    The angle. </param>
    /// <param name="min">      The minimum. </param>
    /// <param name="max">      The maximum. </param>
    ///
    /// <returns>   A float. </returns>

	static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360)
			angle += 360;
		if (angle > 360)
			angle -= 360;
		return Mathf.Clamp(angle, min, max);
	}

	// Set zooming method

    /// <summary>   Sets zoom method. </summary>
    ///
 
    ///
    /// <param name="zoomMethod">   The zoom method. </param>

	public void SetZoomMethod(ZoomMethod zoomMethod)
	{
		m_ZoomMethod = zoomMethod;
	}

	#endregion Utilities
}
