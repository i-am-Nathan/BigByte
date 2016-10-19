using UnityEngine;

/// <summary>
/// Used to control the movement of the main camera
/// Referenced from https://unity3d.com/learn/tutorials/projects/tanks-tutorial/camera-control
/// </summary>
public class PlayerCam : MonoBehaviour
{
    //These are all modified in inspector
	public float DampTime;                 // Approximate time for the camera to refocus.
	public float ScreenEdgeBuffer;           // Space between the top/bottom most target and the screen edge.
    public float ScreenEdgeBufferTop;           // Space between the top/bottom most target and the screen edge.
    public float MinSize;                  // The smallest orthographic size the camera can be.
	public float MaxSize;                  // The smallest orthographic size the camera can be.
    public float XOffset;                 //Offsets the camera to account for it being on an angle
    public Transform[] Targets; // All the targets the camera needs to encompass.


    public GameObject SwoopPositionTarget;
    public GameObject SwoopAngleTarget;

	private Camera _camera;                        // Used for referencing the camera.
	private float _zoomSpeed;                      // Reference speed for the smooth damping of the orthographic size.
	private Vector3 _moveVelocity;                 // Reference velocity for the smooth damping of the position.
	private Vector3 _desiredPosition;              // The position the camera is moving towards.

    public int CameraState = 0;

    private Vector3 _startAngle;
    private Vector3 _startPosition;

    public int tmpFOV = 20;
    /// <summary>
    /// Awakes this instance.
    /// </summary>
    private void Awake ()
	{
		_camera = GetComponentInChildren<Camera> ();
        _startAngle = gameObject.transform.eulerAngles;
        _startPosition = gameObject.transform.position;
	}


    /// <summary>
    /// Fixeds the update.
    /// </summary>
    private void FixedUpdate ()
	{
        if (CameraState == 0)
        {
            // Move the camera towards a desired position.
            Move();

            // Change the size of the camera based.
            Zoom();

        }else if(CameraState == 1)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, new Vector3(SwoopPositionTarget.transform.position.x, SwoopPositionTarget.transform.position.y, SwoopPositionTarget.transform.position.z), 1f);

            var lookPos = SwoopAngleTarget.transform.position - transform.position;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 2);
            _camera.fieldOfView = Mathf.SmoothDamp(_camera.fieldOfView, tmpFOV, ref _zoomSpeed, DampTime);
            //swinging down
        }
		
	}


    /// <summary>
    /// Moves this instance.
    /// </summary>
    private void Move ()
	{
		// Find the average position of the targets.
		FindAveragePosition();

		// Smoothly transition to that position.
		transform.position = Vector3.SmoothDamp(transform.position, _desiredPosition, ref _moveVelocity, DampTime);

        float anglex = Mathf.MoveTowardsAngle(transform.eulerAngles.x, _startAngle.x, 30f * Time.deltaTime);
        float angley = Mathf.MoveTowardsAngle(transform.eulerAngles.y, _startAngle.y, 30f * Time.deltaTime);
        float anglez = Mathf.MoveTowardsAngle(transform.eulerAngles.z, _startAngle.z, 30f * Time.deltaTime);
        transform.eulerAngles = new Vector3(anglex, angley, anglez);
    }


    /// <summary>
    /// Finds the average position.
    /// </summary>
    private void FindAveragePosition ()
	{
		Vector3 averagePos = new Vector3 ();
		int numTargets = 0;

		// Go through all the targets and add their positions together.
		for (int i = 0; i < Targets.Length; i++)
		{
			// If the target isn't active, go on to the next one.
			if (!Targets[i].gameObject.activeSelf)
				continue;

			// Add to the average and increment the number of targets in the average.
			averagePos += Targets[i].position;
			numTargets++;
		}

		// If there are targets divide the sum of the positions by the number of them to find the average.
		if (numTargets > 0)
			averagePos /= numTargets;

		// Keep the same y value.
		averagePos.y = transform.position.y;

        //Offset the camera to account for it being on an angle
        averagePos.x += XOffset;

        averagePos.y = _startPosition.y;

        // The desired position is the average position;
        _desiredPosition = averagePos;

       

    }


    /// <summary>
    /// Zooms this instance.
    /// </summary>
    private void Zoom ()
	{
		// Find the required size based on the desired position and smoothly transition to that size.
		float requiredSize = FindRequiredSize();
		//m_Camera.orthographicSize = Mathf.SmoothDamp (m_Camera.orthographicSize, requiredSize, ref m_ZoomSpeed, _dampTime);
        _camera.fieldOfView = Mathf.SmoothDamp(_camera.fieldOfView, requiredSize, ref _zoomSpeed, DampTime);
    }


    /// <summary>
    /// Finds the size of the required.
    /// </summary>
    /// <returns></returns>
    private float FindRequiredSize ()
	{
		// Find the position the camera rig is moving towards in its local space.
		Vector3 desiredLocalPos = transform.InverseTransformPoint(_desiredPosition);

		// Start the camera's size calculation at zero.
		float size = 0f;

		// Go through all the targets...
		for (int i = 0; i < Targets.Length; i++)
		{
			// ... and if they aren't active continue on to the next target.
			if (!Targets[i].gameObject.activeSelf)
				continue;

			// Otherwise, find the position of the target in the camera's local space.
			Vector3 targetLocalPos = transform.InverseTransformPoint(Targets[i].position);

			// Find the position of the target from the desired position of the camera's local space.
			Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

			// Choose the largest out of the current size and the distance of the tank 'up' or 'down' from the camera.
			size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));

			// Choose the largest out of the current size and the calculated size based on the tank being to the left or right of the camera.
			size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / _camera.aspect);
		}

		// Add the edge buffer to the size.
		size += ScreenEdgeBuffer;

		// Make sure the camera's size isn't below the minimum.
		size = Mathf.Max (size, MinSize);

		size = Mathf.Min (size, MaxSize);

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
		transform.position = _desiredPosition;

		// Find and set the required size of the camera.
		_camera.orthographicSize = FindRequiredSize ();
	}

    public void SwingDown()
    {

    }
}