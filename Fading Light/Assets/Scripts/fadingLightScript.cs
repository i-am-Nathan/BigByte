using UnityEngine;
using System.Collections;

public class fadingLightScript : MonoBehaviour {
    public Light lt;
    private bool isFade= false;

    // Use this for initialization
    void Start()
    {
        lt = GetComponent<Light>();
    }
    // Update is called once per frame
    void Update () {
        if(lt.intensity < 0.3f)
        {
            isFade = false;
        }
        else if(lt.intensity > 1.5f)
        {
            isFade = true;
        }

        if (isFade == false)
        {
            lt.intensity += 0.005f;
        }
        else if (isFade == true) 
        {
            lt.intensity -= 0.005f;
        }
	}
}
