// file:	Assets\Scripts\Mobs\FireAOE.cs
//
// summary:	Implements the fire an oe class

using UnityEngine;
using System.Collections;

/// <summary>   A fire an oe. </summary>
///
/// <remarks>    . </remarks>

public class FireAOE : MonoBehaviour {

    /// <summary>   The damage per frame. </summary>
    public float DamagePerFrame = 2f;
    /// <summary>   True to debug. </summary>
    private bool DEBUG = false;

    /// <summary>   Executes the trigger stay action. </summary>
    ///
 
    ///
    /// <param name="other">    The other. </param>

    void OnTriggerStay(Collider other)
    {
        if (DEBUG) Debug.Log("Collison detected");
        if (other.tag == "Player" || other.tag == "Player2")
        {
            if (DEBUG) Debug.Log("Moledog AOE collision: Player");
            other.GetComponent<Player>().Damage(DamagePerFrame, this.transform.root);             
        }
    }
}
