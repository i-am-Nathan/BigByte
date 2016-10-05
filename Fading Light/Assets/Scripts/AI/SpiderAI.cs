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
        Attack,
        Taunt
	}

    //Spider stats
    public float HardActivationDistance = 70;
    public float LooseActivationDistance = 100;
    public float AttackSpeed = 1;
    public float AttackDamage = 10;
    public float Health = 50;
    public float AttackRange = 16;
    public float Range = .1f;
    public float WalkSpeed = 9;
    public float RunSpeed = 15;
    public float SprintSpeed = 24;

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
    private bool _isRunning;
    private bool _isMoving;

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
        fsm.ChangeState(States.Idle);
    }

	//We can return a coroutine, this is useful animations and the like
	private void Idle_Enter()
	{
        Debug.Log("Entered state: Idle");
        //bool a = _animator.Play("taunt", PlayMode.StopAll);

        //Use coroutine to check when players enter activation range
        StartCoroutine(CheckForPlayers());
    }

	private void Chase_Enter()
	{
        Debug.Log("Entered state: Chase");

        if (!_isMoving)
        {
            _animator.Play("run", PlayMode.StopAll);
            _isMoving = true;
        }

        //Use coroutine to check when players enter activation range       
        StartCoroutine(UpdatePath());
    }

    private void Taunt_Enter()
    {
        Debug.Log("Entered state: Taunt");

        /*//Use coroutine to check when players enter activation range
        pathfinder.enabled = false;
        _animator.Play("taunt");     
        
        while (_animator.isPlaying)
        {

        }

        pathfinder.enabled = enabled;*/

        fsm.ChangeState(States.Idle);
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
            //Debug.Log("Checking for players.." + Vector3.Distance(player1.position, this.gameObject.transform.position));

            pathfinder.SetDestination(spawnLocation);

            if (Vector3.Distance(player1.position, this.gameObject.transform.position) < HardActivationDistance ||
            Vector3.Distance(player2.position, this.gameObject.transform.position) < HardActivationDistance)
            {
                //Debug.Log("Player found.");
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
            if (Vector3.Distance(player1.position, this.gameObject.transform.position) > Vector3.Distance(player2.position, this.gameObject.transform.position))
            {
                target = player2;
                _lockedOn = true;
                //Debug.Log("Locked onto Player 2");
            }
            else
            {
                target = player1;
                _lockedOn = true;
                //Debug.Log("Locked onto Player 1");
            }

            //If they have moved outside the loose activation range, then taunt and stop chasing
            if (Vector3.Distance(target.position, this.gameObject.transform.position) > LooseActivationDistance)
            {
                Debug.Log("Lost player");
                _lockedOn = false;
                _isMoving = false;
                fsm.ChangeState(States.Taunt);
            }

            //Debug.Log("Distance: " + Vector3.Distance(target.position, this.gameObject.transform.position) + " Attack Range: " + AttackRange);

            if (Vector3.Distance(target.position, this.gameObject.transform.position) < AttackRange)
            {
                Debug.Log("In attack range");
                _lockedOn = false;
                _isMoving = false;
                fsm.ChangeState(States.Attack);
            }

            //Every so often sprint at the player
            if (_isRunning)
            {
                pathfinder.speed = SprintSpeed;
            } else
            {
                //Random chance of it starting to run

                pathfinder.speed = RunSpeed;
            }
            
            pathfinder.SetDestination(target.position);

            yield return new WaitForSeconds(refreshRate);
        }

        fsm.ChangeState(States.Idle);
    }

    IEnumerator AttackPlayer()
    {
        //pathfinder.enabled = false;
        //pathfinder.Stop();
        _inAttackRange = true;
        int attackCount = 0;
               
        while (_inAttackRange)
        {
            //Debug.Log("Attacking player");
            //_animator["attack2"].speed = AttackSpeed;
            _animator.Play("attack2");
            BaseEntity targetBase = target.GetComponent<BaseEntity>();
            targetBase.Damage(AttackDamage, this.gameObject.transform);

            if (attackCount == 1)
            {
                _isRunning = false;
                attackCount = 0;
            }

            if (Vector3.Distance(target.position, this.gameObject.transform.position) > AttackRange)
            {
                _inAttackRange = false;
            }

            attackCount++;
            yield return new WaitForSeconds(AttackSpeed);
        }
        
        //pathfinder.enabled = true;
        fsm.ChangeState(States.Chase);
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
