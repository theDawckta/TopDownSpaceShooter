using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{
    public StarShip Player;
    public List<StarShip> EnemyList = new List<StarShip>();
    public float SpawnFrequency = 5.0f;
    public float SpawnAreaMin = 3.0f;
    public float SpawnAreaMax = 5.0f;
    public int EnemyMax = 10;
    public float EnemyRange = 10.0f;

    private System.Random random = new System.Random(System.DateTime.Now.Ticks.GetHashCode());
    private int EnemyIndex = 0;
    private List<StarShip> enemies = new List<StarShip>();
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
            if (enemies.Count < EnemyMax && timePassed > SpawnFrequency)
            {
                SpawnEnemy();
                timePassed = 0.0f;
            }
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            if ((enemies[i].transform.position - enemies[i].Target.transform.position).magnitude > EnemyRange || !enemies[i].Alive)
            {
                Destroy(enemies[i].gameObject);
                enemies.Remove(enemies[i]);
            }
        }
    }

    void SpawnEnemy()
    {
        float angle = GetRandomNumber(0.0f, 360.0f);
        float dist = GetRandomNumber(SpawnAreaMin, SpawnAreaMax);
        Vector3 direction = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0.0f).normalized;

        StarShip newEnemy = Instantiate<StarShip>(EnemyList[EnemyIndex], Player.transform.position + direction * dist, Quaternion.Euler(new Vector3(0.0f, 0.0f, angle)));
        newEnemy.Target = Player.gameObject;
        enemies.Add(newEnemy);
        EnemyIndex = (EnemyIndex + 1 < EnemyList.Count) ? EnemyIndex + 1 : 0;
    }

    public float GetRandomNumber(float minimum, float maximum)
    {
        return (float)random.NextDouble() * (maximum - minimum) + minimum;
    }

    public void EndSpawn()
    {
        foreach(StarShip enemyInstance in enemies)
        {
            enemyInstance.Alive = false;
        }
    }

    public void StartSpawn()
    {
        spawning = true;
    }
}
