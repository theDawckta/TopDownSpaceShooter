using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{
    public EnemyController Enemy;
    public GameObject Player;
    public float SpawnFrequency = 5.0f;
    public float SpawnAreaMin = 3.0f;
    public float SpawnAreaMax = 5.0f;
    public int MaxEnemies = 10;
    public float EnemyRange = 10.0f;

    private System.Random random = new System.Random(System.DateTime.Now.Ticks.GetHashCode());
    private bool spawning = false;
    private List<EnemyController> enemies = new List<EnemyController>();

    void Start()
    {
        StartCoroutine("SpawnTimer");
    }

    void Update()
    {
        if (!spawning && enemies.Count < MaxEnemies)
        {
            StartCoroutine("SpawnTimer");
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            if ((enemies[i].transform.position - enemies[i].Player.transform.position).magnitude > EnemyRange || enemies[i].Alive == false)
            {

                Destroy(enemies[i].gameObject);
                enemies.Remove(enemies[i]);
                
            }
        }
    }

    IEnumerator SpawnTimer()
    {
        float angle = GetRandomNumber(0.0f, 360.0f);
        float dist = GetRandomNumber(SpawnAreaMin, SpawnAreaMax);
        
        spawning = true;

        EnemyController enemy = Instantiate<EnemyController>(Enemy, Player.transform.position, Quaternion.Euler(new Vector3(0.0f, 0.0f, angle)));
        enemy.transform.parent = transform.root;
        enemy.transform.position = enemy.transform.up * dist;
        enemy.Player = Player;
        enemies.Add(enemy);

        float timePassed = 0.0f;
        while ((timePassed / SpawnFrequency) <= 1)
        {
            timePassed = timePassed + Time.deltaTime;
            yield return null;
        }
        spawning = false;
    }

    public float GetRandomNumber(float minimum, float maximum)
    {
        return (float)random.NextDouble() * (maximum - minimum) + minimum;
    }
}
