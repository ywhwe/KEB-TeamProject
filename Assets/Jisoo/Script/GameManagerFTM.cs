using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameManagerFTM : WholeGameManager
{
    public static GameManagerFTM instance;
    
    public int playerLife = 3;
    public Image[] lifeImage;
    public int playerMotionNum = 5;
    public AudioSource BGM;
    
    public float startTime;
    public float finishTime;
    public PhotonView PV;
    
    protected WaitForSeconds calTime = new WaitForSeconds(5f);
    public GameObject player;
    public CharacterMotionController playerController;
    
    public RectTransform timeBar;
    private float timeBarSize = 500.0f;
    private Vector2 tempSize;

    public bool isTimeBarOn = false;
    public TextMeshProUGUI text;
    public int stageTimer = 0;
    public int stageUpTime = 4;
    
    private void Awake()
    {
        instance = this;
        tempSize = timeBar.sizeDelta;
        GameObject temp = Instantiate(TotalManager.instance.playerPrefab, player.transform.position, player.transform.rotation);
        temp.transform.SetParent(player.transform);
        player = temp;
    }

    private void Start()
    {
        BGM.Play();
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

        stageTimer++;
        if (playerLife == 0)
        {
            EndGame();
        }

        if (stageTimer >= stageUpTime)
        {
            StartCoroutine(FasterText());
        }
        else
        {
            StartCoroutine(NextText());
            StartCoroutine(PlayGameRoutine());
        }
    }

    public IEnumerator NextText()
    {
        text.text = "Next";
        yield return new WaitForSeconds(1f);
        text.text = "";
    }

    public IEnumerator FasterText()
    {
        SoundManagerForFollowTheMotion.instance.PlaySound("StageUp");
        text.text = "Faster";
        isTimeBarOn = false;
        yield return new WaitForSeconds(1f);
        isTimeBarOn = true;
        text.text = "";
        Time.timeScale += 0.5f;
        stageTimer = 0;
        stageUpTime++;
        StartCoroutine(PlayGameRoutine());
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
        BGM.Stop();
        Time.timeScale = 1f;
        stageTimer = 0;
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
