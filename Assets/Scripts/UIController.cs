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
        Player.OnPlayerFuelPickup += PlayerFuelPickup;
        Player.EnablePlayer();
    }

    public void GameOn()
    {
        //Level = 1;
        //Score = 0;
        //KillCount = 0;

        GameUI.SetActive(true);
        StartUI.SetActive(false);
        WarpSlider.value = 0.0f;
    }

    public void GameOff()
    {
        GameUI.SetActive(false);
        StartUI.SetActive(true);
        WarpSlider.value = 0.0f;
    }

    public void PlayerFuelPickup()
    {
		WarpSlider.value = Player.PlayerFuelLevel;
		if(WarpSlider.value == 1)
		{
            GameOff();
			EnemySpawn.EndSpawn();
		}
    }
}
