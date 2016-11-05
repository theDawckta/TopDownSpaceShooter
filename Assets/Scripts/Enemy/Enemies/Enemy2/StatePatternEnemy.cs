using UnityEngine;
using System.Collections;

public class StatePatternEnemy : MonoBehaviour
{
    public float MaxSpeed;
    public float Acceleration = 10.0f;
    public float TurnSpeed = 10.0f;
    public float SearchingTurnSpeed = 12.0f;
    public float SearchingDuration = 4f;
    public float SightRange = 20f;
    public Transform[] WayPoints;
    public Transform Eyes;
    public GameObject RollRotation;
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
    private void Awake()
    {
        chaseState = new ChaseState(this);
        alertState = new AlertState(this);
        patrolState = new PatrolState(this);
        enemyRigidbody = transform.GetComponent<Rigidbody2D>();
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