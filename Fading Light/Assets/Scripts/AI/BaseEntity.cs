using UnityEngine;
using System.Collections;

public class BaseEntity : MonoBehaviour{

	public float IntialHealth;
	public float CurrentHealth { get; protected set; }
	protected bool isDead;
    
    protected virtual void Start() {
        CurrentHealth = IntialHealth;
	}

	public virtual void Attacked(float damage, Transform attacker) {
		Damage(damage, attacker);
	}

	public virtual void Damage(float damage, Transform attacker) {

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