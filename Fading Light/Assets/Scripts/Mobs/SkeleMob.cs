using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;
using UnityEngine.UI;

/// <summary>
/// Controls the AI (using FSM) of the large skeleton bosses (e.i. the one found in the tutorial level)
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class SkeleMob : BaseEntity
{
	//skeleton states
	public enum States
	{
		Init,
		Idle,
		Chase,
        Attack,
        Wake,
        Sleep,
        Revival,
        Death
	}

    //skeleton stats
    public float HardActivationDistance = 50;
    public float LooseActivationDistance = 120;
    public float AttackSpeed = 1;
    public float AttackDamage = 5;
    public float Health = 100;
    public float AttackRange = 10;
    public float Range = .1f;
    public float WalkSpeed = 9f;
    public float RunSpeed = 5f;
    public float SprintSpeed = 35f;
    public float AttackCooldown = 0.5f;
    public float AngularSpeed = 10f;

    //Reference to the UI's health circle.
    public Image healthCircle;                                 

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
    private bool _sleeping;
    private bool _inAttackRange;
    private bool _isSprinting;
    private bool _isMoving;
    private int _walkCount;

    private bool DEBUG = false;

	private AchievementManager _achievementManager;

    /// <summary>
    /// Initilized montser location, pathfinding, animation and the AI FSM
    /// </summary>
    private void Awake()
	{
        if (DEBUG) Debug.Log("The skeleton wakes.");
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

    private void Start(){
		_achievementManager = (AchievementManager)GameObject.FindGameObjectWithTag ("AchievementManager").GetComponent(typeof(AchievementManager));
        //healthCircle.enabled = false;
        CurrentHealth = Health;
	}

    /// <summary>
    /// Initial start state for the FSM. Needed for the monster fsm libarary to work.
    /// </summary>
    private void Init_Enter()
    {
        if (DEBUG) Debug.Log("skeleton state machine initilized.");
        fsm.ChangeState(States.Sleep);
    }

    /// <summary>
    /// Entry method for the taunt state. This plays the taunt animation and then transitions back to idle
    /// </summary>
    private IEnumerator Sleep_Enter()
    {
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
    /// Entry method for the taunt state. This plays the taunt animation and then transitions back to idle
    /// </summary>
    private IEnumerator Wake_Enter()
    {
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
    /// Entry method for the attack state. Plays the attack animation once, and deals damage once, before transitioning back to the chase state.
    /// </summary>
    /// <returns></returns>
    IEnumerator Attack_Enter()
    {
        if (DEBUG) Debug.Log("Entered state: Attack");
        pathfinder.enabled = false;

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

    /// <summary>
    /// Entry method for the chase state. Chooses the closets player and moves towards them. Breaks if the player leaves the 
    /// skeletons alert area, or comes into attack range.
    /// </summary>
    /// <returns></returns>
    IEnumerator Chase_Enter()
    {
        if (DEBUG) Debug.Log("Entered state: Chase");

        float refreshRate = 0.05f;
        _lockedOn = true;

        //Find closet player
        Transform player1 = GameObject.FindGameObjectWithTag("Player").transform;
        Transform player2 = GameObject.FindGameObjectWithTag("Player2").transform;

        while (_lockedOn)
        {

            if (!_isMoving)
            {
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
                fsm.ChangeState(States.Idle);
            }

            if (DEBUG) Debug.Log("Chasing player:" + target.tag);

            //If they have moved outside the loose activation range, then taunt and stop chasing
            if (Vector3.Distance(target.position, this.gameObject.transform.position) > LooseActivationDistance)
            {
                if (DEBUG) Debug.Log("Lost player");
                _lockedOn = false;
                _isMoving = false;
                fsm.ChangeState(States.Idle);
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
    /// Entry state for the idle state. Waits in place and constantly checks to see if any players have entered its alert area. If a player enters the area
    /// if transitions to the chase state to chase them down.
    /// </summary>
    /// <returns></returns>
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

    private IEnumerator Death_Enter()
    {
        float refreshRate = 0.5f;
        if (DEBUG) Debug.Log("Entered state: Death");
        //Check to see if either player is within activation range
        while (isDead)
        {
            if (DEBUG) Debug.Log("Waiting for revival");
            yield return new WaitForSeconds(refreshRate);
        }
        this.gameObject.GetComponent<CapsuleCollider>().enabled = true;
        fsm.ChangeState(States.Revival);
    }

    private IEnumerator Revival_Enter()
    {
        if (DEBUG) Debug.Log("Entered state: Revival");
        
        _animator.Play("Resurrection", PlayMode.StopAll);
        while (_animator.isPlaying)
        {
            yield return new WaitForSeconds(0.25f);
            if (DEBUG) Debug.Log("Waiting for revive animation to finish");
        }
        fsm.ChangeState(States.Chase);
    }

    public override void Damage(float amount, Transform attacker)
    {
        if (isDead) return;
        
        if (DEBUG) Debug.Log("skeleton damaged");
        base.Damage(amount, attacker);

        if (amount >= CurrentHealth)
        {
            if (DEBUG) Debug.Log("skeleton killed");
            Killed();
        } else
        {
            try
            {
                _animator.Play("Hit2", PlayMode.StopSameLayer);
            } catch { }
            
        }
    }

    public override void Killed()
    {
        base.Killed();

        //Stop the pathfinder to prevent the dead entity moving and play the death animation
        try
        {
            pathfinder.Stop();
            this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
            _animator.Play("Death", PlayMode.StopAll);
            fsm.ChangeState(States.Death, StateTransition.Overwrite);
            _achievementManager.AddProgressToAchievement("First Blood", 1.0f);
        } catch { }        
    }

    /// <summary>
    /// Hides the health.
    /// </summary>
    public void HideHealth()
    {
        Debug.Log("aaa");
        healthCircle.enabled = false;
    }
}

