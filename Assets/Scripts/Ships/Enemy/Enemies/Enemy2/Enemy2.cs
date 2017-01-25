using UnityEngine;

public class Enemy2 : StatePatternEnemy
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        currentState = patrolState;
        base.Start();
    }

    protected override void Update()
    {
        Look();
        currentState.UpdateState();
        base.Update();
    }

    private void OnTriggerEnter(Collider other)
    {
        currentState.OnTriggerEnter(other);
    }

    private void Look()
    {
        RaycastHit2D hit = Physics2D.Raycast((Vector2)Eyes.transform.position, (Vector2)Eyes.transform.forward, 20);

        if (hit)
        {
            if (hit.collider.tag == "Player")
            {
                chaseTarget = hit.transform;
                currentState.ToChaseState();
            }
            if (hit.collider.tag == "Enemy")
            {
                currentState.ToEvadeState();
            }
        }

        Debug.DrawRay(Eyes.transform.position, Eyes.transform.forward * SightRange, Color.red);
    }
}
