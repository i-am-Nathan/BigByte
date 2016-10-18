using UnityEngine;
using System.Collections;

/// <summary>
/// Scroll dialogue text
/// </summary>
public class TextScroll : MonoBehaviour {

    int scrollspeed = 100;
    // Use this for initialization
    private float xAxis;
    private float xAxisCanvas;
    public float time;


    /// <summary>
    /// Starts this instance.
    /// </summary>
    void Start () {
        time = 20;
    }

    /// <summary>
    /// Updates this instance.
    /// </summary>
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
