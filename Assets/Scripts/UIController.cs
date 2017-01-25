using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour 
{
    //private int Level;
    //private int Score;
    //private int KillCount;
    public PlayerController Player;
    public EnemySpawnController EnemySpawn;
    public GameObject StartUI;
    public GameObject GameUI;
    public Slider WarpSlider;

    void Start()
    {
        Player.OnPlayerDied += PlayerDied;
        Player.OnPlayerFuelPickup += PlayerFuelPickup;
    }

    public void ReStart()
    {
        //Level = 1;
        //Score = 0;
        //KillCount = 0;

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

    public void PlayerFuelPickup()
    {
		WarpSlider.value = Player.PlayerFuelLevel;
		if(WarpSlider.value == 1)
		{
			End();
			EnemySpawn.EndSpawn();
		}
    }

    public void PlayerDied()
    {
        End();
        EnemySpawn.EndSpawn();
    }
}
