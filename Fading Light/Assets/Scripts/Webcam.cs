using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Webcam : MonoBehaviour
{
    public RawImage rawimage;

    void Start()
    {
        Application.RequestUserAuthorization(UserAuthorization.WebCam | UserAuthorization.Microphone);
        
       
        
    }
    bool started = false;
    void Update()
    {
        if (!started && Application.HasUserAuthorization(UserAuthorization.WebCam | UserAuthorization.Microphone))
        {
            Debug.Log("Started");
            WebCamTexture webcamTexture = new WebCamTexture();
            rawimage.texture = webcamTexture;
            rawimage.material.mainTexture = webcamTexture;
            webcamTexture.Play();
            started = true;
        }
        else
        {
        }
    }
}