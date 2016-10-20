// file:	Assets\Scripts\Mobs\SpiderMob.cs
//
// summary:	Implements the spider mob class

using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;

/// <summary>   Controls the AI (using a FSM) of the smaller spider mobs. </summary>
///
/// <remarks>    . </remarks>

[RequireComponent(typeof(NavMeshAgent))]
public class SpiderMob : BaseEntity
{
    //Spider states

    /// <summary>   Values that represent states. </summary>
    ///
 

    public enum States
    {
        /// <summary>   An enum constant representing the init option. </summary>
        Init,
        /// <summary>   An enum constant representing the idle option. </summary>
        Idle,
        /// <summary>   An enum constant representing the chase option. </summary>
        Chase,
        /// <summary>   An enum constant representing the attack option. </summary>
        Attack,
        /// <summary>   An enum constant representing the taunt option. </summary>
        Taunt,
        /// <summary>   An enum constant representing the run option. </summary>
        Run,
        /// <summary>   An enum constant representing the death option. </summary>
        Death
    }

    //Spider stats
    /// <summary>   The hard activation distance. </summary>
    public float HardActivationDistance = 120;
    /// <summary>   The loose activation distance. </summary>
    public float LooseActivationDistance = 200;
    /// <summary>   The attack speed. </summary>
    public float AttackSpeed = 1;
    /// <summary>   The attack damage. </summary>
    public float AttackDamage = 5;
    /// <summary>   The health. </summary>
    public float Health = 50;
    /// <summary>   The attack range. </summary>
    public float AttackRange = 24;
    /// <summary>   The range. </summary>
    public float Range = .1f;
    /// <summary>   The walk speed. </summary>
    public float WalkSpeed = 9;
    /// <summary>   The run speed. </summary>
    public float RunSpeed = 15;
    /// <summary>   The sprint speed. </summary>
    public float SprintSpeed = 10;
    /// <summary>   The attack cooldown. </summary>
    public float AttackCooldown = 0.25f;

    //Target and navigation variables
    /// <summary>   The pathfinder. </summary>
    private NavMeshAgent pathfinder;
    /// <summary>   Target for the. </summary>
    private Transform target;
    /// <summary>   Target base entity. </summary>
    private BaseEntity targetBaseEntity;
    /// <summary>   The torch controller. </summary>
    public TorchFuelController TorchController;
    /// <summary>   The skin material. </summary>
    private Material skinMaterial;
    /// <summary>   The original colour. </summary>
    private Color originalColour;
    /// <summary>   The animator. </summary>
    private Animation _animator;
    /// <summary>   The spawn location. </summary>
    private Vector3 spawnLocation;

    /// <summary>   The fsm. </summary>
    private StateMachine<States> fsm;

    /// <summary>   The next attack time. </summary>
    private float _nextAttackTime;
    /// <summary>   The collision range. </summary>
    private float _collisionRange;
    /// <summary>   Target collision range. </summary>
    private float _targetCollisionRange;
    /// <summary>   True to enable, false to disable the locked. </summary>
    private bool _lockedOn = false;
    /// <summary>   True to in attack range. </summary>
    private bool _inAttackRange;
    /// <summary>   True if this object is running. </summary>
    private bool _isRunning;
    /// <summary>   True if this object is moving. </summary>
    private bool _isMoving;
    /// <summary>   True to in torch light. </summary>
    private bool _inTorchLight;
    /// <summary>   True to running away. </summary>
    private bool _runningAway = false;

    /// <summary>   True to debug. </summary>
    private bool DEBUG = false;
    /// <summary>   The hit. </summary>
    private object hit;

    /// <summary>   Initilized montser location, pathfinding, animation and the AI FSM. </summary>
    ///
 

    private void Awake()
    {
        if (DEBUG) Debug.Log("The spider wakes.");
        //base.Start();
        spawnLocation = this.gameObject.transform.position;

        //Initlize the pathfinder, collision range and animator 
        pathfinder = GetComponent<NavMeshAgent>();
        _collisionRange = GetComponent<CapsuleCollider>().radius;
        _animator = GetComponentInChildren<Animation>();
        TorchController = GameObject.FindGameObjectWithTag("TorchFuelController").transform.GetComponent<TorchFuelController>();

        //Create the FSM controller
        fsm = StateMachine<States>.Initialize(this);
        fsm.ChangeState(States.Init);
        pathfinder.enabled = true;
    }

    /// <summary>   Mock up. </summary>
    ///
 

    public void MockUp()
    {
        base.Start();
    }

    /// <summary>
    /// Initial start state for the FSM. Needed for the monster fsm libarary to work.
    /// </summary>
    ///
 
    ///
    /// <returns>   An IEnumerator. </returns>

    IEnumerator Init_Enter()
    {
        if (DEBUG) Debug.Log("Spider state machine initilized.");
        pathfinder.enabled = false;
        yield return new WaitForSeconds(4f);
        pathfinder.enabled = true;
        fsm.ChangeState(States.Idle);
    }

    /// <summary>
    /// Entry method for the taunt state. This plays the taunt animation and then transitions back to
    /// idle.
    /// </summary>
    ///
 

    private void Taunt_Enter()
    {
        if (DEBUG) Debug.Log("Entered state: Taunt");
        fsm.ChangeState(States.Idle);
    }

    /// <summary>
    /// Entry method for the attack state. Plays the attack animation once, and deals damage once,
    /// before transitioning back to the chase state.
    /// </summary>
    ///
 
    ///
    /// <returns>   An IEnumerator. </returns>

    IEnumerator Attack_Enter()
    {
        if (!isDead)
        {
            if (DEBUG) Debug.Log("Entered state: Attack");

            //Disable pathfinding to prevent the spider moving during the attack animation
            pathfinder.enabled = false;

            //Play the attack animation and deal damage to the target entitiy
            _animator.Play("attack2", PlayMode.StopAll);

            if (target != null)
            {
                target.GetComponent<BaseEntity>().Damage(AttackDamage, this.gameObject.transform);
            }

            //Wait for the animation to finish before continuing back to the chase state
            while (_animator.isPlaying)
            {
                yield return new WaitForSeconds(0.25f);
                if (DEBUG) Debug.Log("Waiting for attack animation to finish");
            }
                       
            pathfinder.enabled = true;
            fsm.ChangeState(States.Chase);
        }       
    }

    /// <summary>
    /// Entry method for the chase state. Chooses the closets player and moves towards them. Breaks
    /// if the player leaves the spiders alert area, or comes into attack range.
    /// </summary>
    ///
 
    ///
    /// <returns>   An IEnumerator. </returns>

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

        while (_lockedOn && !isDead)
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
            if (IsInLight(this.gameObject.transform))
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
            if (IsInLight(target.transform))
            {
                if (DEBUG) Debug.Log("Player In torch range");
                _isMoving = false;
                _lockedOn = false;
                fsm.ChangeState(States.Taunt);
                break;
            }

            pathfinder.speed = _isRunning ? SprintSpeed : RunSpeed;
            pathfinder.acceleration = 16f;
            pathfinder.angularSpeed = 900f;
            pathfinder.SetDestination(target.position);

            if (DEBUG) Debug.Log("Chasing player:" + target.tag);
            yield return new WaitForSeconds(refreshRate);
        }
    }

    /// <summary>   Query if 'transform' is in light. </summary>
    ///
 
    ///
    /// <param name="transform">    The transform. </param>
    ///
    /// <returns>   True if in light, false if not. </returns>

    bool IsInLight(Transform transform)
    {
        if (DEBUG) Debug.Log("Checking if in a light source");
        if (TorchController.IsInTorchRange(transform.position.x, transform.position.z))
        {
            if (DEBUG) Debug.Log("Transform is in torch light");
            _runningFromTorch = true;
            _runningFromCandle = false;
            return true;
        }

        //Then check for candle sources.
        var torchSources = GameObject.FindGameObjectsWithTag("LightSource");
        if (torchSources.Length != 0)
        {
            foreach (var torchSource in torchSources)
            {
                var CandleLight = torchSource.transform.GetComponent<CandleLight>();
                if (CandleLight.isActive())
                {
                    if (DEBUG) Debug.Log("Checking triggered candle light");
                    if (DEBUG) Debug.Log("Distance: " + Vector3.Distance(transform.position, CandleLight.transform.position));
                    if (DEBUG) Debug.Log("Radius: " + CandleLight.Radius);
                    if (Vector3.Distance(transform.position, CandleLight.transform.position) < CandleLight.Radius)
                    {
                        if (DEBUG) Debug.Log("Transform is in the candle light");
                        _runningFromTorch = false;
                        _runningFromCandle = true;
                        _candleRunningFrom = torchSource;
                        return true;
                    }
                }
            }
        }        
        return false;  
    }

    /// <summary>   True to running from torch. </summary>
    private bool _runningFromTorch;
    /// <summary>   True to running from candle. </summary>
    private bool _runningFromCandle;
    /// <summary>   The candle running from. </summary>
    GameObject _candleRunningFrom;

    /// <summary>
    /// Entry method for the chase state. Chooses the closets player and moves towards them. Breaks
    /// if the player leaves the spiders alert area, or comes into attack range.
    /// </summary>
    ///
 
    ///
    /// <returns>   An IEnumerator. </returns>

    IEnumerator Run_Enter()
    {
        if (DEBUG) Debug.Log("Entered state: Run");

        float refreshRate = 0.25f;
        _inTorchLight = true;

        if (!_isMoving)
        {
            _animator.Play("run", PlayMode.StopAll);
            _isMoving = true;
        }
               
        while (_inTorchLight)
        {            

            //If the spider has run out of the torch light, transition back to idle
            if (!IsInLight(this.gameObject.transform) && _inTorchLight)
            {
                if (DEBUG) Debug.Log("Escaped torchlight");
                _inTorchLight = false;
                _isMoving = false;
                fsm.ChangeState(States.Idle);
            } else
            {
                //Determine which way the spider should run
                Vector3 dest;

                if (_runningFromTorch)
                {
                    Vector3 torchDirection = this.gameObject.transform.position - TorchController.GetTorchPosition();
                    dest = this.gameObject.transform.position + torchDirection;
                } else
                {
                    Vector3 torchDirection = this.gameObject.transform.position - _candleRunningFrom.transform.position;
                    dest = this.gameObject.transform.position + torchDirection;
                }                

                pathfinder.speed = SprintSpeed;
                pathfinder.acceleration = 15;
                pathfinder.angularSpeed = 900f;
                pathfinder.SetDestination(new Vector3(dest.x, 0, dest.z));
                _runningAway = true;

                if (DEBUG) Debug.Log("Spider running to: " + dest);
            }          
            yield return new WaitForSeconds(refreshRate);
        }
    }

    /// <summary>
    /// Entry state for the idle state. Waits in place and constantly checks to see if any players
    /// have entered its alert area. If a player enters the area if transitions to the chase state to
    /// chase them down.
    /// </summary>
    ///
 
    ///
    /// <returns>   An IEnumerator. </returns>

    IEnumerator Idle_Enter()
    {
        if (DEBUG) Debug.Log("Entered state: Idle");
        //_animator.Play("idle", PlayMode.StopAll);

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
            if (IsInLight(this.gameObject.transform) && !_lockedOn)
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

    /// <summary>   Death enter. </summary>
    ///
 

    private void Death_Enter()
    {
        if (DEBUG) Debug.Log("Entered state: Death");
    }

    /// <summary>   Damages. </summary>
    ///
 
    ///
    /// <param name="amount">   The damage. </param>
    /// <param name="attacker"> The attacker. </param>

    public override void Damage(float amount, Transform attacker)
    {
        if (true) Debug.Log("Spider damaged");
        if (isDead) return;
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

    /// <summary>   Killed this object. </summary>
    ///
 

    public override void Killed()
    {
        base.Killed();

        try
        {
            GameObject go = GameObject.FindGameObjectWithTag("Game Data");
            GameData _gameDataScript = (GameData)go.GetComponent(typeof(GameData));

            _gameDataScript.UpdateMonstersKilled();
        }
        catch { }

        try
        {
            pathfinder.Stop();
            _animator.Play("death2", PlayMode.StopAll);
            fsm.ChangeState(States.Death);
        } catch { }
       
    }
}

