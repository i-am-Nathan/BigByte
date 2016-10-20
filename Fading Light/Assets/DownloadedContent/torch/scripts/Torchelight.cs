// file:	Assets\DownloadedContent\torch\scripts\Torchelight.cs
//
// summary:	Implements the torchelight class

using UnityEngine;
using System.Collections;

/// <summary>   A torchelight. </summary>
///
/// <remarks>    . </remarks>

public class Torchelight : MonoBehaviour {
	
    /// <summary>   The torch light. </summary>
	public GameObject TorchLight;
    /// <summary>   The main flame. </summary>
	public GameObject MainFlame;
    /// <summary>   The base flame. </summary>
	public GameObject BaseFlame;
    /// <summary>   The etincelles. </summary>
	public GameObject Etincelles;
    /// <summary>   The fumee. </summary>
	public GameObject Fumee;
    /// <summary>   The maximum light intensity. </summary>
	public float MaxLightIntensity;
    /// <summary>   The intensity light. </summary>
	public float IntensityLight;

    /// <summary>   Starts this object. </summary>
    ///
 

	void Start () {
		TorchLight.GetComponent<Light>().intensity=IntensityLight;
		MainFlame.GetComponent<ParticleSystem>().emissionRate=IntensityLight*20f;
		BaseFlame.GetComponent<ParticleSystem>().emissionRate=IntensityLight*15f;	
		Etincelles.GetComponent<ParticleSystem>().emissionRate=IntensityLight*7f;
		Fumee.GetComponent<ParticleSystem>().emissionRate=IntensityLight*12f;
	}

    /// <summary>   Updates this object. </summary>
    ///
 

	void Update () {
		if (IntensityLight<0) IntensityLight=0;
		if (IntensityLight>MaxLightIntensity) IntensityLight=MaxLightIntensity;		

		TorchLight.GetComponent<Light>().intensity=IntensityLight/2f+Mathf.Lerp(IntensityLight-0.1f,IntensityLight+0.1f,Mathf.Cos(Time.time*30));

		TorchLight.GetComponent<Light>().color=new Color(Mathf.Min(IntensityLight/1.5f,1f),Mathf.Min(IntensityLight/2f,1f),0f);
		MainFlame.GetComponent<ParticleSystem>().emissionRate=IntensityLight*20f;
		BaseFlame.GetComponent<ParticleSystem>().emissionRate=IntensityLight*15f;
		Etincelles.GetComponent<ParticleSystem>().emissionRate=IntensityLight*7f;
		Fumee.GetComponent<ParticleSystem>().emissionRate=IntensityLight*12f;		

	}
}
