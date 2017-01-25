using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StatePatternEnemy : StarShip
{
    public Vector3 WayPoint = new Vector3();
    public float SearchingTurnSpeed = 12.0f;
    public float SearchingDuration = 4f;
    public float SightRange = 20f;
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
    public EvadeState evadeState;
    [HideInInspector]
    public AttackState attackState;
    [HideInInspector]
    public PatrolState patrolState;
    [HideInInspector]
    public Rigidbody2D enemyRigidbody;

    protected override void Awake()
    {
        chaseState = new ChaseState(this);
        evadeState = new EvadeState(this);
        attackState = new AttackState(this);
        patrolState = new PatrolState(this);
        enemyRigidbody = transform.GetComponent<Rigidbody2D>();
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    private void OnTriggerEnter(Collider other)
    {
        currentState.OnTriggerEnter(other);
    }
}