using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;

[RequireComponent(typeof(NavMeshAgent))]
public class SpiderBoss : BaseEntity
{
	//Spider states
	public enum States
	{
		Init,
		Idle,
		Chase,
        Attack,
        Taunt, 
        Death
	}

    //Spider stats
    public float HardActivationDistance = 120;
    public float LooseActivationDistance = 200;
    public float AttackSpeed = 1;
    public float AttackDamage = 5;
    public float Health = 50;
    public float AttackRange = 24;
    public float Range = .1f;
    public float WalkSpeed = 9;
    public float RunSpeed = 15;
    public float SprintSpeed = 24;
    public float AttackCooldown = 0.5f;

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
    private bool _lockedOn = false;
    private bool _inAttackRange;
    private bool _isRunning;
    private bool _isMoving;

    private bool DEBUG = false;

   private void Awake()
	{
        if (DEBUG) Debug.Log("The spider wakes.");
        //base.Start();
        spawnLocation = this.gameObject.transform.position;

        //Initlize the pathfinder, collision range and animator 
        pathfinder = GetComponent<NavMeshAgent>();
        _collisionRange = GetComponent<CapsuleCollider>().radius;
        _animator = GetComponentInChildren<Animation>();      

        //Create the FSM controller
        fsm = StateMachine<States>.Initialize(this);
        fsm.ChangeState(States.Init);
    }

    private void Init_Enter()
    {
        if (DEBUG) Debug.Log("Spider state machine initilized.");
        fsm.ChangeState(States.Idle);
    }

    private void Taunt_Enter()
    {
        if (DEBUG) Debug.Log("Entered state: Taunt");
        fsm.ChangeState(States.Idle);
    }

    IEnumerator Attack_Enter()
    {
        if (DEBUG) Debug.Log("Entered state: Attack");
        pathfinder.enabled = false;

        _animator.Play("attack2", PlayMode.StopAll);
        target.GetComponent<BaseEntity>().Damage(AttackDamage, this.gameObject.transform);
        

        while (_animator.isPlaying)
        {
            yield return new WaitForSeconds(0.25f);
            if (DEBUG) Debug.Log("Waiting for attack animation to finish");
        }
        
        int attackCount = 0;

        if (attackCount == 1)
        {
            _isRunning = false;
            attackCount = 0;
        }

        yield return new WaitForSeconds(AttackCooldown);

        attackCount++;

        pathfinder.enabled = true;

        fsm.ChangeState(States.Chase);
    }

    IEnumerator Chase_Enter()
    {
        if (DEBUG) Debug.Log("Entered state: Chase");

        float refreshRate = 0.25f;
        _lockedOn = true;

        if (!_isMoving)
        {
            _animator.Play("run", PlayMode.StopAll);
            _isMoving = true;
        }

        //Find closet player
        Transform player1 = GameObject.FindGameObjectWithTag("Player").transform;
        Transform player2 = GameObject.FindGameObjectWithTag("Player2").transform;

        while (_lockedOn)
        {
            //If player 2 is closer to the spider, and is not dead, then chase them Otherwise, player 1 is closer.              
            if (Vector3.Distance(player1.position, this.gameObject.transform.position) >= Vector3.Distance(player2.position, this.gameObject.transform.position) && !player2.GetComponent<BaseEntity>().isDead)
            {
                if (DEBUG) Debug.Log("Targetting player 2");
                target = player2; 
            }            
            else if (Vector3.Distance(player2.position, this.gameObject.transform.position) >= Vector3.Distance(player1.position, this.gameObject.transform.position) && !player1.GetComponent<BaseEntity>().isDead)
            {
                if (DEBUG) Debug.Log("Targetting player 1");
                target = player1;
            }           
            else
            {
                fsm.ChangeState(States.Taunt);
            }

            if (DEBUG) Debug.Log("Chasing player:" + target.tag);

            //If they have moved outside the loose activation range, then taunt and stop chasing
            if (Vector3.Distance(target.position, this.gameObject.transform.position) > LooseActivationDistance)
            {
                if (DEBUG) Debug.Log("Lost player");
                _lockedOn = false;
                _isMoving = false;
                fsm.ChangeState(States.Taunt);
            }

            //If the target comes into attack range, stop chasing and enter the attack state
            if (Vector3.Distance(target.position, this.gameObject.transform.position) < AttackRange)
            {
                if (DEBUG) Debug.Log("In attack range");
                _isMoving = false;
                _lockedOn = false;
                fsm.ChangeState(States.Attack);
            }

            //Every so often sprint at the player
            pathfinder.speed = _isRunning ? SprintSpeed : RunSpeed;
            pathfinder.SetDestination(target.position);

            yield return new WaitForSeconds(refreshRate);
        }
    }

    IEnumerator Idle_Enter()
    {
        if (DEBUG) Debug.Log("Entered state: Idle");

        float refreshRate = 0.8f;        

        //Check to see if either player is within activation range
        while (!_lockedOn)
        {
            if (DEBUG) Debug.Log("Waiting for players.");
            pathfinder.SetDestination(spawnLocation);

            float player1distance = Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, this.gameObject.transform.position);
            float player2distance = Vector3.Distance(GameObject.FindGameObjectWithTag("Player2").transform.position, this.gameObject.transform.position);
            BaseEntity player1 = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<BaseEntity>();
            BaseEntity player2 = GameObject.FindGameObjectWithTag("Player2").transform.GetComponent<BaseEntity>();

            if (!player1.isDead && (player1distance < HardActivationDistance)|| !player2.isDead &&  (player2distance < HardActivationDistance))
            {
                if (DEBUG) Debug.Log("Player found.");
                _lockedOn = true;
            }
            yield return new WaitForSeconds(refreshRate);
        }
        fsm.ChangeState(States.Chase);
    }

    private void Death_Enter()
    {
        if (DEBUG) Debug.Log("Entered state: Death");
    }

    public override void Damage(float amount, Transform attacker)
    {
        if (true) Debug.Log("Spider damaged");
        base.Damage(amount, attacker);

        if (amount >= CurrentHealth)
        {
            if (true) Debug.Log("Spider killed");
            Killed();
        } else
        {
            _animator.Play("hit1", PlayMode.StopSameLayer);
        }
    }

    public override void Killed()
    {
        base.Killed();
        pathfinder.Stop();
        _animator.Play("death1", PlayMode.StopAll);
        fsm.ChangeState(States.Death);
    }
}

