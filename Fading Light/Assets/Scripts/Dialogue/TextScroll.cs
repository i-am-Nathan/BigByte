// file:	Assets\Scripts\Dialogue\TextScroll.cs
//
// summary:	Implements the text scroll class

using UnityEngine;
using System.Collections;

/// <summary>   Scroll dialogue text. </summary>
///
/// <remarks>    . </remarks>

public class TextScroll : MonoBehaviour {

    /// <summary>   The scrollspeed. </summary>
    int scrollspeed = 100;
    // Use this for initialization
    /// <summary>   The axis. </summary>
    private float xAxis;
    /// <summary>   The axis canvas. </summary>
    private float xAxisCanvas;
    /// <summary>   The time. </summary>
    public float time;

    /// <summary>   Starts this instance. </summary>
    ///
 

    void Start () {
        time = 20;
    }

    /// <summary>   Updates this instance. </summary>
    ///
 

    void Update () {
        Debug.Log(time);
        Vector3 pos = transform.position;


        
        Vector3 localVectorUp = transform.TransformDirection(1, 0, 0);

        pos += localVectorUp * scrollspeed * Time.deltaTime;
        transform.position = pos;

        if (time > 0)
        {
            time -= Time.deltaTime;
        }
        else { 
            Debug.Log("NEXTSCENE");
            //Application.LoadLevel(2);
        }
    }
}
