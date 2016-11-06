using UnityEngine;
using System.Collections;

public class StatePatternEnemy : StarShip
{
    public float SearchingTurnSpeed = 12.0f;
    public float SearchingDuration = 4f;
    public float SightRange = 20f;
    public Transform[] WayPoints;
    public Transform Eyes;
    public Vector3 Offset = new Vector3(0, .5f, 0);
    public MeshRenderer MeshRendererFlag;
    [HideInInspector]
    public Transform chaseTarget;
    [HideInInspector]
    public IEnemyState currentState;
    [HideInInspector]
    public ChaseState chaseState;
    [HideInInspector]
    public AlertState alertState;
    [HideInInspector]
    public PatrolState patrolState;
    [HideInInspector]
    public Rigidbody2D enemyRigidbody;
    protected virtual void Awake()
    {
        chaseState = new ChaseState(this);
        alertState = new AlertState(this);
        patrolState = new PatrolState(this);
        enemyRigidbody = transform.GetComponent<Rigidbody2D>();
        base.Awake();
    }

    // Use this for initialization
    void Start()
    {
        currentState = patrolState;
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState();
    }

    private void OnTriggerEnter(Collider other)
    {
        currentState.OnTriggerEnter(other);
    }
}