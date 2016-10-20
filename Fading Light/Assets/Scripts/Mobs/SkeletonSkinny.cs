﻿// file:	Assets\Scripts\Mobs\SkeletonSkinny.cs
//
// summary:	Implements the skeleton skinny class

using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;
using UnityEngine.UI;

/// <summary>
/// Controls the AI (using FSM) of the large skeleton bosses (e.i. the one found in the tutorial
/// level)
/// </summary>
///
/// <remarks>    . </remarks>

[RequireComponent(typeof(NavMeshAgent))]
public class SkeletonSkinny : BaseEntity
{
	//skeleton states

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
        /// <summary>   An enum constant representing the wake option. </summary>
        Wake,
        /// <summary>   An enum constant representing the sleep option. </summary>
        Sleep, 
        /// <summary>   An enum constant representing the death option. </summary>
        Death
	}

    //skeleton stats
    /// <summary>   The hard activation distance. </summary>
    public float HardActivationDistance = 50;
    /// <summary>   The loose activation distance. </summary>
    public float LooseActivationDistance = 120;
    /// <summary>   The attack speed. </summary>
    public float AttackSpeed = 1;
    /// <summary>   The attack damage. </summary>
    public float AttackDamage = 5;
    /// <summary>   The health. </summary>
    public float Health = 100;
    /// <summary>   The attack range. </summary>
    public float AttackRange = 10;
    /// <summary>   The range. </summary>
    public float Range = .1f;
    /// <summary>   The walk speed. </summary>
    public float WalkSpeed = 9f;
    /// <summary>   The run speed. </summary>
    public float RunSpeed = 5f;
    /// <summary>   The sprint speed. </summary>
    public float SprintSpeed = 35f;
    /// <summary>   The attack cooldown. </summary>
    public float AttackCooldown = 0.5f;
    /// <summary>   The angular speed. </summary>
    public float AngularSpeed = 10f;

    /// <summary>   True if this object is active. </summary>
    public bool IsActive = true;

    //Reference to the UI's health circle.
    /// <summary>   The health circle. </summary>
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
    /// <summary>   True to sleeping. </summary>
    private bool _sleeping;
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

    /// <summary>   The walk sound. </summary>
	public AudioSource WalkSound;
    /// <summary>   The attack sound. </summary>
	public AudioSource AttackSound;
    /// <summary>   The death sound. </summary>
	public AudioSource DeathSound;
    /// <summary>   The hurt sound. </summary>
	public AudioSource HurtSound;
    /// <summary>   The rise sound. </summary>
	public AudioSource RiseSound;

    /// <summary>   Initilized montser location, pathfinding, animation and the AI FSM. </summary>
    ///
 

    private void Awake()
	{
		WalkSound.loop = true;         
        if (DEBUG) Debug.Log("The skeleton wakes.");
        //base.Start();
        spawnLocation = this.gameObject.transform.position;       

        //Initlize the pathfinder, collision range and animator 
        pathfinder = GetComponent<NavMeshAgent>();
        _collisionRange = GetComponent<CapsuleCollider>().radius;
        _animator = GetComponentInChildren<Animation>();      

        //Create the FSM controller
        fsm = StateMachine<States>.Initialize(this);
        
        float randomNumber = Random.Range(1, 10);
        if (randomNumber < 5)
        {
            isDead = true;
            fsm.ChangeState(States.Death);
        } else
        {
            fsm.ChangeState(States.Init);
        }
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
        if (DEBUG) Debug.Log("skeleton state machine initilized.");    
        fsm.ChangeState(States.Sleep);
    }

    /// <summary>
    /// Entry method for the taunt state. This plays the taunt animation and then transitions back to
    /// idle.
    /// </summary>
    ///
 
    ///
    /// <returns>   An IEnumerator. </returns>

    private IEnumerator Sleep_Enter()
    {
        if (IsActive)
        {
            fsm.ChangeState(States.Death);
        }

        if (DEBUG) Debug.Log("Entered state: Sleep");
        float refreshRate = 0.8f;
        _sleeping = true;
        _animator["NewStandUp01"].speed = 0;
        _animator.Play("NewStandUp01", PlayMode.StopAll);

        //Check to see if either player is within activation range
        while (_sleeping)
        {
            if (DEBUG) Debug.Log("Waiting for players to wake me up.");
            
            //Retrieve the distance to the two playesr and their entity objects
            float player1distance = Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, this.gameObject.transform.position);
            float player2distance = Vector3.Distance(GameObject.FindGameObjectWithTag("Player2").transform.position, this.gameObject.transform.position);
            BaseEntity player1 = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<BaseEntity>();
            BaseEntity player2 = GameObject.FindGameObjectWithTag("Player2").transform.GetComponent<BaseEntity>();

            //If there is a non-dead player inside the hard activiation distance, break the loop and wake up them
            if (!player1.isDead && (player1distance < HardActivationDistance) || !player2.isDead && (player2distance < HardActivationDistance))
            {
                if (DEBUG) Debug.Log("Skele woken up by player.");
                _sleeping = false;
            }
            yield return new WaitForSeconds(refreshRate);
        }
        fsm.ChangeState(States.Wake);
    }

    /// <summary>
    /// Entry method for the taunt state. This plays the taunt animation and then transitions back to
    /// idle.
    /// </summary>
    ///
 
    ///
    /// <returns>   An IEnumerator. </returns>

    private IEnumerator Wake_Enter()
    {
		RiseSound.Play ();
        if (DEBUG) Debug.Log("Entered state: Wake");
        _animator["NewStandUp01"].speed = 1;
        _animator.Play("NewStandUp01", PlayMode.StopAll);

        while (_animator.isPlaying)
        {
            yield return new WaitForSeconds(0.25f);
            if (DEBUG) Debug.Log("Waiting for waking animation to finish");
        }
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
            pathfinder.enabled = false;
			AttackSound.Play ();
            _animator["SwingNormal"].speed = 1.5f;
            _animator.Play("SwingNormal", PlayMode.StopAll);
            target.GetComponent<BaseEntity>().Damage(AttackDamage, this.gameObject.transform);


            while (_animator.isPlaying)
            {
                yield return new WaitForSeconds(0.25f);
                if (DEBUG) Debug.Log("Waiting for attack animation to finish");
            }

            if (_isSprinting) _isSprinting = false;

            //yield return new WaitForSeconds(AttackCooldown);


            pathfinder.enabled = true;

            fsm.ChangeState(States.Chase);
        }        
    }

    /// <summary>
    /// Entry method for the chase state. Chooses the closets player and moves towards them. Breaks
    /// if the player leaves the skeletons alert area, or comes into attack range.
    /// </summary>
    ///
 
    ///
    /// <returns>   An IEnumerator. </returns>

    IEnumerator Chase_Enter()
    {
        if (DEBUG) Debug.Log("Entered state: Chase");

        float refreshRate = 0.05f;
        _lockedOn = true;

        //Find closet player
        Transform player1 = GameObject.FindGameObjectWithTag("Player").transform;
        Transform player2 = GameObject.FindGameObjectWithTag("Player2").transform;

        while (_lockedOn && !isDead)
        {

            if (!_isMoving)
            {
				WalkSound.Play ();
                //_animator["Run"].speed = _isSprinting ? 1.5f : 1.0f;
                _animator.Play("Walk02", PlayMode.StopAll);
                _isMoving = true;
            }

            //If player 2 is closer to the skeleton, and is not dead, then chase them Otherwise, player 1 is closer.              
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
				WalkSound.Stop ();
                fsm.ChangeState(States.Idle);
            }

            if (DEBUG) Debug.Log("Chasing player:" + target.tag);

            //If they have moved outside the loose activation range, then taunt and stop chasing
            if (Vector3.Distance(target.position, this.gameObject.transform.position) > LooseActivationDistance)
            {
                if (DEBUG) Debug.Log("Lost player");
                _lockedOn = false;
                _isMoving = false;
				WalkSound.Stop ();
                fsm.ChangeState(States.Idle);
            }

            //If the target comes into attack range, stop chasing and enter the attack state
            if (Vector3.Distance(target.position, this.gameObject.transform.position) < AttackRange)
            {
                if (DEBUG) Debug.Log("In attack range");
                _isMoving = false;
                _lockedOn = false;
				WalkSound.Stop ();
                fsm.ChangeState(States.Attack);
            }

            //Set the speed of the pathfinder (either running or sprinting) and the target positions
            pathfinder.speed = 10f;
            pathfinder.acceleration = 13f;
            pathfinder.angularSpeed = 900f;
            pathfinder.SetDestination(target.position);

            //Every so often sprint at the player
            if (_walkCount > 12)
            {
                if (true) Debug.Log("skeleton has started sprinting!");
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

            //Move the skeleton back to its "lair" if there are no targets to chase/attack
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
        if (isDead) return;
        base.Damage(amount, attacker);

        // Set the health bar's value to the current health.
        try
        {
            //healthCircle.enabled = true;
            //healthCircle.fillAmount -= amount / 100.0f;
            //Invoke("HideHealth", 3);
        }
        catch { }


        if (true) Debug.Log("skeleton damaged");

        if (amount >= CurrentHealth)
        {
            if (true) Debug.Log("skeleton killed");
            Killed();
        } else
        {
            try
            {
				HurtSound.Play();
                _animator.Play("Hit2", PlayMode.StopSameLayer);
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
			DeathSound.Play();
            _animator.Play("Death", PlayMode.StopAll);
            fsm.ChangeState(States.Death, StateTransition.Overwrite);
            _achievementManager.AchievementObtained("First Blood");
        } catch { }        
    }

    /// <summary>   Hides the health. </summary>
    ///
 

    public void HideHealth()
    {
        Debug.Log("aaa");
        healthCircle.enabled = false;
    }
}

