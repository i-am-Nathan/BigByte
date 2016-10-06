using UnityEngine;
using System.Collections;
using System;

public class BaseEntity : MonoBehaviour{

    public float IntialHealth = 50;
	public float CurrentHealth { get; protected set; }
	public bool isDead;
    private bool DEBUG = true;
    
    protected virtual void Start() {
        CurrentHealth = IntialHealth;
	}

	public void Attacked(float damage, Transform attacker) {
		Damage(damage, attacker);
	}

	public virtual void Damage(float damage, Transform attacker) {
        if (DEBUG) Debug.Log("Base entity damaged.");
        //Update current health - if killed call Killed() method
        CurrentHealth -= damage;
		if (CurrentHealth <= 0 && !isDead) {
			Killed();
		}
	}

	public virtual void Killed() {
		isDead = true;		
	}  
}