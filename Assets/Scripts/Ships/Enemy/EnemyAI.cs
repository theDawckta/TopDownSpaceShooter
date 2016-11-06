using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

    public float HitPoints = 3.0f;
    public ENEMY_STATE states;

    public enum ENEMY_STATE
    {
        INACTIVE = 0,
        IDLE = 1,
        ATTACK = 2,
        DEAD = 3
    }
    
    void Awake()
    {
        states = ENEMY_STATE.INACTIVE;
    }

	// Use this for initialization
    void Start()
    {
        StartCoroutine(EnemyFSM());
    }

    IEnumerator EnemyFSM()
    {
        while (true)
        {
            yield return StartCoroutine(states.ToString());
        }
    }

    IEnumerator INACTIVE()
    {
        while (states == ENEMY_STATE.INACTIVE)
        {
            yield return null;
        }
    }

    IEnumerator IDLE()
    {
        while (states == ENEMY_STATE.IDLE)
        {
            yield return StartCoroutine("HandleIdle");
        }
    }

    IEnumerator ATTACK()
    {
        while (states == ENEMY_STATE.ATTACK)
        {
            yield return StartCoroutine("HandleAttack");
        }
    }

    IEnumerator DEAD()
    {
        while (states == ENEMY_STATE.DEAD)
        {
            yield return StartCoroutine("HandleDead");
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
            states = ENEMY_STATE.IDLE;
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
            states = ENEMY_STATE.INACTIVE;
    }

    public void TakeDamage(float damage)
    {
        HitPoints = HitPoints - damage;
        if (HitPoints <= 0)
            states = ENEMY_STATE.DEAD;
    }
}
