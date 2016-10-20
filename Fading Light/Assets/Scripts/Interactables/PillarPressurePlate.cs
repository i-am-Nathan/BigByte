// file:	Assets\Scripts\Interactables\PillarPressurePlate.cs
//
// summary:	Implements the pillar pressure plate class

using UnityEngine;
using System.Collections;

/// <summary>   A pillar pressure plate. </summary>
///
/// <remarks>    . </remarks>

public class PillarPressurePlate : MonoBehaviour {

    /// <summary>   The things on top. </summary>
    private int _thingsOnTop = 0;
    /// <summary>   True if pressed. </summary>
    private bool _pressed = false;
    /// <summary>   True to on cooldown. </summary>
    public bool _onCooldown = false;
    /// <summary>   The pillar number. </summary>
    public int pillarNumber;
    /// <summary>   The cooldown start time. </summary>
    private float _cooldownStartTime;

    /// <summary>   Updates this object. </summary>
    ///
 

    void Update()
    {
        if (_onCooldown && ((Time.fixedTime - _cooldownStartTime)>10f))
        {
            takeOffCoolDown();
        }
        
    }

    /// <summary>   Executes the trigger enter action. </summary>
    ///
 
    ///
    /// <param name="other">    The other. </param>

    void OnTriggerEnter(Collider other)
    {

        if ((other.tag == "Player") || (other.tag == "Player2")) {
            _thingsOnTop++;
            //if the weight is heavy enough, then the plate is triggered
            if (_thingsOnTop >= 2 && !_pressed)

            {
                GameObject parent = transform.parent.gameObject;
                this.GetComponent<Animation>().Play("CirclePlateDown");

                //set pillar to charged state
                Transform lightning = transform.Find("Lightning");
                ParticleSystem lightningPS = lightning.GetComponent<ParticleSystem>();
                lightningPS.startSize = 3;
                var em = lightningPS.emission;
                em.rate = new ParticleSystem.MinMaxCurve(50);
                Transform cloud = lightning.Find("Cloud");
                cloud.GetComponent<ParticleSystem>().startSize = 1.5f;

                //notify main pillar
                transform.parent.parent.GetComponent<MainPillar>().activatePillar(this);

                _pressed = true;
            }
        }

    }

    /// <summary>   Executes the trigger exit action. </summary>
    ///
 
    ///
    /// <param name="other">    The other. </param>

    void OnTriggerExit(Collider other)
    {
        if ((other.tag == "Player") || (other.tag == "Player2"))
            {
            _thingsOnTop--;
            //if the weight is heavy enough, then the plate is triggered
            if (_thingsOnTop < 1 && _pressed)

            {
                GameObject parent = transform.parent.gameObject;
                this.GetComponent<Animation>().Play("CirclePlateUp");
                
                //set pillar to uncharged state
                Transform lightning = transform.Find("Lightning");
                ParticleSystem lightningPS = lightning.GetComponent<ParticleSystem>();
                lightningPS.startSize = 1;
                var em = lightningPS.emission;
                em.rate = new ParticleSystem.MinMaxCurve(15);
                Transform cloud = lightning.Find("Cloud");
                cloud.GetComponent<ParticleSystem>().startSize = 1;

                //notify main pillar
                transform.parent.parent.GetComponent<MainPillar>().deactivatePillar(this);

                _pressed = false;

            }
        }

    }

    /// <summary>   Puts on cool down. </summary>
    ///
 

    public void putOnCoolDown()
    {
        transform.Find("Lightning").GetComponent<ParticleSystem>().Stop();
        //set on cooldown
        _onCooldown = true;
        //start timing
        _cooldownStartTime = Time.fixedTime;
    }

    /// <summary>   Take off cool down. </summary>
    ///
 

    public void takeOffCoolDown()
    {
        _onCooldown = false;
        //start playing particles
        transform.Find("Lightning").GetComponent<ParticleSystem>().Play();
    }
}
