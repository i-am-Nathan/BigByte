using UnityEngine;
using System.Collections;

public class BaseEntity : MonoBehaviour{

	public float IntialHealth;
	public float CurrentHealth { get; protected set; }
	protected bool dead;
    
    protected virtual void Start() {
        CurrentHealth = IntialHealth;
	}

	public virtual void Attacked(float damage, Transform attacker) {
		TakeDamage(damage, null);
	}

	public virtual void TakeDamage(float damage, Transform attacker) {

        //Update current health - if killed call Killed() method
        CurrentHealth -= damage;
		if (CurrentHealth <= 0 && !dead) {
			Killed();
		}
	}

	public virtual void Killed() {
		dead = true;		
	}
}