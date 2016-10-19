using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;
using UnityEngine.UI;

/// <summary>
/// Controls the AI (using FSM) of the large skeleton bosses (e.i. the one found in the tutorial level)
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class SkeleBoss : BaseEntity
{
	//skeleton states
	public enum States
	{
		Init,
		Idle,
		Chase,
        Attack,
        Taunt,
        Summoning,
        Sheilding,
        Death
	}

    //skeleton stats
    public float HardActivationDistance = 50;
    public float LooseActivationDistance = 120;
    public float AttackSpeed = 1;
    public float AttackDamage = 5;
    public float Health = 5000;
    public float AttackRange = 24;
    public float Range = .1f;
    public float WalkSpeed = 9f;
    public float RunSpeed = 15f;
    public float SprintSpeed = 35f;
    public float AttackCooldown = 0.5f;
    public float RotationSpeed = 10f;


    public Image healthCircle;                                 // Reference to the UI's health circle.

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
    private bool _isSprinting;
    private bool _isMoving;
    private int _walkCount;

    private bool _summonedOnce = false;
    private bool _summonedTwice = false;

    private bool DEBUG = false;

	private AchievementManager _achievementManager;
    private GameObject _cloud;

    private bool _isAttacking;

    /// <summary>
    /// Determines whether this instance is attacking.
    /// </summary>
    /// <returns>
    ///   <c>true</c> if this instance is attacking; otherwise, <c>false</c>.
    /// </returns>
    public bool isAttacking()
    {
        return _isAttacking;
    }

    /// <summary>
    /// Sets the attacking.
    /// </summary>
    /// <param name="a">if set to <c>true</c> [a].</param>
    public void setAttacking(bool a)
    {
        _isAttacking = a;
    }

    /// <summary>
    /// Initilized montser location, pathfinding, animation and the AI FSM
    /// </summary>
    private void Awake()
	{
        if (DEBUG) Debug.Log("The skeleton wakes.");
        //base.Start();
        spawnLocation = this.gameObject.transform.position;

        _cloud = GameObject.Find("Holy Shine");
        _cloud.SetActive(false);

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
    IEnumerator Init_Enter()
    {
        if (DEBUG) Debug.Log("skeleton state machine initilized.");

        pathfinder.enabled = false;
        yield return new WaitForSeconds(4f);
        pathfinder.enabled = true;

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
        _isAttacking = true;

        RotateTowards(target);

        pathfinder.enabled = false;

        _animator.Play("Attack", PlayMode.StopAll);
        target.GetComponent<BaseEntity>().Damage(AttackDamage, this.gameObject.transform);
        

        while (_animator.isPlaying)
        {
            yield return new WaitForSeconds(0.25f);
            if (DEBUG) Debug.Log("Waiting for attack animation to finish");
        }        
        
        if (_isSprinting) _isSprinting = false;

        //yield return new WaitForSeconds(AttackCooldown);
        

        pathfinder.enabled = true;
        _isAttacking = false;

        fsm.ChangeState(States.Chase);
    }

    private bool _summoning = false;

    /// <summary>
    /// Entry method for the chase state. Chooses the closets player and moves towards them. Breaks if the player leaves the 
    /// skeletons alert area, or comes into attack range.
    /// </summary>
    /// <returns></returns>
    IEnumerator Summoning_Enter()
    {
        if (DEBUG) Debug.Log("Entered state: Summoning begins");
        pathfinder.enabled = false;

        _animator.Play("Roar", PlayMode.StopAll);
        while (_animator.isPlaying)
        {
            yield return new WaitForSeconds(0.25f);
        }

        _cloud.SetActive(true);
        _summoning = true;

        _animator["Summon2"].speed = 0.2f;
        _animator.Play("Summon2", PlayMode.StopAll);

        float refreshRate = !_isSprinting ? 0.3f : 0.05f;        
       
        //Check too see if all minion mobs have been killed
        GameObject[] mobs = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject mob in mobs)
        {
            //if (DEBUG) Debug.Log("Checking enemy tagged object");
            SkeleMob a = mob.transform.GetComponent<SkeleMob>();
            if (a != null)
            {
                if (DEBUG) Debug.Log("Checking skele mob...");
                if (a.isDead)
                {
                    if (DEBUG) Debug.Log("Reviving.");
                    a.isDead = false;
                }
            }
        }
        fsm.ChangeState(States.Sheilding);
    }

    IEnumerator Sheilding_Enter()
    {
        if (DEBUG) Debug.Log("Entered state: Summoning exit");

        float refreshRate = !_isSprinting ? 0.3f : 0.05f;
        _summoning = true;

        while (_summoning)
        {
            //Check too see if all minion mobs have been killed
            GameObject[] mobs = GameObject.FindGameObjectsWithTag("Enemy");

            bool _oneAlive = false;
            foreach (GameObject mob in mobs)
            {
                //if (DEBUG) Debug.Log("Checking enemy tagged object");
                SkeleMob a = mob.transform.GetComponent<SkeleMob>();
                if (a != null)
                {
                    if (DEBUG) Debug.Log("Checking skele mob...");
                    if (!a.isDead)
                    {
                        if (DEBUG) Debug.Log("Found one alive.");
                        _oneAlive = true;
                    }
                }
            }

            if (!_oneAlive)
            {
                break;
            }

            yield return new WaitForSeconds(refreshRate);
        }

        _cloud.SetActive(false);
        _summoning = false;
        pathfinder.enabled = true;
        fsm.ChangeState(States.Chase, StateTransition.Overwrite);
    }

    /// <summary>
    /// Entry method for the chase state. Chooses the closets player and moves towards them. Breaks if the player leaves the 
    /// skeletons alert area, or comes into attack range.
    /// </summary>
    /// <returns></returns>
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

            //If health below
            

            if (!_isMoving)
            {
                //_animator["Walk"].speed = _isSprinting ? 1.5f : 1.0f;
                _animator.Play("Walk", PlayMode.StopAll);
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
                fsm.ChangeState(States.Taunt, StateTransition.Overwrite);
            }

            if (DEBUG) Debug.Log("Chasing player:" + target.tag);

            //If they have moved outside the loose activation range, then taunt and stop chasing
            if (Vector3.Distance(target.position, this.gameObject.transform.position) > LooseActivationDistance)
            {
                if (DEBUG) Debug.Log("Lost player");
                _lockedOn = false;
                _isMoving = false;
                fsm.ChangeState(States.Taunt, StateTransition.Overwrite);
            }

            //If the target comes into attack range, stop chasing and enter the attack state
            if (Vector3.Distance(target.position, this.gameObject.transform.position) < AttackRange)
            {
                if (DEBUG) Debug.Log("In attack range");
                _isMoving = false;
                _lockedOn = false;
                fsm.ChangeState(States.Attack, StateTransition.Overwrite);
            }

            //Set the speed of the pathfinder (either running or sprinting) and the target positions
            pathfinder.speed = _isSprinting ? SprintSpeed : RunSpeed;
            pathfinder.acceleration = 13f;
            pathfinder.angularSpeed = 900f;
            pathfinder.SetDestination(target.position);
            Debug.Log(pathfinder.speed);

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
        fsm.ChangeState(States.Chase, StateTransition.Overwrite);
    }

    private void RotateTowards(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * RotationSpeed);
    }

    private void Death_Enter()
    {
        if (DEBUG) Debug.Log("Entered state: Death");
    }

    public override void Damage(float amount, Transform attacker)
    {
        if (isDead) return;

        if (!_summoning)
        {
            base.Damage(amount, attacker);

            if (CurrentHealth < 150 && !_summonedOnce)
            {
                _summonedOnce = true;
                Debug.Log("Health reduced to first summoning level");
                fsm.ChangeState(States.Summoning, StateTransition.Overwrite);
                return;
            }

            if (CurrentHealth < 40 && !_summonedTwice)
            {
                _summonedTwice = true;
                Debug.Log("Health reduced to first summoning level");
                fsm.ChangeState(States.Summoning, StateTransition.Overwrite);
                return;
            }

            // Set the health bar's value to the current health.
            try
            {
                //healthCircle.enabled = true;
                //healthCircle.fillAmount -= amount / 100.0f;
                Debug.Log("YOYOYOYO " + healthCircle.fillAmount);
                Invoke("HideHealth", 3);
            }
            catch { }


            if (true) Debug.Log("skeleton damaged");

            if (amount >= CurrentHealth)
            {
                if (true) Debug.Log("skeleton killed");
                Killed();
            }
            else
            {
                try
                {
                    _animator.Play("Damage", PlayMode.StopSameLayer);
                }
                catch { }                
            }
        } else
        {
            if (DEBUG) Debug.Log("CANNOT DAMAGE SKELETON WHEN SUMMONING");
        }
    }

    public override void Killed()
    {
        base.Killed();

        //Stop the pathfinder to prevent the dead entity moving and play the death animation
        try
        {
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
        //healthCircle.enabled = false;
    }
}

