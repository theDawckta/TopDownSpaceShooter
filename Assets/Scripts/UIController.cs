using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour 
{
    private int Level;
    private int Score;
    private int KillCount;
    public PlayerController Player;
    public EnemySpawnController EnemySpawn;
    public GameObject StartUI;
    public GameObject GameUI;

    void Start()
    {
        Player.OnPlayerDied += PlayerDied;
    }

    public void ReStart()
    {
        Level = 1;
        Score = 0;
        KillCount = 0;

        Player.EnablePlayer();
        GameUI.SetActive(true);
        StartUI.SetActive(false);
        EnemySpawn.StartSpawn();
    }

    public void End()
    {
        Player.DisablePlayer();
        GameUI.SetActive(false);
        StartUI.SetActive(true);
    }

    public void PlayerDied()
    {
        End();
        EnemySpawn.EndSpawn();
    }
}
