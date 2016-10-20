// file:	assets\scripts\mobs\skeleboss.cs
//
// summary:	Implements the skeleboss class

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
public class SkeleBoss : BaseEntity
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
        /// <summary>   An enum constant representing the taunt option. </summary>
        Taunt,
        /// <summary>   An enum constant representing the summoning option. </summary>
        Summoning,
        /// <summary>   An enum constant representing the sheilding option. </summary>
        Sheilding,
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
    /// <summary>   The rotation speed. </summary>
    public float RotationSpeed = 10f;

    /// <summary>   The health slider. </summary>
	public Slider HealthSlider;
    /// <summary>   Name of the boss. </summary>
	public Text BossName;
    /// <summary>   The boss panel. </summary>
	public GameObject BossPanel;

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

    /// <summary>   True to summoned once. </summary>
    private bool _summonedOnce = false;
    /// <summary>   True to summoned twice. </summary>
    private bool _summonedTwice = false;

    /// <summary>   True to debug. </summary>
    private bool DEBUG = false;

    /// <summary>   Manager for achievement. </summary>
	private AchievementManager _achievementManager;
    /// <summary>   The cloud. </summary>
    private GameObject _cloud;
    /// <summary>   The storyline. </summary>
    Storyline _storyline;

    /// <summary>   True if this object is attacking. </summary>
    private bool _isAttacking;

    /// <summary>   The death sound. </summary>
	public AudioSource DeathSound;
    /// <summary>   The hit sound. </summary>
	public AudioSource HitSound;
    /// <summary>   The no mercy. </summary>
	public AudioSource NoMercy;
    /// <summary>   The walk sound. </summary>
	public AudioSource WalkSound;
    /// <summary>   The hurt sounds. </summary>
	public AudioSource HurtSounds;

    /// <summary>   The end of level trigger script. </summary>
	public EndOfLevelTrigger EndOfLevelTriggerScript;

    /// <summary>   Determines whether this instance is attacking. </summary>
    ///
 
    ///
    /// <returns>   <c>true</c> if this instance is attacking; otherwise, <c>false</c>. </returns>

    public bool isAttacking()
    {
        return _isAttacking;
    }

    /// <summary>   Sets the attacking. </summary>
    ///
 
    ///
    /// <param name="a">    if set to <c>true</c> [a]. </param>

    public void setAttacking(bool a)
    {
        _isAttacking = a;
    }

    /// <summary>   Initilized montser location, pathfinding, animation and the AI FSM. </summary>
    ///
 

    private void Awake()
	{
        if (DEBUG) Debug.Log("The skeleton wakes.");
        //base.Start();
        spawnLocation = this.gameObject.transform.position;

        _cloud = GameObject.Find("GreenShield");
        _cloud.SetActive(false);

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
        CurrentHealth = Health;
		WalkSound.loop = true;
		HealthSlider = HealthSlider.GetComponent<Slider>();
		HealthSlider.value = CurrentHealth;
		BossName = BossName.GetComponent<Text>();
		BossName.text = "Skeleton Boss";
		BossPanel.SetActive(false);
	}

    /// <summary>
    /// Initial start state for the FSM. Needed for the monster fsm libarary to work.
    /// </summary>
    ///
 
    ///
    /// <returns>   An IEnumerator. </returns>

    IEnumerator Init_Enter()
    {
        if (DEBUG) Debug.Log("skeleton state machine initilized.");

        pathfinder.enabled = false;
        yield return new WaitForSeconds(4f);
        pathfinder.enabled = true;
        _animator.Play("Idle", PlayMode.StopAll);
    }

    /// <summary>   Begins a cutscene. </summary>
    ///
 
    ///
    /// <param name="storyline">    The storyline. </param>

    public void BeginCutscene(Storyline storyline)
    {
        _storyline = storyline;
        fsm.ChangeState(States.Taunt);
    }

    /// <summary>
    /// Entry method for the taunt state. This plays the taunt animation and then transitions back to
    /// idle.
    /// </summary>
    ///
 
    ///
    /// <returns>   An IEnumerator. </returns>

    IEnumerator Taunt_Enter()
    {
		
        if (DEBUG) Debug.Log("Entered state: Taunt");
		NoMercy.Play ();
        yield return new WaitForSeconds(3f);

        _animator["Scream"].speed = 0.75f;
        _animator.Play("Scream", PlayMode.StopAll);

        while (_animator.isPlaying)
        {
            yield return new WaitForSeconds(0.2f);
        }
        _storyline.NextMoleMan();
        yield return new WaitForSeconds(0.8f);
        fsm.ChangeState(States.Chase);
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
        _isAttacking = true;
		BossPanel.SetActive(true);
        RotateTowards(target);


        pathfinder.enabled = false;

        _animator.Play("Attack", PlayMode.StopAll);
		yield return new WaitForSeconds (1f);
        target.GetComponent<BaseEntity>().Damage(AttackDamage, this.gameObject.transform);

		HitSound.Play ();

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

    /// <summary>   True to summoning. </summary>
    private bool _summoning = false;

    /// <summary>
    /// Entry method for the chase state. Chooses the closets player and moves towards them. Breaks
    /// if the player leaves the skeletons alert area, or comes into attack range.
    /// </summary>
    ///
 
    ///
    /// <returns>   An IEnumerator. </returns>

    IEnumerator Summoning_Enter()
    {
        if (DEBUG) Debug.Log("Entered state: Summoning begins");
        pathfinder.enabled = false;
		NoMercy.Play ();
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

    /// <summary>   Sheilding enter. </summary>
    ///
 
    ///
    /// <returns>   An IEnumerator. </returns>

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
    /// Entry method for the chase state. Chooses the closets player and moves towards them. Breaks
    /// if the player leaves the skeletons alert area, or comes into attack range.
    /// </summary>
    ///
 
    ///
    /// <returns>   An IEnumerator. </returns>

    IEnumerator Chase_Enter()
    {
        if (DEBUG) Debug.Log("Entered state: Chase");
		BossPanel.SetActive(true);
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
				WalkSound.Play ();
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
                fsm.ChangeState(States.Taunt, StateTransition.Overwrite);
            }

            if (DEBUG) Debug.Log("Chasing player:" + target.tag);

            //If they have moved outside the loose activation range, then taunt and stop chasing
            if (Vector3.Distance(target.position, this.gameObject.transform.position) > LooseActivationDistance)
            {
                if (DEBUG) Debug.Log("Lost player");
                _lockedOn = false;
                _isMoving = false;
				WalkSound.Stop ();
                fsm.ChangeState(States.Taunt, StateTransition.Overwrite);
            }

            //If the target comes into attack range, stop chasing and enter the attack state
            if (Vector3.Distance(target.position, this.gameObject.transform.position) < AttackRange)
            {
                if (DEBUG) Debug.Log("In attack range");
                _isMoving = false;
                _lockedOn = false;
				WalkSound.Stop ();
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
		BossPanel.SetActive(false);
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

    /// <summary>   Rotate towards. </summary>
    ///
 
    ///
    /// <param name="target">   Target for the. </param>

    private void RotateTowards(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * RotationSpeed);
    }

    /// <summary>   Death enter. </summary>
    ///
 

    private void Death_Enter()
    {
		BossPanel.SetActive(false);
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

        if (!_summoning)
        {
            base.Damage(amount, attacker);
			HealthSlider.value -= amount;

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
					HurtSounds.Play();
                    _animator.Play("Damage", PlayMode.StopSameLayer);
                }
                catch { }                
            }
        } else
        {
            if (DEBUG) Debug.Log("CANNOT DAMAGE SKELETON WHEN SUMMONING");
        }
    }

    /// <summary>   Boss dead wait. </summary>
    ///
 
    ///
    /// <returns>   An IEnumerator. </returns>

	public IEnumerator BossDeadWait () 
	{
		yield return new WaitForSeconds(1f);
        
		EndOfLevelTriggerScript.TriggerEndOfLevel ();
	}

    /// <summary>   Killed this object. </summary>
    ///
 

    public override void Killed()
    {
        base.Killed();

        var _achievementManager = (AchievementManager)GameObject.FindGameObjectWithTag("AchievementManager").GetComponent(typeof(AchievementManager));
        _achievementManager.AchievementObtained("Skeleton King.");

        //Stop the pathfinder to prevent the dead entity moving and play the death animation
        try
        {
			DeathSound.Play();
            _animator.Play("Death", PlayMode.StopAll);
            fsm.ChangeState(States.Death, StateTransition.Overwrite);
            _achievementManager.AchievementObtained("First Blood");

            // Triggering end of level 1 second after boss is defeated
            StartCoroutine(BossDeadWait());

        } catch { }        
    }
}

