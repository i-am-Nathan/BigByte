using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;

[RequireComponent(typeof(NavMeshAgent))]
public class SpiderAI : BaseEntity
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

        _inAttackRange = true;
        int attackCount = 0;

        //pathfinder.enabled = false;

        while (_inAttackRange)
        {
           if (attackCount == 1)
            {
                _isRunning = false;
                attackCount = 0;
            }

            //If the target moves out of attack range or dies, then stop attacking and go back to chasing the other player
            if (Vector3.Distance(target.position, this.gameObject.transform.position) > AttackRange || !target.GetComponent<BaseEntity>().isDead)
            {
                _inAttackRange = false;
            }
            
            _animator.Play("attack2");
            target.GetComponent<BaseEntity>().Damage(AttackDamage, this.gameObject.transform);

            attackCount++;
            yield return new WaitForSeconds(AttackSpeed);
        }

        //pathfinder.enabled = true;

        fsm.ChangeState(States.Chase);
    }

    IEnumerator Chase_Enter()
    {
        if (DEBUG) Debug.Log("Entered state: Chase");

        float refreshRate = .25f;

        if (!_isMoving)
        {
            _animator.Play("run", PlayMode.StopAll);
            _isMoving = true;
        }

        //Find closet player
        Transform player1 = GameObject.FindGameObjectWithTag("Player").transform;
        Transform player2 = GameObject.FindGameObjectWithTag("Player2").transform;
        bool isCloser = (Vector3.Distance(player1.position, this.gameObject.transform.position) > Vector3.Distance(player2.position, this.gameObject.transform.position));

        while (_lockedOn)
        {                       
            if (!player2.GetComponent<BaseEntity>().isDead && isCloser)
            {
                //If player 2 is closer to the spider, and is not dead, then chase them
                target = player2; 
            }            
            else if (!player1.GetComponent<BaseEntity>().isDead)
            {
                //Otherwise, player 1 is closer. Chase them if they are not dead.
                target = player1;
            }           
            else
            {
                //Otherwise both players are dead. Celebrate!!!
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
                _lockedOn = false;
                _isMoving = false;
                fsm.ChangeState(States.Attack);
            }

            //Every so often sprint at the player
            pathfinder.speed = _isRunning ? SprintSpeed : RunSpeed;
            pathfinder.SetDestination(target.position);

            yield return new WaitForSeconds(refreshRate);
        }
        fsm.ChangeState(States.Idle);
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

    public override void Attacked(float damage, Transform attacker)
    {
        //If damage is to kill the spider - play animations/sounds
        if (damage >= CurrentHealth)
        {
            base.Killed();

        }
        base.Attacked(damage, attacker);
    }
}
