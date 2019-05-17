using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{
    public delegate void OnEnemyDeath(StarShip deadEnemy);
    public event OnEnemyDeath OnEnemyStarShipDeathEvent;
    public GameObject Player;
    public List<GameObject> EnemyList = new List<GameObject>();
    public float SpawnFrequency = 5.0f;
    public float SpawnAreaMin = 3.0f;
    public float SpawnAreaMax = 5.0f;
    public int EnemyMax = 10;
    public float EnemyRange = 10.0f;

    [HideInInspector]
    public List<StarShip> EnemyStarShips = new List<StarShip>();

    private bool _spawning;
    private System.Random random = new System.Random(System.DateTime.Now.Ticks.GetHashCode());
    private int _enemyIndex = 0;
    private float _timePassed = 0.0f;

    void Start()
    {
        _spawning = false;
    }

    void Update()
    {
        if(_spawning)
        {
            _timePassed = _timePassed + Time.deltaTime;
            if (EnemyStarShips.Count < EnemyMax && _timePassed > SpawnFrequency)
            {
                SpawnEnemy();
                _timePassed = 0.0f;
            }
        }

   //     for (int i = 0; i < EnemyStarShips.Count; i++)
   //     {
   //         if ((EnemyStarShips[i].transform.position - Player.transform.position).magnitude > EnemyRange)
			//{
   //             EnemyStarShips[i].Alive = false;
   //             EnemyStarShips.Remove(EnemyStarShips[i]);
   //         }
   //     }
    }

    public StarShip SpawnEnemy()
    {
        float angle = UtilityFunctions.GetRandomNumber(0.0f, 360.0f, random);
        float dist = UtilityFunctions.GetRandomNumber(SpawnAreaMin, SpawnAreaMax, random);
        Vector3 direction = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0.0f).normalized;
        GameObject newEnemy = (GameObject)Instantiate(EnemyList[_enemyIndex], Player.transform.position + direction * dist, Quaternion.Euler(new Vector3(0.0f, 0.0f, angle)));
        StarShip enemyStarShip = newEnemy.GetComponent<StarShip>();
        enemyStarShip.OnDeath += EnemyStarShipDeath;
        EnemyStarShips.Add(enemyStarShip);
        _enemyIndex = (_enemyIndex + 1 < EnemyList.Count) ? _enemyIndex + 1 : 0;

        return enemyStarShip;
    }

    private void EnemyStarShipDeath(StarShip enemy)
    {
        EnemyStarShips.Remove(enemy);
        OnEnemyStarShipDeathEvent(enemy);
    }

    //public float GetRandomNumber(float minimum, float maximum)
    //{
    //    return (float)random.NextDouble() * (maximum - minimum) + minimum;
    //}

    public void EndSpawn()
    {
    	_spawning = false;
    	
        foreach(StarShip enemyInstance in EnemyStarShips)
        {
            Destroy(enemyInstance.gameObject);
        }

        EnemyStarShips.Clear();
    }

    public void StartSpawn()
    {
        _spawning = true;
    }
}
