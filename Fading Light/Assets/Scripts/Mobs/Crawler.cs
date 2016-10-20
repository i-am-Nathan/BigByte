﻿// file:	Assets\Scripts\Mobs\Crawler.cs
//
// summary:	Implements the crawler class

using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;
using UnityEngine.UI;

/// <summary>
/// Controls the AI (using FSM) of the large crawler bosses (e.i. the one found in the tutorial
/// level)
/// </summary>
///
/// <remarks>    . </remarks>

[RequireComponent(typeof(NavMeshAgent))]
public class Crawler : BaseEntity
{
	//crawler states

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
        /// <summary>   An enum constant representing the death option. </summary>
        Death
	}

    //crawler stats
    /// <summary>   The hard activation distance. </summary>
    public float HardActivationDistance = 50;
    /// <summary>   The loose activation distance. </summary>
    public float LooseActivationDistance = 120;
    /// <summary>   The attack speed. </summary>
    public float AttackSpeed = 1;
    /// <summary>   The attack damage. </summary>
    public float AttackDamage = 5;
    /// <summary>   The health. </summary>
    public float Health = 5000;
    /// <summary>   The attack range. </summary>
    public float AttackRange = 24;
    /// <summary>   The range. </summary>
    public float Range = .1f;
    /// <summary>   The walk speed. </summary>
    public float WalkSpeed = 9f;
    /// <summary>   The run speed. </summary>
    public float RunSpeed = 15f;
    /// <summary>   The sprint speed. </summary>
    public float SprintSpeed = 35f;
    /// <summary>   The attack cooldown. </summary>
    public float AttackCooldown = 0.5f;


    /// <summary>   Reference to the UI's health circle. </summary>
    public Image healthCircle;

    //Target and navigation variables
    /// <summary>   The pathfinder. </summary>
    NavMeshAgent pathfinder;
    /// <summary>   Target for the. </summary>
    Transform target;
    /// <summary>   Target base entity. </summary>
    BaseEntity targetBaseEntity;
    /// <summary>   The skin material. </summary>
    Material skinMaterial;
    /// <summary>   The original colour. </summary>
    Color originalColour;
    /// <summary>   The animator. </summary>
    Animation _animator;
    /// <summary>   The spawn location. </summary>
    Vector3 spawnLocation;

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
    /// <summary>   True if this object is sprinting. </summary>
    private bool _isSprinting;
    /// <summary>   True if this object is moving. </summary>
    private bool _isMoving;
    /// <summary>   Number of walks. </summary>
    private int _walkCount;

    /// <summary>   True to debug. </summary>
    private bool DEBUG = false;

    /// <summary>   Manager for achievement. </summary>
	private AchievementManager _achievementManager;

    /// <summary>   Initilized montser location, pathfinding, animation and the AI FSM. </summary>
    ///
 

    private void Awake()
	{
        if (DEBUG) Debug.Log("The crawler wakes.");
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

    /// <summary>   Mock up. </summary>
    ///
 

    public void MockUp()
    {
        base.Start();
    }

    /// <summary>   Starts this object. </summary>
    ///
 

    private void Start(){
		_achievementManager = (AchievementManager)GameObject.FindGameObjectWithTag ("AchievementManager").GetComponent(typeof(AchievementManager));
        //healthCircle.enabled = false;
        CurrentHealth = Health;
	}

    /// <summary>
    /// Initial start state for the FSM. Needed for the monster fsm libarary to work.
    /// </summary>
    ///
 

    private void Init_Enter()
    {
        if (DEBUG) Debug.Log("crawler state machine initilized.");
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
        if (DEBUG) Debug.Log("Entered state: Attack");
        pathfinder.enabled = false;

        _animator.Play("attack", PlayMode.StopAll);
        target.GetComponent<BaseEntity>().Damage(AttackDamage, this.gameObject.transform);
        

        while (_animator.isPlaying)
        {
            yield return new WaitForSeconds(0.25f);
            if (DEBUG) Debug.Log("Waiting for attack animation to finish");
        }        
        
        if (_isSprinting) _isSprinting = false;

        yield return new WaitForSeconds(AttackCooldown);
        

        pathfinder.enabled = true;

        fsm.ChangeState(States.Chase);
    }

    /// <summary>
    /// Entry method for the chase state. Chooses the closets player and moves towards them. Breaks
    /// if the player leaves the crawlers alert area, or comes into attack range.
    /// </summary>
    ///
 
    ///
    /// <returns>   An IEnumerator. </returns>

    IEnumerator Chase_Enter()
    {
        if (DEBUG) Debug.Log("Entered state: Chase");

        float refreshRate = !_isSprinting ? 0.3f : 0.05f;
        _lockedOn = true;

        //Find closet player
        Transform player1 = GameObject.FindGameObjectWithTag("Player").transform;
        Transform player2 = GameObject.FindGameObjectWithTag("Player2").transform;

        while (_lockedOn)
        {

            if (!_isMoving)
            {
                //_animator["run"].speed = _isSprinting ? 1.5f : 1.0f;
                _animator.Play("crawl", PlayMode.StopAll);
                _isMoving = true;
            }

            //If player 2 is closer to the crawler, and is not dead, then chase them Otherwise, player 1 is closer.              
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

            //Set the speed of the pathfinder (either running or sprinting) and the target positions
            pathfinder.speed = _isSprinting ? SprintSpeed : RunSpeed;
            pathfinder.SetDestination(target.position);
            Debug.Log(pathfinder.speed);

            //Every so often sprint at the player
            if (_walkCount > 12)
            {
                if (true) Debug.Log("crawler has started sprinting!");
                _isSprinting = true;
                _isMoving = false;
                _walkCount = -5;
            }
            _walkCount++;
            
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
        float refreshRate = 0.8f;        

        //Check to see if either player is within activation range
        while (!_lockedOn)
        {
            if (DEBUG) Debug.Log("Waiting for players.");

            //Move the crawler back to its "lair" if there are no targets to chase/attack
            pathfinder.SetDestination(spawnLocation);

            //Retrieve the distance to the two playesr and their entity objects
            float player1distance = Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, this.gameObject.transform.position);
            float player2distance = Vector3.Distance(GameObject.FindGameObjectWithTag("Player2").transform.position, this.gameObject.transform.position);
            BaseEntity player1 = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<BaseEntity>();
            BaseEntity player2 = GameObject.FindGameObjectWithTag("Player2").transform.GetComponent<BaseEntity>();

            //If there is a non-dead player inside the hard activiation distance, break the loop and chase them
            if (!player1.isDead && (player1distance < HardActivationDistance)|| !player2.isDead &&  (player2distance < HardActivationDistance))
            {
                if (DEBUG) Debug.Log("Player found.");
                _lockedOn = true;
            }
            yield return new WaitForSeconds(refreshRate);
        }
        fsm.ChangeState(States.Chase);
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
        base.Damage(amount, attacker);

        // Set the health bar's value to the current health.
        try
        {
            //healthCircle.enabled = true;
            //healthCircle.fillAmount -= amount / 100.0f;
            Debug.Log("YOYOYOYO " + healthCircle.fillAmount);
            Invoke("HideHealth", 3);
        }
        catch { }


        if (true) Debug.Log("crawler damaged");

        if (amount >= CurrentHealth)
        {
            if (true) Debug.Log("crawler killed");
            Killed();
        } else
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

        //Stop the pathfinder to prevent the dead entity moving and play the death animation
        try
        {
            pathfinder.Stop();
            _animator.Play("death1", PlayMode.StopAll);
            fsm.ChangeState(States.Death);
            _achievementManager.AchievementObtained("First Blood");
        } catch { }        
    }

    /// <summary>   Hides the health. </summary>
    ///
 

    public void HideHealth()
    {
        Debug.Log("aaa");
        //healthCircle.enabled = false;
    }
}

