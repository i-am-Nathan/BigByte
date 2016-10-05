using UnityEngine;
using System.Collections;

public class StatePatternEnemy : MonoBehaviour {

    //Spider stats
    public float HardActivationDistance = 70;
    public float LooseActivationDistance = 100;
    public float AttackSpeed = 1;
    public float AttackDamage = 10;
    public float Health = 50;
    public float AttackRange = 16;
    public float Range = .1f;
    public float WalkSpeed = 9;
    public float RunSpeed = 15;
    public float SprintSpeed = 24;
    public Transform eyes;

    private float _nextAttackTime;
    private float _collisionRange;
    private float _targetCollisionRange;
    private bool _lockedOn;
    private bool _inAttackRange;
    private bool _isRunning;
    private bool _isMoving;

    //Target and navigation variables
    NavMeshAgent pathfinder;
    Transform target;
    BaseEntity targetBaseEntity;
    Material skinMaterial;
    Color originalColour;
    Animation _animator;
    Vector3 spawnLocation;


    [HideInInspector] public Transform playerTarget;
    [HideInInspector] public IEnemyState currentState;
    [HideInInspector] public ChaseState chaseState;
    [HideInInspector] public AlertState alertState;
    [HideInInspector] public IdleState idleState;
    [HideInInspector] public AttackState attackState;
    [HideInInspector] public NavMeshAgent navMeshAgent;

    private void Awake()
    {
        chaseState = new ChaseState(this);
        alertState = new AlertState(this);
        idleState = new IdleState(this);
        attackState = new AttackState(this);

        navMeshAgent = GetComponent<NavMeshAgent>();

    }

    // Use this for initialization
    void Start () {
        currentState = idleState;
	}

    //Since this is a monobehaviour can call it
    private void OnTriggerEnter(Collider other)
    {
        currentState.OnTriggerEnter(other);
    }
	
	// Update is called once per frame
	void Update () {
        currentState.UpdateState();
	}
}
