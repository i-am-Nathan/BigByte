// file:	Assets\Scripts\Mobs\BaseEntity.cs
//
// summary:	Implements the base entity class

using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// The class which all damageable entities inherit from. It tracks values such as health and
/// whether or not an entity is dead. It also handles all generic functions relating to these
/// functions.
/// </summary>
///
/// <remarks>    . </remarks>

public class BaseEntity : MonoBehaviour{

    /// <summary>   The intial health. </summary>
    public float IntialHealth = 50;

    /// <summary>   Gets or sets the current health. </summary>
    ///
    /// <value> The current health. </value>

	public float CurrentHealth { get; protected set; }
    /// <summary>   True if this object is dead. </summary>
	public bool isDead;
    /// <summary>   True to debug. </summary>
    private bool DEBUG = false;

    /// <summary>   Starts this object. </summary>
    ///
 

    protected virtual void Start() {
        CurrentHealth = IntialHealth;
	}

    /// <summary>   Attacked. </summary>
    ///
 
    ///
    /// <param name="damage">   The damage. </param>
    /// <param name="attacker"> The attacker. </param>

	public void Attacked(float damage, Transform attacker) {
		Damage(damage, attacker);
	}

    /// <summary>   Damages. </summary>
    ///
 
    ///
    /// <param name="damage">   The damage. </param>
    /// <param name="attacker"> The attacker. </param>

	public virtual void Damage(float damage, Transform attacker) {
        if (DEBUG) Debug.Log("Base entity damaged.");
        //Update current health - if killed call Killed() method
        CurrentHealth -= damage;
		if (CurrentHealth <= 0 && !isDead) {
			Killed();
		}
	}

    /// <summary>   Killed this object. </summary>
    ///
 

	public virtual void Killed() {
		isDead = true;		
	}  
}