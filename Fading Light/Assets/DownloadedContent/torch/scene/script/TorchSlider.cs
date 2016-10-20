// file:	Assets\DownloadedContent\torch\scene\script\TorchSlider.cs
//
// summary:	Implements the torch slider class

using UnityEngine;
using System.Collections;

/// <summary>   A torch slider. </summary>
///
/// <remarks>    . </remarks>

public class TorchSlider : MonoBehaviour {
    /// <summary>   The torche object. </summary>
	public GameObject TorcheObj;
    /// <summary>   The skin slider. </summary>
	public GUISkin SkinSlider;

    /// <summary>   Executes the graphical user interface action. </summary>
    ///
 

    void OnGUI() {
		GUI.Label(new Rect(25,25,150,30),"Light Intensity",SkinSlider.label);
        TorcheObj.GetComponent<Torchelight>().IntensityLight = GUI.HorizontalSlider(new Rect(25, 50, 150, 30), TorcheObj.GetComponent<Torchelight>().IntensityLight, 0.0F, TorcheObj.GetComponent<Torchelight>().MaxLightIntensity,SkinSlider.horizontalSlider,SkinSlider.horizontalSliderThumb);
    }
}