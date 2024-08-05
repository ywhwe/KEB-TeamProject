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
    public int playerMotionNum = 5;
    
    public float startTime;
    public float finishTime;
    public PhotonView PV;
    
    protected WaitForSeconds calTime = new WaitForSeconds(5f);
    public GameObject player;
    public CharacterMotionController playerController;
    
    public RectTransform timeBar;
    private float timeBarSize = 500.0f;
    private Vector2 tempSize;

    private bool isTimeBarOn = false;
    
    private void Awake()
    {
        instance = this;
        tempSize = timeBar.sizeDelta;
    }

    private void Start()
    {
        player = CreatePlayer.instance.player1;
        playerController = player.GetComponent<CharacterMotionController>();
        playerController.OnKeyPressed += PlayerInput;
        NetworkManager.instance.isDescending = true;
    }
    
    void Update()
    {
        if (isTimeBarOn)
        {
            tempSize.x += 100f * Time.smoothDeltaTime;
            timeBar.sizeDelta = tempSize;

            if (timeBar.sizeDelta.x >= timeBarSize)
            {
                tempSize.x = 0f;
                timeBar.sizeDelta = tempSize;
            }
        }
    }

    public IEnumerator PlayGameRoutine()
    {
        
        RandomMotion.instance.RandomAction();
        playerMotionNum = 4;
        
        yield return calTime;
        
        playerLife = RandomMotion.instance.CompareMotionNumber(playerMotionNum, playerLife);
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

    private void PlayerInput(int motionNum)
    {
        playerMotionNum = motionNum;
    }

    public void StartGame()
    {
        startTime = Time.time;
        isTimeBarOn = true;
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
    public override void ReadyForStart()
    {
        
    }
}
