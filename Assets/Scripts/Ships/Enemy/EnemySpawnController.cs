using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{
    public List<GameObject> EnemyList = new List<GameObject>();
    public float SpawnFrequency = 5.0f;
    public float SpawnAreaMin = 3.0f;
    public float SpawnAreaMax = 5.0f;
    public int EnemyMax = 10;
    public float EnemyRange = 10.0f;

    private System.Random random = new System.Random(System.DateTime.Now.Ticks.GetHashCode());
    private int EnemyIndex = 0;
    private List<StarShip> enemyStarShips = new List<StarShip>();
    private float timePassed = 0.0f;
    private bool spawning;

    void Start()
    {
        spawning = false;
    }

    void Update()
    {
        if(spawning)
        {
            timePassed = timePassed += Time.deltaTime;
            if (enemyStarShips.Count < EnemyMax && timePassed > SpawnFrequency)
            {
                SpawnEnemy();
                timePassed = 0.0f;
            }
        }

        for (int i = 0; i < enemyStarShips.Count; i++)
        {
            if ((enemyStarShips[i].transform.position - transform.position).magnitude > EnemyRange || !enemyStarShips[i].Alive)
            {
                Destroy(enemyStarShips[i].gameObject);
                enemyStarShips.Remove(enemyStarShips[i]);
            }
        }
    }

    void SpawnEnemy()
    {
        float angle = GetRandomNumber(0.0f, 360.0f);
        float dist = GetRandomNumber(SpawnAreaMin, SpawnAreaMax);
        Vector3 direction = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0.0f).normalized;
        GameObject newEnemy = (GameObject)Instantiate(EnemyList[EnemyIndex], transform.position + direction * dist, Quaternion.Euler(new Vector3(0.0f, 0.0f, angle)));
        StarShip enemyStarShip = newEnemy.GetComponent<StarShip>();
        enemyStarShips.Add(enemyStarShip);
        EnemyIndex = (EnemyIndex + 1 < EnemyList.Count) ? EnemyIndex + 1 : 0;
        Debug.Log(transform.position);
    }

    public float GetRandomNumber(float minimum, float maximum)
    {
        return (float)random.NextDouble() * (maximum - minimum) + minimum;
    }

    public void EndSpawn()
    {
        foreach(StarShip enemyInstance in enemyStarShips)
        {
            enemyInstance.Alive = false;
        }
    }

    public void StartSpawn()
    {
        spawning = true;
    }
}
