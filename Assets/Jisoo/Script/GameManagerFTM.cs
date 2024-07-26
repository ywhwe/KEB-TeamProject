using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameManagerFTM : WholeGameManager
{
    public static GameManagerFTM instance;
    
    public int playerLife = 3;
    public Image[] lifeImage;
    
    public float startTime;
    public float finishTime;
    public PhotonView PV;
    
    protected WaitForSeconds calTime = new WaitForSeconds(5f);
    public GameObject player;
    protected int player1Motion;
    
    private void Awake()
    {
        instance = this;
        
    }

    private void Start()
    {
        player = CreatePlayer.instance.player1;
        NetworkManager.instance.isDescending = true;
    }
    
    void Update()
    {
        player1Motion = player.GetComponent<CharacterControl>().motionNumber;
    }

    public IEnumerator PlayGameRoutine()
    {
        RandomMotion.instance.RandomAction();

        yield return calTime;
        
        playerLife = RandomMotion.instance.CompareMotionNumber(player1Motion, playerLife);
        LifeImageDelete();
        
        if (playerLife == 0)
        {
            EndGame();
        }
        else
        {
            StartCoroutine(PlayGameRoutine());   
        }
    }

    public void LifeImageDelete()
    {
        switch (playerLife)
        {
            case 2:
                lifeImage[playerLife].gameObject.SetActive(false);
                break;
            case 1:
                lifeImage[playerLife].gameObject.SetActive(false);
                break;
            case 0:
                lifeImage[playerLife].gameObject.SetActive(false);
                break;
        }
    }

    public void StartGame()
    {
        startTime = Time.time;
        
        StartCoroutine(PlayGameRoutine());
    }

    public void EndGame()
    {
        Time.timeScale = 1f;
        RandomMotion.instance.stage = 0;
        finishTime = Time.time;
        score = finishTime - startTime;
        TotalManager.instance.StartFinish();
    }
    
    public override void GameStart()
    {
        StartGame();
    }

    public override void SpawnObsPlayer()
    {
        
    }
}
