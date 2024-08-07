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

    private IEnumerator compareIsPressed;
    
    public int playerLife = 3;
    public Image[] lifeImage;
    public int playerMotionNum = 5;
    public AudioSource BGM;
    
    public float startTime;
    public float finishTime;
    public PhotonView PV;
    
    protected WaitForSeconds calTime = new WaitForSeconds(3f);
    private float tempTime = 3f;
    public GameObject player;
    public CharacterMotionController playerController;
    
    public RectTransform timeBar;
    public RectTransform timeBarBackground;
    public float timeBarSize;
    private Vector2 tempSize;

    public bool isTimeBarOn = false;
    public TextMeshProUGUI text;
    public float stageTimer = 0f;
    private float stageUpTime = 3f;

    public bool isPressed = false;
    
    private void Awake()
    {
        instance = this;
        tempSize = timeBar.sizeDelta;
        GameObject temp = Instantiate(TotalManager.instance.playerPrefab, player.transform.position, player.transform.rotation);
        temp.transform.SetParent(player.transform);
        player = temp;
        tempSize = timeBarBackground.sizeDelta;
        timeBarSize = tempSize.x;
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

    public IEnumerator CompareIsPressed()
    {
        yield return new WaitUntil(()=>isPressed);
        playerLife = RandomMotion.instance.CompareMotionNumber(playerMotionNum, playerLife);
        LifeImageDelete();
    }

    public void StartCoroutineCompareIsPressed()
    {
        compareIsPressed = CompareIsPressed();
        StartCoroutine(compareIsPressed);
    }
    
    public void StopCoroutineCompareIsPressed()
    {
        StopCoroutine(compareIsPressed);
    }

    public IEnumerator PlayGameRoutine()
    {
        RandomMotion.instance.RandomAction();
        playerMotionNum = 4;
        isPressed = false;

        StartCoroutineCompareIsPressed();
        
        yield return calTime;

        if (!isPressed)
        {
            playerLife = RandomMotion.instance.CompareMotionNumber(playerMotionNum, playerLife);
            LifeImageDelete();
            isPressed = true;
        }
        
        stageTimer++;
        
        StopCoroutineCompareIsPressed();
        if (playerLife == 0)
        {
            EndGame();
        } 
        else if (stageTimer >= stageUpTime)
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
        Nextround();
        isTimeBarOn = false;
        yield return new WaitForSeconds(1f);
        isTimeBarOn = true;
        text.text = "";
        Time.timeScale += 0.2f;
        stageTimer = 0f;
        stageUpTime += 0.5f;
        StartCoroutine(PlayGameRoutine());
    }

    public void Nextround()
    {
        tempSize.x = timeBarSize - 20f;
        timeBarBackground.sizeDelta = tempSize;
        timeBarSize = tempSize.x;
        tempTime -= 0.2f;
        calTime = new WaitForSeconds(tempTime);
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
        if (!isPressed)
        {
            playerMotionNum = motionNum;
            isPressed = true;
        }
        
    }

    public void StartGame()
    {
        startTime = Time.time;
        isTimeBarOn = true;
        StartCoroutine(PlayGameRoutine());
    }

    public void EndGame()
    {
        isTimeBarOn = false;
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
    public override void ReadyForStart()
    {
        
    }
}
