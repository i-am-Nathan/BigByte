// file:	Assets\Scripts\Mobs\SpiderBehaviour.cs
//
// summary:	Implements the spider behaviour class

using UnityEngine;
using System.Collections;

/// <summary>   A spider behaviour. </summary>
///
/// <remarks>    . </remarks>

[RequireComponent (typeof (NavMeshAgent))]
public class SpiderBehaviour : BaseEntity {
    /*
	public enum State {
		Idle, 
		Chasing, 
		Attacking
	};	
	State currentState;

	public ParticleSystem deathEffect;
	public static event System.Action OnDeathStatic;

	NavMeshAgent pathfinder;

	Transform target;
	BaseEntity targetBaseEntity;
	Material skinMaterial;
	Color originalColour;

	public float attackDistanceThreshold = .5f;
	public float timeBetweenAttacks = 1;
	public float damage = 1;

	float nextAttackTime;
	float myCollisionRadius;
	float targetCollisionRadius;

	bool lockedOn;

	void Awake() {
		
		pathfinder = GetComponent<NavMeshAgent> ();
		Transform player1 = GameObject.FindGameObjectWithTag ("Player").transform;
		Transform player2 = GameObject.FindGameObjectWithTag ("Player2").transform;

        //Find closest player to attack
        if (Vector3.Distance(player1.position, this.gameObject.transform.position) > Vector3.Distance(player1.position, this.gameObject.transform.position))
        {
            target = player1;
            lockedOn = true;
        }
        else
        {
            target = player2;
            lockedOn = true;
        }


        targetEntity = target.GetComponent<BaseEntity> ();
		myCollisionRadius = GetComponent<CapsuleCollider> ().radius;
		targetCollisionRadius = target.GetComponent<CapsuleCollider> ().radius;
	}

	protected override void Start () {
		base.Start();

		if (hasTarget) {
			currentState = State.Chasing;
			targetEntity.OnDeath += OnTargetDeath;

			StartCoroutine (UpdatePath());
		}
	}

	public void SetCharacteristics(float moveSpeed, int hitsToKillPlayer, float enemyHealth, Color skinColour) {
		pathfinder.speed = moveSpeed;

		if (hasTarget) {
			damage = Mathf.Ceil(targetEntity.startingHealth / hitsToKillPlayer);
		}
		startingHealth = enemyHealth;

		deathEffect.startColor = new Color (skinColour.r, skinColour.g, skinColour.b, 1);
		skinMaterial = GetComponent<Renderer> ().material;
		skinMaterial.color = skinColour;
		originalColour = skinMaterial.color;
	}


	public override void Attacked(float damage, Transform attacker)
	{
		if (damage >= health) {
			if (OnDeathStatic != null) {
				OnDeathStatic ();
			}
		}
		base.Attacked(damage, hitPoint);
	}

	void OnTargetDeath() {
		hasTarget = false;
		currentState = State.Idle;
	}

	void Update () {

		if (hasTarget) {
			if (Time.time > nextAttackTime) {
				float sqrDstToTarget = (target.position - transform.position).sqrMagnitude;
				if (sqrDstToTarget < Mathf.Pow (attackDistanceThreshold + myCollisionRadius + targetCollisionRadius, 2)) {
					nextAttackTime = Time.time + timeBetweenAttacks;
					StartCoroutine (Attack ());
				}

			}
		}

	}

	IEnumerator Attack() {

		currentState = State.Attacking;
		pathfinder.enabled = false;

		Vector3 originalPosition = transform.position;
		Vector3 dirToTarget = (target.position - transform.position).normalized;
		Vector3 attackPosition = target.position - dirToTarget * (myCollisionRadius);

		float attackSpeed = 3;
		float percent = 0;

		skinMaterial.color = Color.red;
		bool hasAppliedDamage = false;

		while (percent <= 1) {

			if (percent >= .5f && !hasAppliedDamage) {
				hasAppliedDamage = true;
				targetEntity.TakeDamage(damage);
			}

			percent += Time.deltaTime * attackSpeed;
			float interpolation = (-Mathf.Pow(percent,2) + percent) * 4;
			transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);

			yield return null;
		}

		skinMaterial.color = originalColour;
		currentState = State.Chasing;
		pathfinder.enabled = true;
	}

	IEnumerator UpdatePath() {
		float refreshRate = .25f;

		while (hasTarget) {
			if (currentState == State.Chasing) {
				Vector3 dirToTarget = (target.position - transform.position).normalized;
				Vector3 targetPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius + attackDistanceThreshold/2);
				if (!dead) {
					pathfinder.SetDestination (targetPosition);
				}
			}
			yield return new WaitForSeconds(refreshRate);
		}
	}
    */
}