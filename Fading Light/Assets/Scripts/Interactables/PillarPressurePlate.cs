using UnityEngine;
using System.Collections;

public class PillarPressurePlate : MonoBehaviour {

    private int _thingsOnTop = 0;
    private bool _pressed = false;


    void OnTriggerEnter(Collider other)
    {


        if ((other.tag == "Player") || (other.tag == "Player2")) {
            _thingsOnTop++;
            Debug.Log(other);
            Debug.Log("on" + _thingsOnTop);
            //if the weight is heavy enough, then the plate is triggered
            if (_thingsOnTop >= 1 && !_pressed)

            {
                GameObject parent = transform.parent.gameObject;
                this.GetComponent<Animation>().Play("CirclePlateDown");
                Debug.LogWarning("down");
                Transform lightning = transform.Find("Lightning");
                ParticleSystem lightningPS = lightning.GetComponent<ParticleSystem>();

                lightningPS.startSize = 3;
                var em = lightningPS.emission;
                em.rate = new ParticleSystem.MinMaxCurve(50);


                Transform cloud = lightning.Find("Cloud");
                cloud.GetComponent<ParticleSystem>().startSize = 1.5f;

                //set pillar to charged state
                _pressed = true;

            }
        }
        

    }

    void OnTriggerExit(Collider other)
    {
        if ((other.tag == "Player") || (other.tag == "Player2"))
            {
            Debug.Log(other);
            _thingsOnTop--;
            Debug.Log("off" + _thingsOnTop);
            //if the weight is heavy enough, then the plate is triggered
            if (_thingsOnTop < 1 && _pressed)

            {
                GameObject parent = transform.parent.gameObject;
                this.GetComponent<Animation>().Play("CirclePlateUp");
                Debug.LogWarning("up");
                //set pillar to uncharged state
                Transform lightning = transform.Find("Lightning");
                ParticleSystem lightningPS = lightning.GetComponent<ParticleSystem>();
                lightningPS.startSize = 1;

                var em = lightningPS.emission;
                em.rate = new ParticleSystem.MinMaxCurve(15);

                Transform cloud = lightning.Find("Cloud");
                cloud.GetComponent<ParticleSystem>().startSize = 1;
                _pressed = false;

            }
        }
            

    }
}
