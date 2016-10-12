using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;

/// <summary>
/// Controls the AI (using a FSM) of the smaller spider mobs
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class SpiderMob : BaseEntity
{
    //Spider states
    public enum States
    {
        Init,
        Idle,
        Chase,
        Attack,
        Taunt,
        Run,
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
    public float SprintSpeed = 10;
    public float AttackCooldown = 0.25f;

    //Target and navigation variables
    private NavMeshAgent pathfinder;
    private Transform target;
    private BaseEntity targetBaseEntity;
    public TorchFuelController TorchController;
    private Material skinMaterial;
    private Color originalColour;
    private Animation _animator;
    private Vector3 spawnLocation;

    private StateMachine<States> fsm;

    private float _nextAttackTime;
    private float _collisionRange;
    private float _targetCollisionRange;
    private bool _lockedOn = false;
    private bool _inAttackRange;
    private bool _isRunning;
    private bool _isMoving;
    private bool _inTorchLight;
    private bool _runningAway = false;

    private bool DEBUG = false;

    /// <summary>
    /// Initilized montser location, pathfinding, animation and the AI FSM
    /// </summary>
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

    public void MockUp()
    {
        base.Start();
    }

    /// <summary>
    /// Initial start state for the FSM. Needed for the monster fsm libarary to work.
    /// </summary>
    private void Init_Enter()
    {
        if (DEBUG) Debug.Log("Spider state machine initilized.");
        fsm.ChangeState(States.Idle);
    }

    /// <summary>
    /// Entry method for the taunt state. This plays the taunt animation and then transitions back to idle
    /// </summary>
    private void Taunt_Enter()
    {
        if (DEBUG) Debug.Log("Entered state: Taunt");
        fsm.ChangeState(States.Idle);
    }

    /// <summary>
    /// Entry method for the attack state. Plays the attack animation once, and deals damage once, before transitioning back to the chase state.
    /// </summary>
    /// <returns></returns>
    IEnumerator Attack_Enter()
    {
        if (DEBUG) Debug.Log("Entered state: Attack");

        //Disable pathfinding to prevent the spider moving during the attack animation
        pathfinder.enabled = false;

        //Play the attack animation and deal damage to the target entitiy
        _animator.Play("attack2", PlayMode.StopAll);
        target.GetComponent<BaseEntity>().Damage(AttackDamage, this.gameObject.transform);

        //Wait for the animation to finish before continuing back to the chase state
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
        
        attackCount++;

        pathfinder.enabled = true;

        fsm.ChangeState(States.Chase);
    }

    /// <summary>
    /// Entry method for the chase state. Chooses the closets player and moves towards them. Breaks if the player leaves the 
    /// spiders alert area, or comes into attack range.
    /// </summary>
    /// <returns></returns>
    IEnumerator Chase_Enter()
    {
        if (DEBUG) Debug.Log("Entered state: Chase");

        float refreshRate = 0.35f;
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

            //Check if the torch has moved over the spider. If so then transition to the run state
            if (TorchController.IsInTorchRange(this.gameObject.transform.position.x, this.gameObject.transform.position.z))
            {
                if (DEBUG) Debug.Log("Spider inside torch");
                //if (DEBUG) Debug.Log(this.gameObject.transform.position);
                //if (DEBUG) Debug.Log(TorchController.GetTorchPosition());
                _lockedOn = true;
                fsm.ChangeState(States.Run);
                break;
            }

            //If they have moved outside the loose activation range, then taunt and stop chasing
            if (Vector3.Distance(target.position, this.gameObject.transform.position) > LooseActivationDistance)
            {
                if (DEBUG) Debug.Log("Lost player");
                _lockedOn = false;
                _isMoving = false;
                fsm.ChangeState(States.Taunt);
                break;
            }

            //If the target comes into attack range, stop chasing and enter the attack state
            if (Vector3.Distance(target.position, this.gameObject.transform.position) < AttackRange)
            {
                if (DEBUG) Debug.Log("Player in attack range");
                _isMoving = false;
                _lockedOn = false;
                fsm.ChangeState(States.Attack);
                break;
            }                      
            
            //Check if the player is inside the torch, if so move along outside of radius
            if (TorchController.IsInTorchRange(target.transform.position.x, target.transform.position.z))
            {
                if (DEBUG) Debug.Log("Player In torch range");
                _isMoving = false;
                _lockedOn = false;
                fsm.ChangeState(States.Taunt);
                break;
            }

            pathfinder.speed = _isRunning ? SprintSpeed : RunSpeed;
            pathfinder.SetDestination(target.position);

            if (DEBUG) Debug.Log("Chasing player:" + target.tag);
            yield return new WaitForSeconds(refreshRate);
        }
    }

    /// <summary>
    /// Entry method for the chase state. Chooses the closets player and moves towards them. Breaks if the player leaves the 
    /// spiders alert area, or comes into attack range.
    /// </summary>
    /// <returns></returns>
    IEnumerator Run_Enter()
    {
        if (DEBUG) Debug.Log("Entered state: Run");

        float refreshRate = 0.25f;
        float fleeDistance = 10f;
        _inTorchLight = true;

        if (!_isMoving)
        {
            _animator.Play("run", PlayMode.StopAll);
            _isMoving = true;
        }
               
        while (_inTorchLight)
        {            

            //If the spider has run out of the torch light, transition back to idle
            if (!TorchController.IsInTorchRange(this.gameObject.transform.position.x, this.gameObject.transform.position.z) && _inTorchLight)
            {
                if (DEBUG) Debug.Log("Escaped torchlight");
                _inTorchLight = false;
                _isMoving = false;
                fsm.ChangeState(States.Idle);
            } else
            {
                if (DEBUG) Debug.Log("Spider running");
                //if (DEBUG) Debug.Log(this.gameObject.transform.position);
                //if (DEBUG) Debug.Log(TorchController.GetTorchPosition());

                //Determine which way the spider should run
                Vector3 torchDirection = this.gameObject.transform.position - TorchController.GetTorchPosition();
                Vector3 dest = this.gameObject.transform.position + torchDirection;

                //TODO: CHECK FOR IF DEST POSITION IS IN THE NAVMESH
                                
                
                //if (!_runningAway)
                //{
                    //Set the spider to run away as fast as possible
                    pathfinder.speed = SprintSpeed;
                    pathfinder.acceleration = 15;
                    pathfinder.SetDestination(dest);
                    _runningAway = true;
                /*} else
                {
                    //Check to see if we have reached the destination if we were running
                    if (!pathfinder.pathPending)
                    {
                        if (pathfinder.remainingDistance <= pathfinder.stoppingDistance)
                        {
                            if (!pathfinder.hasPath || pathfinder.velocity.sqrMagnitude == 0f)
                            {
                                _runningAway = false;
                            }
                        }
                    }
                }*/
            }          
            yield return new WaitForSeconds(refreshRate);
        }
    }

    /// <summary>
    /// Entry state for the idle state. Waits in place and constantly checks to see if any players have entered its alert area. If a player enters the area
    /// if transitions to the chase state to chase them down.
    /// </summary>
    /// <returns></returns>
    IEnumerator Idle_Enter()
    {
        if (DEBUG) Debug.Log("Entered state: Idle");

        float refreshRate = 0.25f;
        _lockedOn = false;

        //Check to see if either player is within activation range
        while (!_lockedOn)
        {
            if (DEBUG) Debug.Log("Waiting for players.");
            //pathfinder.SetDestination(spawnLocation);

            float player1distance = Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, this.gameObject.transform.position);
            float player2distance = Vector3.Distance(GameObject.FindGameObjectWithTag("Player2").transform.position, this.gameObject.transform.position);
            BaseEntity player1 = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<BaseEntity>();
            BaseEntity player2 = GameObject.FindGameObjectWithTag("Player2").transform.GetComponent<BaseEntity>();

            //if (DEBUG) Debug.Log(this.gameObject.transform.position);
            //if (DEBUG) Debug.Log(TorchController.GetTorchPosition());

            //Debug.Log(Vector3.Distance(this.gameObject.transform.position, TorchController.GetTorchPosition()));

            //Check if the torch has moved over the spider. If so then transition to the run state
            if (TorchController.IsInTorchRange(this.gameObject.transform.position.x, this.gameObject.transform.position.z) && !_lockedOn)
            {
                if (DEBUG) Debug.Log("Spider inside torch");
                //if (DEBUG) Debug.Log(this.gameObject.transform.position);
                //if (DEBUG) Debug.Log(TorchController.GetTorchPosition());
                _lockedOn = true;
                fsm.ChangeState(States.Run);
                break;
            }
                        
            if (!player1.isDead && (player1distance < HardActivationDistance) || !player2.isDead && (player2distance < HardActivationDistance) && !_lockedOn)
            {
                if (DEBUG) Debug.Log("Player found.");
                _lockedOn = true;
                fsm.ChangeState(States.Chase);
                break;
            }

            yield return new WaitForSeconds(refreshRate);
        }        
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
        }
        else
        {
            try
            {
                _animator.Play("hit1", PlayMode.StopSameLayer);
            } catch { }            
        }
    }

    public override void Killed()
    {
        base.Killed();

        try
        {
            pathfinder.Stop();
            _animator.Play("death2", PlayMode.StopAll);
            fsm.ChangeState(States.Death);
        } catch { }
       
    }
}

