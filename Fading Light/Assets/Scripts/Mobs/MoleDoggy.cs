// file:	assets\scripts\mobs\moledoggy.cs
//
// summary:	Implements the moledoggy class

using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;
using UnityEngine.UI;
using Assets.Scripts.Mobs;

/// <summary>
/// Controls the AI (using FSM) of the large molemans dog bosses (e.i. the one found in the
/// tutorial level)
/// </summary>
///
/// <remarks>   Jack, 21/10/2016. </remarks>

[RequireComponent(typeof(NavMeshAgent))]
public class MoleDoggy : BaseEntity
{
	//molemans dog states

    /// <summary>   Values that represent states. </summary>
    ///
    /// <remarks>   Jack, 21/10/2016. </remarks>

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
        /// <summary>   An enum constant representing the fireball spawning option. </summary>
        FireballSpawning,
        /// <summary>   An enum constant representing the death option. </summary>
        Death
	}

    //molemans dog stats
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
    /// <summary>   True to active. </summary>
    private bool _active = false;

    /// <summary>   True to debug. </summary>
    private bool DEBUG = false;

    /// <summary>   Manager for achievement. </summary>
	private AchievementManager _achievementManager;

    /// <summary>   The cloud. </summary>
    private GameObject _cloud;
    /// <summary>   The shield. </summary>
    private GameObject _shield;

    /// <summary>   The hit. </summary>
    public AudioClip Hit;
    /// <summary>   The death. </summary>
    public AudioClip Death;
    /// <summary>   The attack. </summary>
    public AudioClip Attack;
    /// <summary>   The aoe. </summary>
    public AudioClip AOE;
    /// <summary>   Source for the. </summary>
    private AudioSource _source;

    /// <summary>   The end of level trigger script. </summary>
	public EndOfLevelTrigger EndOfLevelTriggerScript;

    /// <summary>   Initilized montser location, pathfinding, animation and the AI FSM. </summary>
    ///
    /// <remarks>   Jack, 21/10/2016. </remarks>

    private void Awake()
	{
        _cloud = GameObject.Find("AOE");
        _cloud.SetActive(false);
        _shield = GameObject.Find("RedShield");
        _shield.SetActive(false);
        _source = GetComponent<AudioSource>();

        if (DEBUG) Debug.Log("The molemans dog wakes.");
        //base.Start();
        spawnLocation = this.gameObject.transform.position;

        Transform collider = this.transform.Find("AOECollider");
        collider.gameObject.SetActive(false);

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
    /// <remarks>   Jack, 21/10/2016. </remarks>

    public void MockUp()
    {
        base.Start();
    }

    /// <summary>   Starts this object. </summary>
    ///
    /// <remarks>   Jack, 21/10/2016. </remarks>

    private void Start(){
		_achievementManager = (AchievementManager)GameObject.FindGameObjectWithTag ("AchievementManager").GetComponent(typeof(AchievementManager));
        CurrentHealth = Health;
		EndOfLevelTriggerScript.GetComponent<EndOfLevelTrigger> ();

		HealthSlider = HealthSlider.GetComponent<Slider>();
		HealthSlider.value = CurrentHealth;
		BossName = BossName.GetComponent<Text>();
		BossName.text = "Mole Dog";
		BossPanel.SetActive(false);
	}

    /// <summary>
    /// Initial start state for the FSM. Needed for the monster fsm libarary to work.
    /// </summary>
    ///
    /// <remarks>   Jack, 21/10/2016. </remarks>

    private void Init_Enter()
    {
        if (DEBUG) Debug.Log("molemans dog state machine initilized. Waiting for cutscene");
        //fsm.ChangeState(States.Idle);
    }

    /// <summary>   The storyline. </summary>
    Storyline _storyline;

    /// <summary>   Begins a cutscene. </summary>
    ///
    /// <remarks>   Jack, 21/10/2016. </remarks>
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
    /// <remarks>   Jack, 21/10/2016. </remarks>
    ///
    /// <returns>   An IEnumerator. </returns>

    IEnumerator Taunt_Enter()
    {
        if (DEBUG) Debug.Log("Entered state: Taunt");       
        _animator.Play("Idle", PlayMode.StopAll);
        yield return new WaitForSeconds(2f);
        _animator.Play("Attack", PlayMode.StopAll);
        while (_animator.isPlaying)
        {
            yield return new WaitForSeconds(0.2f);
        }
        _storyline.NextMoleMan();
        _animator.Play("Idle", PlayMode.StopAll);
        yield return new WaitForSeconds(1f);
        fsm.ChangeState(States.Chase);
    }

    /// <summary>
    /// Entry method for the attack state. Plays the attack animation once, and deals damage once,
    /// before transitioning back to the chase state.
    /// </summary>
    ///
    /// <remarks>   Jack, 21/10/2016. </remarks>
    ///
    /// <returns>   An IEnumerator. </returns>

    IEnumerator Attack_Enter()
    {
        if (!isDead)
        {

            if (DEBUG) Debug.Log("Entered state: Attack");
			BossPanel.SetActive(true);
            RotateTowards(target, false);

            pathfinder.enabled = false;
           
            _animator.Play("Attack", PlayMode.StopAll);
            _source.PlayOneShot(Attack);

            yield return new WaitForSeconds(1.15f);

            if (Vector3.Distance(target.position, this.gameObject.transform.position) < AttackRange + 4f)
            {
                target.GetComponent<BaseEntity>().Damage(AttackDamage, this.gameObject.transform);
            }

            while (_animator.isPlaying)
            {
                yield return new WaitForSeconds(0.2f);
                if (DEBUG) Debug.Log("Waiting for attack animation to finish");
            }

            pathfinder.enabled = true;
            fsm.ChangeState(States.Chase);
        }        
    }

    /// <summary>   True if this object is rotating. </summary>
    private bool _isRotating = false;

    //http://gamedev.stackexchange.com/questions/102126/unable-to-stop-navmeshagent-for-rotation-before-moving

    /// <summary>   Rotate agent. </summary>
    ///
    /// <remarks>   Jack, 21/10/2016. </remarks>
    ///
    /// <param name="currentRotation">  The current rotation. </param>
    /// <param name="targetRotation">   Target rotation. </param>
    ///
    /// <returns>   An IEnumerator. </returns>

    IEnumerator RotateAgent(Quaternion currentRotation, Quaternion targetRotation)
    {
        _isRotating = true;
        while (currentRotation != targetRotation) {
            transform.rotation = Quaternion.RotateTowards(currentRotation, targetRotation, RotationSpeed * Time.deltaTime);
            yield return 1;
        }
        _isRotating = false;
    }

    /// <summary>   Fireball spawning enter. </summary>
    ///
    /// <remarks>   Jack, 21/10/2016. </remarks>
    ///
    /// <returns>   An IEnumerator. </returns>

    IEnumerator FireballSpawning_Enter()
    {        
        _cloud.SetActive(true);
        _shield.SetActive(true);
        pathfinder.enabled = false;
        Transform collider = this.transform.Find("AOECollider");
        collider.gameObject.SetActive(true);

        yield return new WaitForSeconds(3f);

        for (int i = 0; i<5; i++)
        {

            TorchFuelController TorchController = GameObject.FindGameObjectWithTag("TorchFuelController").transform.GetComponent<TorchFuelController>();
            if (TorchController.TorchWithPlayer1())
            {
                target = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<Player>().transform;
            }
            else
            {
                target = GameObject.FindGameObjectWithTag("Player2").transform.GetComponent<Player>().transform;
            }

            _animator.Play("WalkFixed", PlayMode.StopAll);

            while (true)
            {
                float step = 2f * Time.deltaTime;
                Vector3 targetDir = target.transform.position - transform.position;
                Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
                transform.rotation = Quaternion.LookRotation(newDir);

                Vector3 myDir = transform.forward;
                Vector3 yourDir = target.forward;

                float myAngle = Vector3.Angle(transform.forward, targetDir);

                if (myAngle < 8.0f)
                {
                    break;
                }                
                yield return new WaitForSeconds(0.04f);
            }
            
            //RotateTowards(target, false);
            _animator.Play("Attack", PlayMode.StopAll);
            while (_animator.isPlaying)
            {
                yield return new WaitForSeconds(0.25f);
                if (DEBUG) Debug.Log("Waiting for attack animation to finish");
            }

            if (DEBUG) Debug.Log("Creating fireball");
            GameObject newFireball = (GameObject)Instantiate(Resources.Load("Fireball"));
            Vector3 newPos = transform.TransformPoint(new Vector3(0.2200114f, 7.866667f, 8.053325f));
            newFireball.transform.position = newPos;
            yield return new WaitForSeconds(1f);
        }
               
        _active = true;      
        yield return new WaitForSeconds(3f);     

        _cloud.SetActive(false);
        _shield.SetActive(false);
        pathfinder.enabled = true;
        collider.gameObject.SetActive(false);

        fsm.ChangeState(States.Chase);
    }

    //values that will be set in the Inspector
    /// <summary>   Target for the. </summary>
    public Transform Target;
    /// <summary>   The fireball rotation speed. </summary>
    public float FireballRotationSpeed = 10f;

    //values for internal use
    /// <summary>   The look rotation. </summary>
    private Quaternion _lookRotation;
    /// <summary>   The direction. </summary>
    private Vector3 _direction;

    // Update is called once per frame

    /// <summary>   Rotate towards player. </summary>
    ///
    /// <remarks>   Jack, 21/10/2016. </remarks>
    ///
    /// <returns>   An IEnumerator. </returns>

    IEnumerator RotateTowardsPlayer()
    {
        if (DEBUG) Debug.Log("Begin rotating");
        while (1==1)
        {
            if (DEBUG) Debug.Log("Rotating");
            //find the vector pointing from our position to the target
            _direction = (target.position - transform.position).normalized;

            //create the rotation we need to be in to look at the target
            _lookRotation = Quaternion.LookRotation(_direction);

            //rotate us over time according to speed until we are in the required rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * FireballRotationSpeed);
            yield return new WaitForSeconds(0.1f);
        }
    }

    /// <summary>
    /// Entry method for the chase state. Chooses the closets player and moves towards them. Breaks
    /// if the player leaves the molemans dogs alert area, or comes into attack range.
    /// </summary>
    ///
    /// <remarks>   Jack, 21/10/2016. </remarks>
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

        while (_lockedOn && !isDead)
        {

            if (!_isMoving)
            {
                _animator.Play("WalkFixed", PlayMode.StopAll);
                _isMoving = true;
            }

            //If player 2 is closer to the molemans dog, and is not dead, then chase them Otherwise, player 1 is closer.              
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
            pathfinder.speed = RunSpeed;
            pathfinder.acceleration = 18f;
            pathfinder.angularSpeed = 900f;
            pathfinder.SetDestination(target.position);
            Debug.Log(pathfinder.speed);

            //Every so often sprint at the player
            if (_walkCount > 12)
            {
                if (DEBUG) Debug.Log("molemans dog has started sprinting!");
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
    /// <remarks>   Jack, 21/10/2016. </remarks>
    ///
    /// <returns>   An IEnumerator. </returns>

    IEnumerator Idle_Enter()
    {
        if (DEBUG) Debug.Log("Entered state: Idle");
        float refreshRate = 0.8f;
        _animator.Play("Idle", PlayMode.StopSameLayer);
		BossPanel.SetActive(false);
        //Check to see if either player is within activation range
        while (!_lockedOn)
        {
            if (DEBUG) Debug.Log("Waiting for players.");

            //Move the molemans dog back to its "lair" if there are no targets to chase/attack
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

    /// <summary>   Rotate towards. </summary>
    ///
    /// <remarks>   Jack, 21/10/2016. </remarks>
    ///
    /// <param name="target">   Target for the. </param>
    /// <param name="navMesh">  True to navigation mesh. </param>

    private void RotateTowards(Transform target, bool navMesh)
    {
        if (!navMesh)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * RotationSpeed);
        } else
        {

        }        
    }

    /// <summary>   Death enter. </summary>
    ///
    /// <remarks>   Jack, 21/10/2016. </remarks>

    private void Death_Enter()
    {
        _source.PlayOneShot(Death);
        _cloud.SetActive(false);
		BossPanel.SetActive(false);

        if (DEBUG) Debug.Log("Entered state: Death");
    }

    /// <summary>   True to fireballed once. </summary>
    private bool _fireballedOnce = false;
    /// <summary>   True to fireballed twice. </summary>
    private bool _fireballedTwice = false;

    /// <summary>   Damages. </summary>
    ///
    /// <remarks>   Jack, 21/10/2016. </remarks>
    ///
    /// <param name="amount">   The damage. </param>
    /// <param name="attacker"> The attacker. </param>

    public override void Damage(float amount, Transform attacker)
    {
        if (isDead) return;
        if (fsm.State != States.FireballSpawning)
        {
            base.Damage(amount, attacker);
            _source.PlayOneShot(Hit);
			HealthSlider.value -= amount;

            if (CurrentHealth < 150 && !_fireballedOnce)
            {
                _fireballedOnce = true;
                Debug.Log("Health reduced to first fireballing level");
                fsm.ChangeState(States.FireballSpawning, StateTransition.Overwrite);
                return;
            }

            if (CurrentHealth < 40 && !_fireballedTwice)
            {
                _fireballedTwice = true;
                Debug.Log("Health reduced to first fireballing level");
                fsm.ChangeState(States.FireballSpawning, StateTransition.Overwrite);
                return;
            }
				
            if (DEBUG) Debug.Log("molemans dog damaged");

            if (amount >= CurrentHealth)
            {
                if (DEBUG) Debug.Log("molemans dog killed");
                Killed();
            }
            else
            {
                try
                {
                    _animator.Play("hit1", PlayMode.StopSameLayer);
                }
                catch { }
            }
        }        
    }

    /// <summary>   Boss dead wait. </summary>
    ///
    /// <remarks>   Jack, 21/10/2016. </remarks>
    ///
    /// <returns>   An IEnumerator. </returns>

	public IEnumerator BossDeadWait () 
	{
		yield return new WaitForSeconds(1f);
		EndOfLevelTriggerScript.TriggerEndOfLevel ();
	}

    /// <summary>   Killed this object. </summary>
    ///
    /// <remarks>   Jack, 21/10/2016. </remarks>

    public override void Killed()
    {
        base.Killed();

        var _achievementManager = (AchievementManager)GameObject.FindGameObjectWithTag("AchievementManager").GetComponent(typeof(AchievementManager));
        _achievementManager.AchievementObtained("Dog House.");

        //Stop the pathfinder to prevent the dead entity moving and play the death animation
        try
        {
            pathfinder.Stop();
            _animator.Play("Die", PlayMode.StopAll);
            fsm.ChangeState(States.Death, StateTransition.Overwrite);
            _achievementManager.AchievementObtained("First Blood");

            // Triggering end of level 1 second after boss is defeated
            StartCoroutine(BossDeadWait());

        } catch { }        
    }
}

