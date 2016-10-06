﻿using UnityEngine;

/// <summary>
/// Used to control the movement of the main camera
/// </summary>
public class PlayerCam : MonoBehaviour
{
    //These are all modified in inspector
	public float _dampTime;                 // Approximate time for the camera to refocus.
	public float _screenEdgeBuffer;           // Space between the top/bottom most target and the screen edge.
    public float _screenEdgeBufferTop;           // Space between the top/bottom most target and the screen edge.
    public float _minSize;                  // The smallest orthographic size the camera can be.
	public float _maxSize;                  // The smallest orthographic size the camera can be.
    public float _xOffset;                 //Offsets the camera to account for it being on an angle
    /*[HideInInspector]*/
    public Transform[] m_Targets; // All the targets the camera needs to encompass.


	private Camera m_Camera;                        // Used for referencing the camera.
	private float m_ZoomSpeed;                      // Reference speed for the smooth damping of the orthographic size.
	private Vector3 m_MoveVelocity;                 // Reference velocity for the smooth damping of the position.
	private Vector3 m_DesiredPosition;              // The position the camera is moving towards.


    /// <summary>
    /// Awakes this instance.
    /// </summary>
    private void Awake ()
	{
		m_Camera = GetComponentInChildren<Camera> ();
	}


    /// <summary>
    /// Fixeds the update.
    /// </summary>
    private void FixedUpdate ()
	{
		// Move the camera towards a desired position.
		Move ();

		// Change the size of the camera based.
		Zoom ();
	}


    /// <summary>
    /// Moves this instance.
    /// </summary>
    private void Move ()
	{
		// Find the average position of the targets.
		FindAveragePosition();

		// Smoothly transition to that position.
		transform.position = Vector3.SmoothDamp(transform.position, m_DesiredPosition, ref m_MoveVelocity, _dampTime);
	}


    /// <summary>
    /// Finds the average position.
    /// </summary>
    private void FindAveragePosition ()
	{
		Vector3 averagePos = new Vector3 ();
		int numTargets = 0;

		// Go through all the targets and add their positions together.
		for (int i = 0; i < m_Targets.Length; i++)
		{
			// If the target isn't active, go on to the next one.
			if (!m_Targets[i].gameObject.activeSelf)
				continue;

			// Add to the average and increment the number of targets in the average.
			averagePos += m_Targets[i].position;
			numTargets++;
		}

		// If there are targets divide the sum of the positions by the number of them to find the average.
		if (numTargets > 0)
			averagePos /= numTargets;

		// Keep the same y value.
		averagePos.y = transform.position.y;

        //Offset the camera to account for it being on an angle
        averagePos.x += _xOffset; 


		// The desired position is the average position;
		m_DesiredPosition = averagePos;
	}


    /// <summary>
    /// Zooms this instance.
    /// </summary>
    private void Zoom ()
	{
		// Find the required size based on the desired position and smoothly transition to that size.
		float requiredSize = FindRequiredSize();
		//m_Camera.orthographicSize = Mathf.SmoothDamp (m_Camera.orthographicSize, requiredSize, ref m_ZoomSpeed, _dampTime);
        m_Camera.fieldOfView = Mathf.SmoothDamp(m_Camera.fieldOfView, requiredSize, ref m_ZoomSpeed, _dampTime);
    }


    /// <summary>
    /// Finds the size of the required.
    /// </summary>
    /// <returns></returns>
    private float FindRequiredSize ()
	{
		// Find the position the camera rig is moving towards in its local space.
		Vector3 desiredLocalPos = transform.InverseTransformPoint(m_DesiredPosition);

		// Start the camera's size calculation at zero.
		float size = 0f;

		// Go through all the targets...
		for (int i = 0; i < m_Targets.Length; i++)
		{
			// ... and if they aren't active continue on to the next target.
			if (!m_Targets[i].gameObject.activeSelf)
				continue;

			// Otherwise, find the position of the target in the camera's local space.
			Vector3 targetLocalPos = transform.InverseTransformPoint(m_Targets[i].position);

			// Find the position of the target from the desired position of the camera's local space.
			Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

			
			size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));
            
			size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / m_Camera.aspect);
		}

		// Add the edge buffer to the size.
		size += _screenEdgeBuffer;

		// Make sure the camera's size isn't below the minimum.
		size = Mathf.Max (size, _minSize);

		size = Mathf.Min (size, _maxSize);

		return size;
	}


    /// <summary>
    /// Sets the start size of the position and.
    /// </summary>
    public void SetStartPositionAndSize ()
	{
		// Find the desired position.
		FindAveragePosition ();

    

		// Set the camera's position to the desired position without damping.
		transform.position = m_DesiredPosition;

		// Find and set the required size of the camera.
		m_Camera.orthographicSize = FindRequiredSize ();
	}
}