using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;

[RequireComponent(typeof(NavMeshAgent))]
public class SpiderAI : BaseEntity
{
	//Spider stats
	public enum States
	{
		Init,
		Idle,
		Chase,
        Attack
	}

    //Spider stats
    public float HardActivationDistance = .5f;
    public float LooseActivationDistance = .10f;
    public float AttackSpeed = 1;
    public float AttackDamage = 1;
    public float Health = 50;
    public float AttackRange = .1f;
    public float Range = .1f;
    public float WalkSpeed = .1f;
    public float RunSpeed = .1f;

    //Target and navigation variables
    NavMeshAgent pathfinder;
    Transform target;
    BaseEntity targetBaseEntity;
    Material skinMaterial;
    Color originalColour;
    Animation _animator;

    private StateMachine<States> fsm;

    private float _nextAttackTime; 
    private float _collisionRange;
    private float _targetCollisionRange;
    private bool _lockedOn;

    private void Awake()
	{
        //Initlize health stats and collider range
        base.Start();
        _collisionRange = GetComponent<CapsuleCollider>().radius;

        _animator = GetComponentInChildren<Animator>();

        //Initlize the pathfinder
        pathfinder = GetComponent<NavMeshAgent>();

        //Create the FSM controller
        fsm = StateMachine<States>.Initialize(this, States.Idle);
	}

	private void Init_Enter()
	{
        Debug.Log("Spider state intilized");
        fsm.ChangeState(States.Idle);
    }

	//We can return a coroutine, this is useful animations and the like
	private void Idle_Enter()
	{
        Debug.Log("Entered state: Idle");
        //Use coroutine to check when players enter activation range
        StartCoroutine(CheckForPlayers());
    }

	private void Chase_Enter()
	{
        Debug.Log("Entered state: Chase");
        //Use coroutine to check when players enter activation range
        StartCoroutine(UpdatePath());
    }

    private void Attack_Enter()
    {
        Debug.Log("Entered state: Attack");
        
        //Use coroutine to check when players enter activation range
        //StartCoroutine(AttackPlayer());        
    }

    /// <summary>
	/// Method with updates the pathfinding towards the player
	/// </summary>
	/// <returns>The path.</returns>
	IEnumerator CheckForPlayers()
    {
        float refreshRate = 0.8f;        

        Transform player1 = GameObject.FindGameObjectWithTag("Player").transform;
        Transform player2 = GameObject.FindGameObjectWithTag("Player2").transform;

        //Check to see if either player is within activation range
        while (!_lockedOn)
        {
            Debug.Log("Checking for players.." + Vector3.Distance(player1.position, this.gameObject.transform.position));
            if (Vector3.Distance(player1.position, this.gameObject.transform.position) < HardActivationDistance ||
            Vector3.Distance(player2.position, this.gameObject.transform.position) < HardActivationDistance)
            {
                Debug.Log("Player found.");
                //If they are then change to attack state
                _lockedOn = true;
            }
            yield return new WaitForSeconds(refreshRate);
        }
        fsm.ChangeState(States.Chase);
    }

    /// <summary>
    /// Method with updates the pathfinding towards the player
    /// </summary>
    /// <returns>The path.</returns>
    IEnumerator UpdatePath() {
		float refreshRate = .25f;

        //Find closet player
        Transform player1 = GameObject.FindGameObjectWithTag("Player").transform;
        Transform player2 = GameObject.FindGameObjectWithTag("Player2").transform;

        while (_lockedOn)
        {
            //Find closest player to attack
            if (Vector3.Distance(player1.position, this.gameObject.transform.position) > Vector3.Distance(player1.position, this.gameObject.transform.position))
            {
                target = player1;
                _lockedOn = true;
                Debug.Log("Locked onto Player 1");
            }
            else
            {
                target = player2;
                _lockedOn = true;
                Debug.Log("Locked onto Player 2");
            }

            //If they have moved outside the loose activation range, then stop chasing
            if (Vector3.Distance(target.position, this.gameObject.transform.position) > LooseActivationDistance)
            {
                _lockedOn = false;                
            }

            pathfinder.SetDestination(target.position);
            //If the characters are in attack range, switch to the attack state
            //fsm.ChangeState(States.Attack);

            yield return new WaitForSeconds(refreshRate);
        }
        fsm.ChangeState(States.Idle);
    }

    IEnumerator Attack()
    {
        pathfinder.enabled = false;

        Vector3 originalPosition = transform.position;
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        Vector3 attackPosition = target.position - dirToTarget * (_collisionRange);

        float attackSpeed = 3;
        float percent = 0;

        skinMaterial.color = Color.red;
        bool hasAppliedDamage = false;

        while (percent <= 1)
        {

            if (percent >= .5f && !hasAppliedDamage)
            {
                target.GetComponent<BaseEntity>().TakeDamage(AttackDamage, this.gameObject.transform);
            }

            percent += Time.deltaTime * attackSpeed;
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);

            yield return null;
        }
        
        pathfinder.enabled = true;
    }

    public override void Attacked(float damage, Transform attacker)
    {
        //If damage is to kill the spider - play animations/sounds
        if (damage >= CurrentHealth)
        {
            
        }
        base.Attacked(damage, attacker);
    }
}
