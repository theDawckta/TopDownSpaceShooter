using UnityEngine;

public class Enemy1 : StatePatternEnemy
{
    protected override void Awake()
    {
        Debug.Log("awake");
        base.Awake();
    }

    protected override void Start()
    {
        currentState = patrolState;
        base.Start();
    }

    protected override void Update()
    {
        currentState.UpdateState();
    }

    private void OnTriggerEnter(Collider other)
    {
        currentState.OnTriggerEnter(other);
    }
}