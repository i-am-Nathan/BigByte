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
    Vector3 spawnLocation;

    private StateMachine<States> fsm;

    private float _nextAttackTime; 
    private float _collisionRange;
    private float _targetCollisionRange;
    private bool _lockedOn;
    private bool _inAttackRange;

    private void Awake()
	{
        //Initlize health stats and collider range
        base.Start();
        _collisionRange = GetComponent<CapsuleCollider>().radius;

        _animator = GetComponentInChildren<Animation>();

        //Initlize the pathfinder
        pathfinder = GetComponent<NavMeshAgent>();

        spawnLocation = this.gameObject.transform.position;

        //Create the FSM controller
        fsm = StateMachine<States>.Initialize(this, States.Idle);
	}

	private void Init_Enter()
	{
        Debug.Log("Spider state intilized");
        _animator.Play("taunt");
        fsm.ChangeState(States.Idle);
    }

	//We can return a coroutine, this is useful animations and the like
	private void Idle_Enter()
	{
        Debug.Log("Entered state: Idle");
        pathfinder.SetDestination(spawnLocation);
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
        StartCoroutine(AttackPlayer());        
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
                fsm.ChangeState(States.Idle);
            }

            if (Vector3.Distance(target.position, this.gameObject.transform.position) < AttackRange)
            {
                _lockedOn = false;
                fsm.ChangeState(States.Attack);
            }

            pathfinder.speed = RunSpeed;
            pathfinder.SetDestination(target.position);

            yield return new WaitForSeconds(refreshRate);
        }

        fsm.ChangeState(States.Idle);
    }

    IEnumerator AttackPlayer()
    {
        pathfinder.enabled = false;
        _inAttackRange = true;   
               
        while (_inAttackRange)
        {
            target.GetComponent<BaseEntity>().TakeDamage(AttackDamage, this.gameObject.transform);

            if (Vector3.Distance(target.position, this.gameObject.transform.position) > AttackRange)
            {
                _inAttackRange = false;
                fsm.ChangeState(States.Chase);
            }

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
