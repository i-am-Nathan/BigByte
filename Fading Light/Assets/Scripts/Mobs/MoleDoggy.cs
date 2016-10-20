using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;
using UnityEngine.UI;
using Assets.Scripts.Mobs;

/// <summary>
/// Controls the AI (using FSM) of the large molemans dog bosses (e.i. the one found in the tutorial level)
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class MoleDoggy : BaseEntity
{
	//molemans dog states
	public enum States
	{
		Init,
		Idle,
		Chase,
        Attack,
        Taunt,
        FireballSpawning,
        Death
	}

    //molemans dog stats
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
    private bool _active = false;

    private bool DEBUG = true;

	private AchievementManager _achievementManager;

    private GameObject _cloud;

    public AudioClip Hit;
    public AudioClip Death;
    public AudioClip Attack;
    public AudioClip AOE;
    private AudioSource _source;

    /// <summary>
    /// Initilized montser location, pathfinding, animation and the AI FSM
    /// </summary>
    private void Awake()
	{
        _cloud = GameObject.Find("AOE");
        _cloud.SetActive(false);
        _source = GetComponent<AudioSource>();

        if (DEBUG) Debug.Log("The molemans dog wakes.");
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
        if (DEBUG) Debug.Log("molemans dog state machine initilized.");
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

    private bool _isRotating = false;

    //http://gamedev.stackexchange.com/questions/102126/unable-to-stop-navmeshagent-for-rotation-before-moving
    IEnumerator RotateAgent(Quaternion currentRotation, Quaternion targetRotation)
    {
        _isRotating = true;
        while (currentRotation != targetRotation) {
            transform.rotation = Quaternion.RotateTowards(currentRotation, targetRotation, RotationSpeed * Time.deltaTime);
            yield return 1;
        }
        _isRotating = false;
    }

    IEnumerator FireballSpawning_Enter()
    {        
        _cloud.SetActive(true);
        pathfinder.enabled = false;

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
        pathfinder.enabled = true;

        fsm.ChangeState(States.Chase);
    }

    //values that will be set in the Inspector
    public Transform Target;
    public float FireballRotationSpeed = 10f;

    //values for internal use
    private Quaternion _lookRotation;
    private Vector3 _direction;

    // Update is called once per frame
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
    /// Entry method for the chase state. Chooses the closets player and moves towards them. Breaks if the player leaves the 
    /// molemans dogs alert area, or comes into attack range.
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
    /// Entry state for the idle state. Waits in place and constantly checks to see if any players have entered its alert area. If a player enters the area
    /// if transitions to the chase state to chase them down.
    /// </summary>
    /// <returns></returns>
    IEnumerator Idle_Enter()
    {
        if (DEBUG) Debug.Log("Entered state: Idle");
        float refreshRate = 0.8f;
        _animator.Play("Idle", PlayMode.StopSameLayer);

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

    private void Death_Enter()
    {
        _source.PlayOneShot(Death);
        _cloud.SetActive(false);
        if (DEBUG) Debug.Log("Entered state: Death");
    }

    private bool _fireballedOnce = false;
    private bool _fireballedTwice = false;

    public override void Damage(float amount, Transform attacker)
    {
        if (isDead) return;
        if (fsm.State != States.FireballSpawning)
        {
            base.Damage(amount, attacker);
            _source.PlayOneShot(Hit);

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

            // Set the health bar's value to the current health.
            try
            {
                healthCircle.enabled = true;
                healthCircle.fillAmount -= amount / base.IntialHealth;
                Debug.Log("YOYOYOYO " + healthCircle.fillAmount);
                Invoke("HideHealth", 3);
            }
            catch { }


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

    public override void Killed()
    {
        base.Killed();

        //Stop the pathfinder to prevent the dead entity moving and play the death animation
        try
        {
            pathfinder.Stop();
            _animator.Play("Die", PlayMode.StopAll);
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

