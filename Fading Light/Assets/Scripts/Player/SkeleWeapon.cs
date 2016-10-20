// file:	Assets\Scripts\Player\SkeleWeapon.cs
//
// summary:	Implements the skele weapon class

using UnityEngine;
using System.Collections;

/// <summary>   A skele weapon. </summary>
///
/// <remarks>    . </remarks>

public class SkeleWeapon : MonoBehaviour
{

    /// <summary>   The weapon damage. </summary>
    public float WeaponDamage = 30f;
    /// <summary>   True to debug. </summary>
    private bool DEBUG = false;

    // Use this for initialization

    /// <summary>   Starts this object. </summary>
    ///
 

    void Start()
    {

    }

    // Update is called once per frame

    /// <summary>   Updates this object. </summary>
    ///
 

    void Update()
    {

    }

    /// <summary>   Executes the trigger enter action. </summary>
    ///
 
    ///
    /// <param name="other">    The other. </param>

    void OnTriggerEnter(Collider other)
    {
        SkeleBoss skele = this.transform.root.GetComponent<SkeleBoss>();

        if (skele.isAttacking() && (other.tag == "Player" || other.tag == "Player2"))
        {
            if (DEBUG) Debug.Log("Weapon collision: Enemy");
            other.transform.GetComponent<BaseEntity>().Damage(WeaponDamage, this.transform.root);
            skele.setAttacking(false);
        }
    }
}
