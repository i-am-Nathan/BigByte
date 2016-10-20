// file:	Assets\DownloadedContent\torch\scene\script\Torchslider2.cs
//
// summary:	Implements the torchslider 2 class

using UnityEngine;
using System.Collections;

/// <summary>   A torchslider 2. </summary>
///
/// <remarks>    . </remarks>

public class Torchslider2 : MonoBehaviour {
    /// <summary>   The torche object. </summary>
	public GameObject TorcheObj;
    /// <summary>   The main camera. </summary>
	public Camera MainCamera;
    /// <summary>   The skin slider. </summary>
	public GUISkin SkinSlider;
    /// <summary>   The torch. </summary>
	private GameObject Torch;
    /// <summary>   The intensity light. </summary>
	private float Intensity_Light;
    /// <summary>   True to camera rendering. </summary>
	private bool CameraRendering;

    /// <summary>   Executes the graphical user interface action. </summary>
    ///
 

    void OnGUI() {
		GUI.Label(new Rect(25,25,150,30),"Light Intensity",SkinSlider.label);
		Intensity_Light= GUI.HorizontalSlider(new Rect(25, 50, 150, 30), Intensity_Light, 0.0F, TorcheObj.GetComponent<Torchelight>().MaxLightIntensity,SkinSlider.horizontalSlider,SkinSlider.horizontalSliderThumb);
		CameraRendering=GUI.Toggle(new Rect(25,80,150,30),CameraRendering,"Deferred lighting");
		if (CameraRendering==true) {
			MainCamera.renderingPath=RenderingPath.DeferredLighting;
		}
		else {
			MainCamera.renderingPath=RenderingPath.Forward;
		}
		

	}

    /// <summary>   Updates this object. </summary>
    ///
 

	void Update() {
		foreach (GameObject i in GameObject.FindGameObjectsWithTag("TagLight")) {
			i.GetComponent<Torchelight>().IntensityLight=Intensity_Light;
		}
	}
}
