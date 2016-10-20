using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;
using UnityEngine.UI;

/// <summary>
/// Controls the AI (using FSM) of the large moleman bosses (e.i. the one found in the tutorial level)
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class MolemanBoss : BaseEntity
{
    //moleman states
    public enum States
    {
        Init,
        Idle,
        Chase,
        Attack,
        Taunt,
        RoarSummon,
        Death
    }

    //moleman stats
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
    public float AttackCooldown = 2f;
    public float RotationSpeed = 10f;

	public Slider HealthSlider;
    public Text BossName;
    public GameObject BossPanel;

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
    private bool _canTakeDamage = true;

    private bool DEBUG = false;
    public bool isBoss = true;

	public EndOfLevelTrigger EndOfLevelTriggerScript;

    private AchievementManager _achievementManager;

    /// <summary>
    /// Initilized montser location, pathfinding, animation and the AI FSM
    /// </summary>
    private void Awake()
    {
        if (DEBUG) Debug.Log("The moleman wakes.");
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

    void Update()
    {
        _canTakeDamage = true;
    } 

    public void MockUp()
    {
        base.Start();
    }

    private void Start()
    {
        _achievementManager = (AchievementManager)GameObject.FindGameObjectWithTag("AchievementManager").GetComponent(typeof(AchievementManager));
        CurrentHealth = Health;
        HealthSlider = HealthSlider.GetComponent<Slider>();
		HealthSlider.value = CurrentHealth;
		BossName = BossName.GetComponent<Text>();
		BossName.text = "Moleman";
		BossPanel.SetActive(false);
        
    }

    /// <summary>
    /// Initial start state for the FSM. Needed for the monster fsm libarary to work.
    /// </summary>
    private void Init_Enter()
    {
        if (DEBUG) Debug.Log("moleman state machine initilized.");
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
        if (!isDead)
        {
            if (DEBUG) Debug.Log("Entered state: Attack");
            if (isBoss) BossPanel.SetActive(true);
            RotateTowards(target);

            pathfinder.enabled = false;

            _animator.Play("creature1Attack1", PlayMode.StopAll);
            yield return new WaitForSeconds(0.4f);
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
    }

    private bool _summoning = false;

    IEnumerator RoarSummon_Enter()
    {
        _summoning = true;
        pathfinder.enabled = false;

        _animator.Play("creature1roar", PlayMode.StopAll);

        while (_animator.isPlaying)
        {
            yield return new WaitForSeconds(0.25f);
        }

        fsm.ChangeState(States.Chase);
        pathfinder.enabled = true;
        _summoning = true;
    }

    IEnumerator Chase_Enter()
    {
        if (DEBUG) Debug.Log("Entered state: Chase");
        if (isBoss) BossPanel.SetActive(true);
        float refreshRate = !_isSprinting ? 0.03f : 0.03f;
        _lockedOn = true;

        //Find closet player
        Transform player1 = GameObject.FindGameObjectWithTag("Player").transform;
        Transform player2 = GameObject.FindGameObjectWithTag("Player2").transform;

        while (_lockedOn && !isDead)
        {

            if (!_isMoving)
            {
                //_animator["run"].speed = _isSprinting ? 1.5f : 1.0f;
                _animator.Play("creature1walkforward", PlayMode.StopAll);
                _isMoving = true;
            }

            //If player 2 is closer to the moleman, and is not dead, then chase them Otherwise, player 1 is closer.              
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
            pathfinder.acceleration = 19f;
            pathfinder.angularSpeed = 900f;
            pathfinder.SetDestination(target.position);

            //Every so often sprint at the player
            if (_walkCount > 12)
            {
                if (DEBUG) Debug.Log("moleman has started sprinting!");
                _isSprinting = true;
                _isMoving = false;
                _walkCount = -5;
            }
            _walkCount++;

            yield return new WaitForSeconds(refreshRate);
        }
    }

    public bool isSummoning()
    {
        return _summoning;
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
        if (isBoss) BossPanel.SetActive(false);
        //Check to see if either player is within activation range
        while (!_lockedOn)
        {
            if (DEBUG) Debug.Log("Waiting for players.");

            //Move the moleman back to its "lair" if there are no targets to chase/attack
            pathfinder.SetDestination(spawnLocation);

            //Retrieve the distance to the two playesr and their entity objects
            float player1distance = Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, this.gameObject.transform.position);
            float player2distance = Vector3.Distance(GameObject.FindGameObjectWithTag("Player2").transform.position, this.gameObject.transform.position);
            BaseEntity player1 = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<BaseEntity>();
            BaseEntity player2 = GameObject.FindGameObjectWithTag("Player2").transform.GetComponent<BaseEntity>();

            //If there is a non-dead player inside the hard activiation distance, break the loop and chase them
            if (!player1.isDead && (player1distance < HardActivationDistance) || !player2.isDead && (player2distance < HardActivationDistance))
            {
                if (DEBUG) Debug.Log("Player found.");
                _lockedOn = true;
            }
            yield return new WaitForSeconds(refreshRate);
        }
        fsm.ChangeState(States.Chase);
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
        if (isBoss) BossPanel.SetActive(false);
    }

    private bool _summonedOnce;
    private bool _summonedTwice;

    public override void Damage(float amount, Transform attacker)
    {
        if (isDead) return;
        if (_summoning) return;
        base.Damage(amount, attacker);

        if (CurrentHealth < 3800 && !_summonedOnce)
        {
            _summonedOnce = true;
            Debug.Log("Health reduced to first summoning level");
            fsm.ChangeState(States.RoarSummon, StateTransition.Overwrite);
            return;
        }

        if (CurrentHealth < 1200 * 0.25 && !_summonedTwice)
        {
            _summonedTwice = true;
            Debug.Log("Health reduced to first summoning level");
            fsm.ChangeState(States.RoarSummon, StateTransition.Overwrite);
            return;
        }

        // Set the health bar's value to the current health.
        try
        {
            if (isBoss)
            {
				HealthSlider.value -= amount;
            }

        }
        catch { }


        if (DEBUG) Debug.Log("moleman damaged");

        if (amount >= CurrentHealth)
        {
            if (DEBUG) Debug.Log("moleman killed");
            Killed();
        }
        else
        {
            try
            {
                _animator.Play("creature1GetHit", PlayMode.StopSameLayer);
            }
            catch { }

        }
    }

    public override void Killed()
    {
        base.Killed();

        //Stop the pathfinder to prevent the dead entity moving and play the death animation
        try
        {
            pathfinder.Stop();
            _animator.Play("creature1Die", PlayMode.StopAll);
            fsm.ChangeState(States.Death, StateTransition.Overwrite);
            _achievementManager.AddProgressToAchievement("First Blood", 1.0f);

			// Triggering end of level 1 second after boss is defeated
			yield return new WaitForSeconds(1f);
			EndOfLevelTriggerScript.TriggerEndOfLevel ();
        }
        catch { }
    }


    void OnTriggerStay(Collider other)
    {
        if (_canTakeDamage && other.tag.Equals("LightningCollision"))
        {
            Damage(10f, null);
            _canTakeDamage = false;
        }
    }
}

